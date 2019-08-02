using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPagcerto.Model.ViewModel
{
    public class SolicitacaoAntecipacaoRequest
    {
        public int ClienteId { get; set; }
        public List<int> Transacoes { get; set; }
    }
}
