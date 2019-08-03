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
    public class PagcertoController : ControllerBase
    {
        private readonly PagcertoContext _context;

        public PagcertoController(PagcertoContext context)
        {
            _context = context;
        }

        [HttpGet("solicitacao/{id}", Name = "Consultar detalhes de solicitação")]
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

        [HttpGet("solicitacao-disponivel-antecipacao/{clienteId}", Name = "ObterTransacoesParaAntecipacao")]
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

        [HttpGet("solicitacoes-por-periodo")]
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
        [Route("transacao")]
        [SwaggerRequestExample(typeof(TransacaoRequest), typeof(TransacaoRequestExample))]
        public IActionResult PostTransacao([FromBody] TransacaoRequest transacaoRequest)
        {
            try
            {
                var transacao = new Transacao(transacaoRequest);
                _context.Transacoes.Add(transacao);
                _context.SaveChanges();

                return Created("Transacao", transacao);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("solicitacao")]
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

        [HttpPut("atender-solicitacao/{id}")]
        //[Route("atualizar-status-solicitacao")]
        [SwaggerRequestExample(typeof(AtualizarStatusSolicitacaoRequest), typeof(AtualizarStatusSolicitacaoRequestExample))]
        public IActionResult Put(int id, [FromBody] AtualizarStatusSolicitacaoRequest status)
        {
            try
            {
                var solicitacao = _context.SolicitacaoRepasseAntecipados.Where(x => x.Id == id).Single();

                if (status.Status == 2 && solicitacao.Status == (int)EStatus.AguardandoAnalise)
                {
                    solicitacao.Status = status.Status;
                    solicitacao.DataAnaliseInicio = DateTime.Now;
                }

                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpPut("atualizar-situacao/{id}")]
        [SwaggerRequestExample(typeof(AtualizarSituacaoSolicitacaoRequest), typeof(AtualizarSituacaoSolicitacaoRequestExample))]
        public IActionResult Delete(int id, [FromBody] AtualizarSituacaoSolicitacaoRequest situacao)
        {
            try
            {
                var solicitacao = _context.SolicitacaoRepasseAntecipados.Where(x => x.Id == id).Single();

                if (solicitacao.Status == (int)EStatus.EmAnalise && solicitacao.Situacao == null)
                {
                    solicitacao.Situacao = situacao.Situacao;
                    solicitacao.Status = (int)EStatus.Finalizada;
                    solicitacao.DataAnaliseFim = DateTime.Now;
                }

                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
    }
}
