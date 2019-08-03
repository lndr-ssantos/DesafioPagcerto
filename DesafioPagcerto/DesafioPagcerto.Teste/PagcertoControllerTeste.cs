using DesafioPagcerto.Model.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DesafioPagcerto.Teste
{
    public class PagcertoControllerTeste : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public PagcertoControllerTeste(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Nao_IniciarAtendimento_QuantoAtendimentoJaIniciado()
        {
            var client = _factory.CreateClient();

            var request = new
            {
                Url = "api/SolicitacoesAntecipacoes/solicitacoes-antecipacoes/4/atendimento/inicio",
                Body = new { }
            };

            var response = await client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [Fact]
        public async Task RealizarPostComSucesso()
        {
            var client = _factory.CreateClient();

            var request = new
            {
                Url = "api/transacoes/",
                Body = new TransacaoRequest
                {
                    ClienteId = 3,
                    NumeroCartao = "5252040005861703",
                    NumeroParcelas = 2,
                    ValorTransacao = 100
                }
            };

            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
        }
    }

    public static class ContentHelper
    {
        public static StringContent GetStringContent(object obj)
            => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
    }
}
