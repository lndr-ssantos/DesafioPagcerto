﻿using DesafioPagcerto.Model.ViewModel;
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
        private const decimal _taxaFixa = 0.9M;

        public Transacao() { }

        public Transacao(TransacaoRequest transacaoRequest)
        {
            DataTransacao = transacaoRequest.DataTransacao;
            ValorTransacao = transacaoRequest.ValorTransacao;
            ValorRepasse = CalcularValorRepasse(transacaoRequest.ValorTransacao, transacaoRequest.NumeroParcelas);
            NumeroParcelas = transacaoRequest.NumeroParcelas;
            DigitosCartao = transacaoRequest.NumeroCartao.Substring(12, 4);
        }

        private decimal CalcularValorRepasse(decimal valorTransacao, int numeroParcelas)
        {
            var valorParcela = valorTransacao / numeroParcelas;
            return valorParcela - _taxaFixa;
        }
    }
}
