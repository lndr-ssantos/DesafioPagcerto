using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioPagcerto.Model.EntityModel.EntityConfiguration
{
    public class TransacaoConfiguration : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.ToTable("TRANSACOES");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("ID").ValueGeneratedOnAdd();
            builder.Property(t => t.DataTransacao).HasColumnName("DATA_TRANSACAO").IsRequired();
            builder.Property(t => t.DataRepasse).HasColumnName("DATA_REPASSE");
            builder.Property(t => t.ConfirmacaoAdquirente).HasColumnName("CONFIRMACAO_ADQUIRENTE");
            builder.Property(t => t.ValorTransacao).HasColumnName("VALOR_TRANSACAO").IsRequired();
            builder.Property(t => t.ValorRepasse).HasColumnName("VALOR_REPASSE").IsRequired();
            builder.Property(t => t.NumeroParcelas).HasColumnName("NR_PARCELAS").IsRequired();
            builder.Property(t => t.DigitosCartao).HasColumnName("DIGITOS_CARTAO").IsRequired();
            builder.Property(t => t.ClientId).HasColumnName("CLIENTE_ID").IsRequired();
        }
    }
}
