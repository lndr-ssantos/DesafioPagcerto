using DesafioPagcerto.Controllers.Examples;
using DesafioPagcerto.Model.EntityModel;
using DesafioPagcerto.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace DesafioPagcerto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransacoesController : ControllerBase
    {
        private readonly PagcertoContext _context;

        public TransacoesController(PagcertoContext context)
        {
            _context = context;
        }

        [HttpPost]
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
    }
}