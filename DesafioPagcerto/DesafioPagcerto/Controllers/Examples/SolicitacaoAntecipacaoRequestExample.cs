using DesafioPagcerto.Model.ViewModel;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace DesafioPagcerto.Controllers.Examples
{
    public class SolicitacaoAntecipacaoRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SolicitacaoAntecipacaoRequest
            {
                ClienteId = 2,
                Transacoes = new List<int>()
                {
                    1,
                    2
                }
            };
        }
    }
}
