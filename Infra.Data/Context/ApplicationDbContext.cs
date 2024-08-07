using Domain.Tarefa.Entidades;
using Infra.Data.EntityConfigurations.Tarefa;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Tarefas> Tarefas { get; set; }
        public DbSet<SituacaoTarefa> SituacaoTarefa { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new SituacaoTarefaConfiguration());
            builder.ApplyConfiguration(new TarefaConfiguration());
        }
    }
}
