using DesafioPagcerto.Model.ViewModel;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace DesafioPagcerto.Controllers.Examples
{
    public class TransacaoRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new TransacaoRequest()
            {
                DataTransacao = DateTime.Parse("2019-07-31"),
                NumeroCartao = "5252040005861703",
                NumeroParcelas = 2,
                ValorTransacao = 100.00M,
                ClienteId = 2
            };
        }
    }
}
