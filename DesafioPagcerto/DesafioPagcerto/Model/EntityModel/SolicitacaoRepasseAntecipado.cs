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
        public string Status { get; set; }

        public IList<Transacao> Transacoes { get; set; }

        public SolicitacaoRepasseAntecipado() { }

        public SolicitacaoRepasseAntecipado(List<Transacao> transacoes)
        {
            Transacoes = transacoes;
        }
    }
}
