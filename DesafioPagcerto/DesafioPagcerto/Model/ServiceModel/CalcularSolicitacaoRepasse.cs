using DesafioPagcerto.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPagcerto.Model.ServiceModel
{
    public class CalcularSolicitacaoRepasseAntecipado
    {
        public decimal ValorTotalTransacoes { get; private set; }
        public decimal ValorTotalRepasse { get; private set; }

        public CalcularSolicitacaoRepasseAntecipado(List<Transacao> transacoes)
        {
            CalcularSolicitacao(transacoes);
        }

        private CalcularSolicitacaoRepasseAntecipado CalcularSolicitacao(List<Transacao> transacoes)
        {
            CalcularValores(transacoes);
            return this;
        }

        private void CalcularValores(List<Transacao> transacoes)
        {
            decimal taxaTotalAntecipacao = 0;
            foreach (var transacao in transacoes)
            {
                ValorTotalTransacoes += transacao.ValorRepasse;
                taxaTotalAntecipacao = CalcularValorRepasse(transacao);
            }

            ValorTotalRepasse = ValorTotalTransacoes - taxaTotalAntecipacao;
        }

        private decimal CalcularValorRepasse(Transacao transacao)
        {
            decimal taxaTotalAntecipacao = 0;
            var taxaAntecipacao = 0.038M;
            var valorParcela = transacao.ValorTransacao / transacao.NumeroParcelas;
            for (var i = 1; i <= transacao.NumeroParcelas; i++)
            {
                if (i == 1)
                {
                    taxaTotalAntecipacao += Math.Round((valorParcela - 0.9M) * taxaAntecipacao, 2);
                }
                else
                {
                    taxaTotalAntecipacao += Math.Round(valorParcela * (taxaAntecipacao * i), 2);
                }
            }

            return taxaTotalAntecipacao;
        }
    }
}
