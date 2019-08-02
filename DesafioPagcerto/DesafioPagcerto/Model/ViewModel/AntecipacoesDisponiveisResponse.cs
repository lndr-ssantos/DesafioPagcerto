using DesafioPagcerto.Model.EntityModel;
using DesafioPagcerto.Model.ServiceModel;
using System.Collections.Generic;

namespace DesafioPagcerto.Model.ViewModel
{
    public class AntecipacoesDisponiveisResponse
    {
        public decimal ValorTotalTransacoes { get; set; }
        public decimal ValorTotalRepasse { get; set; }
        public List<Transacao> Transacoes { get; set; }

        public AntecipacoesDisponiveisResponse(List<Transacao> transacoes)
        {
            var valorSolicitacao = new CalcularSolicitacaoRepasseAntecipado(transacoes);
            ValorTotalTransacoes = valorSolicitacao.ValorTotalTransacoes;
            ValorTotalRepasse = valorSolicitacao.ValorTotalRepasse;
            Transacoes = transacoes;
        }
    }
}
