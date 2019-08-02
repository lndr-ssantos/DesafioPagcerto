using DesafioPagcerto.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPagcerto.Model.ViewModel
{
    public class AntecipacoesDisponiveisResponse
    {
        public decimal ValorTotalTransacoes { get; set; }
        public decimal ValorTotalRepasse { get; set; }
        public List<Transacao> Transacoes { get; set; }

        public AntecipacoesDisponiveisResponse(List<Transacao> transacoes)
        {
            CalcularValores(transacoes);
            Transacoes = transacoes;
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
