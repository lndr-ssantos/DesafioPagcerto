namespace DesafioPagcerto.Model.ViewModel
{
    public class TransacaoRequest
    {
        public decimal ValorTransacao { get; set; }
        public int NumeroParcelas { get; set; }
        public string NumeroCartao { get; set; }
        public int ClienteId { get; set; }
    }
}
