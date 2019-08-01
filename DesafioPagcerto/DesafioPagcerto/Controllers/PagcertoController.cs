using DesafioPagcerto.Controllers.Examples;
using DesafioPagcerto.Model.EntityModel;
using DesafioPagcerto.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

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
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Pagcerto
        [HttpPost]
        public IActionResult PostTransacao([FromBody] TransacaoRequest transacaoRequest)
        {
            var transacao = new Transacao(transacaoRequest);
            _context.Transacoes.Add(transacao);
            _context.SaveChanges();
          
            return Created("Transacao", transacao);
        }

        // PUT: api/Pagcerto/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
