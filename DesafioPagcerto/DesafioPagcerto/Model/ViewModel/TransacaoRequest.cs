using System;

namespace DesafioPagcerto.Model.ViewModel
{
    public class TransacaoRequest
    {
        public DateTime DataTransacao { get; set; }
        public decimal ValorTransacao { get; set; }
        public int NumeroParcelas { get; set; }
        public string NumeroCartao { get; set; }
    }
}
