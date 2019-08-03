using DesafioPagcerto.Model.EntityModel;
using System.Collections.Generic;

namespace DesafioPagcerto.Model.ViewModel
{
    public class DetalhesSolicitacaoResponse
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int Situacao { get; set; }
        public List<Transacao> Transacoes { get; set; }
    }
}
