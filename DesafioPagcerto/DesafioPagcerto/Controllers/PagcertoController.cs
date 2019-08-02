using DesafioPagcerto.Controllers.Examples;
using DesafioPagcerto.Model.EntityModel;
using DesafioPagcerto.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
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

        // GET: api/Pagcerto
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Pagcerto/5
        [HttpGet("{clienteId}", Name = "ObterTransacoesParaAntecipacao")]
        public IActionResult Get(int clienteId)
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

        // POST: api/Pagcerto
        [HttpPost]
        [Route("transacao")]
        [SwaggerRequestExample(typeof(TransacaoRequest), typeof(TransacaoRequestExample))]
        public IActionResult PostTransacao([FromBody] TransacaoRequest transacaoRequest)
        {
            var transacao = new Transacao(transacaoRequest);
            _context.Transacoes.Add(transacao);
            _context.SaveChanges();
          
            return Created("Transacao", transacao);
        }

        [HttpPost]
        [Route("solicitacao")]
        [SwaggerRequestExample(typeof(SolicitacaoAntecipacaoRequest), typeof(SolicitacaoAntecipacaoRequestExample))]
        public IActionResult PostSolicitacao([FromBody] SolicitacaoAntecipacaoRequest solicitacaoAntecipacaoRequest)
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
            else
            {
                return NotFound();
            }
        }

        [HttpPut("atender-solicitacao/{id}")]
        //[Route("atualizar-status-solicitacao")]
        [SwaggerRequestExample(typeof(AtualizarStatusSolicitacaoRequest), typeof(AtualizarStatusSolicitacaoRequestExample))]
        public IActionResult Put(int id, [FromBody] AtualizarStatusSolicitacaoRequest status)
        {
            var solicitacao = _context.SolicitacaoRepasseAntecipados.Where(x => x.Id == id).Single();

            if (status.Status == 2 && solicitacao.Status == (int)EStatus.AguardandoAnalise)
            {
                solicitacao.Status = status.Status;
            }

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
