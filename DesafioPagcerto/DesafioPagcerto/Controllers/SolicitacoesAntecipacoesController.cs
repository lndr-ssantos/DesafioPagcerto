using DesafioPagcerto.Controllers.Examples;
using DesafioPagcerto.Model.EntityModel;
using DesafioPagcerto.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using static DesafioPagcerto.Model.EntityModel.SolicitacaoRepasseAntecipado;

namespace DesafioPagcerto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitacoesAntecipacoesController : ControllerBase
    {
        private readonly PagcertoContext _context;

        public SolicitacoesAntecipacoesController(PagcertoContext context)
        {
            _context = context;
        }

        [HttpGet("solicitacoes-atencipacoes/{id}", Name = "Consultar detalhes de solicitação")]
        public IActionResult GetDetalhesSolicitacao(int id)
        {
            try
            {
                var solicitacao = _context.SolicitacaoRepasseAntecipados
                        .Where(x => x.Id == id)
                        .Include(t => t.Transacoes)
                        .Select(x => new DetalhesSolicitacaoResponse()
                        {
                            Id = x.Id,
                            Situacao = x.Situacao ?? 0,
                            Status = x.Status,
                            DataAnaliseInicio = x.DataAnaliseInicio,
                            DataAnaliseFim = x.DataAnaliseFim,
                            DataSolicitacao = x.DataSolicitacao,
                            Transacoes = (List<Transacao>)x.Transacoes
                        })
                        .Single();

                if (solicitacao != null)
                {
                    return Ok(solicitacao);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("solicitacoes-atencipacoes-disponiveis/{clienteId}", Name = "Obter transações disponíveis para antecipacao")]
        public IActionResult GetTransacoesDisponiveisParaAntecipacao(int clienteId)
        {
            try
            {
                var transacoes = _context.Transacoes
                        .Where(x => x.ClientId == clienteId && x.SolicitacaoRepasseId == null).ToList();

                if (transacoes.Count > 0)
                {
                    var propostasAntecipacao = new AntecipacoesDisponiveisResponse(transacoes);
                    return Ok(propostasAntecipacao);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("solicitacoes-atencipacoes-por-periodo")]
        public IActionResult GetSolicitacoesByPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                if (dataFim < dataInicio)
                {
                    var dataAux = dataInicio;
                    dataInicio = dataFim;
                    dataFim = dataAux;
                }

                var solicitacoes = _context.SolicitacaoRepasseAntecipados
                    .Where(x => x.DataSolicitacao.Date >= dataInicio && x.DataSolicitacao.Date <= dataFim)
                    .Include(t => t.Transacoes)
                    .Select(x => new SolicitacoesPorPeriodoResponse()
                    {
                        Id = x.Id,
                        Situacao = x.Situacao ?? 0,
                        Status = x.Status,
                        DataAnaliseInicio = x.DataAnaliseInicio,
                        DataAnaliseFim = x.DataAnaliseFim,
                        DataSolicitacao = x.DataSolicitacao.Date,
                        Transacoes = (List<Transacao>)x.Transacoes
                    })
                    .ToList();
                if (solicitacoes.Count > 0)
                {
                    return Ok(solicitacoes);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("solicitacoes-antecipacoes")]
        [SwaggerRequestExample(typeof(SolicitacaoAntecipacaoRequest), typeof(SolicitacaoAntecipacaoRequestExample))]
        public IActionResult PostSolicitacao([FromBody] SolicitacaoAntecipacaoRequest solicitacaoAntecipacaoRequest)
        {
            try
            {
                var transacoes = _context.Transacoes
                        .Where(x => solicitacaoAntecipacaoRequest.Transacoes.Contains(x.Id)
                                && x.SolicitacaoRepasseId == null
                                && x.ClientId == solicitacaoAntecipacaoRequest.ClienteId)
                        .ToList();

                if (transacoes.Count > 0)
                {
                    var solicitacaoRepasse = new SolicitacaoRepasseAntecipado(transacoes);

                    _context.SolicitacaoRepasseAntecipados.Add(solicitacaoRepasse);

                    _context.SaveChanges();
                    return Created("Solicitacao", solicitacaoRepasse);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("solicitacoes-antecipacoes/{id}/atendimento/inicio")]
        //[SwaggerRequestExample(typeof(AtualizarStatusSolicitacaoRequest), typeof(AtualizarStatusSolicitacaoRequestExample))]
        public IActionResult PutIniciarAtendimentoSolicitacao(int id)
        {
            try
            {
                var solicitacao = _context.SolicitacaoRepasseAntecipados.Where(x => x.Id == id).Single();

                if (solicitacao.Status == (int)EStatus.AguardandoAnalise)
                {
                    solicitacao.Status = (int)EStatus.EmAnalise;
                    solicitacao.DataAnaliseInicio = DateTime.Now;
                    _context.SaveChanges();
                    return NoContent();
                }

                return BadRequest("Análise de solicitação já iniciada");                
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpPut("solicitacoes-antecipacoes/{id}/atendimento/fim")]
        [SwaggerRequestExample(typeof(AtualizarSituacaoSolicitacaoRequest), typeof(AtualizarSituacaoSolicitacaoRequestExample))]
        public IActionResult PutFinalizarSolicitacao(int id, [FromBody] AtualizarSituacaoSolicitacaoRequest situacao)
        {
            try
            {
                var solicitacao = _context.SolicitacaoRepasseAntecipados.Where(x => x.Id == id).Single();

                if (solicitacao.Status == (int)EStatus.EmAnalise && solicitacao.Situacao == null)
                {
                    solicitacao.Situacao = situacao.Situacao;
                    solicitacao.Status = (int)EStatus.Finalizada;
                    solicitacao.DataAnaliseFim = DateTime.Now;

                    _context.SaveChanges();
                    return NoContent();
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
    }
}
