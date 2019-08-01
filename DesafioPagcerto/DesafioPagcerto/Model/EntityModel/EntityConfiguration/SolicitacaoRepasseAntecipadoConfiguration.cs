using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioPagcerto.Model.EntityModel.EntityConfiguration
{
    public class SolicitacaoRepasseAntecipadoConfiguration : IEntityTypeConfiguration<SolicitacaoRepasseAntecipado>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoRepasseAntecipado> builder)
        {
            builder.ToTable("SOLICITACOES");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("ID").ValueGeneratedOnAdd();
            builder.Property(t => t.DataSolicitacao).HasColumnName("DATA_SOLICATACAO").IsRequired();
            builder.Property(t => t.DataAnalise).HasColumnName("DATA_ANALISE").IsRequired();
            builder.Property(t => t.ValorTotalTransacoes).HasColumnName("VALOR_TOTAL_TRANSACOES").IsRequired();
            builder.Property(t => t.ValorTotalRepasse).HasColumnName("VALOR_TOTAL_REPASSE").IsRequired();
            builder.Property(t => t.Status).HasColumnName("STATUS").IsRequired();
        }
    }
}
