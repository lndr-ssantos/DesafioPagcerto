using DesafioPagcerto.Model.EntityModel.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace DesafioPagcerto.Model.EntityModel
{
    public class PagcertoContext : DbContext
    {
        public PagcertoContext(DbContextOptions<PagcertoContext> options) : base(options) { }

        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransacaoConfiguration());
        }
    }
}
