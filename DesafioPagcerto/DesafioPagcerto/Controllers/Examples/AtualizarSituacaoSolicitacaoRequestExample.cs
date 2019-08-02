using DesafioPagcerto.Model.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace DesafioPagcerto.Controllers.Examples
{
    public class AtualizarSituacaoSolicitacaoRequestExample : IExamplesProvider
    { 
        public object GetExamples()
        {
            return new AtualizarSituacaoSolicitacaoRequest
            {
                Situacao = 1
            };
        }
    }
}
