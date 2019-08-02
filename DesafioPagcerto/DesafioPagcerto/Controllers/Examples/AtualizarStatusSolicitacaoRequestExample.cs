using DesafioPagcerto.Model.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace DesafioPagcerto.Controllers.Examples
{
    public class AtualizarStatusSolicitacaoRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AtualizarStatusSolicitacaoRequest
            {
                Status = 2
            };
        }
    }
}
