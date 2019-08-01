using DesafioPagcerto.Model.ViewModel;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioPagcerto.Model.EntityModel
{
    public class Transacao
    {
        public int Id { get; set; }
        public DateTime DataTransacao { get; set; }
        public DateTime? DataRepasse { get; set; }
        public string ConfirmacaoAdquirente { get; set; }
        public decimal ValorTransacao { get; set; }
        public decimal ValorRepasse { get; set; }
        public int NumeroParcelas { get; set; }
        public string DigitosCartao { get; set; }
        public int ClientId { get; set; }
        [JsonIgnore]
        public SolicitacaoRepasseAntecipado SolicitacaoRepasse { get; set; }
        public int? SolicitacaoRepasseId { get; set; }

        private const decimal _taxaFixa = 0.9M;

        public Transacao() { }

        public Transacao(TransacaoRequest transacaoRequest)
        {
            DataTransacao = DateTime.Now;
            ValorTransacao = transacaoRequest.ValorTransacao;
            ValorRepasse = CalcularValorRepasse(transacaoRequest.ValorTransacao, transacaoRequest.NumeroParcelas);
            NumeroParcelas = transacaoRequest.NumeroParcelas;
            DigitosCartao = transacaoRequest.NumeroCartao.Substring(12, 4);
            ClientId = transacaoRequest.ClienteId;
        }

        private decimal CalcularValorRepasse(decimal valorTransacao, int numeroParcelas)
        {
            var valorParcela = valorTransacao / numeroParcelas;
            return (valorParcela - _taxaFixa) + valorParcela;
        }
    }
}
