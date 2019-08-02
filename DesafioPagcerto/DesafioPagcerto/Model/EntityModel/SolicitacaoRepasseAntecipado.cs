using DesafioPagcerto.Model.ServiceModel;
using DesafioPagcerto.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPagcerto.Model.EntityModel
{
    public class SolicitacaoRepasseAntecipado
    {
        public int Id { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime DataAnalise { get; set; }
        public decimal ValorTotalTransacoes { get; set; }
        public decimal ValorTotalRepasse { get; set; }
        public int Status { get; set; }
        public int? Situacao { get; set; }

        public IList<Transacao> Transacoes { get; set; }

        public SolicitacaoRepasseAntecipado() { }

        public SolicitacaoRepasseAntecipado(List<Transacao> transacoes)
        {
            Transacoes = transacoes;
            DataSolicitacao = DateTime.Now;
            DataAnalise = DateTime.Now;
            var valorSolicitacao = new CalcularSolicitacaoRepasseAntecipado(transacoes);
            ValorTotalRepasse = valorSolicitacao.ValorTotalRepasse;
            ValorTotalTransacoes = valorSolicitacao.ValorTotalTransacoes;
            Status = (int)EStatus.AguardandoAnalise;
        }

        public enum EStatus
        {
            AguardandoAnalise = 1,
            EmAnalise = 2,
            Finalizada = 3
        }
    }
}
