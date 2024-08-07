using Domain.Tarefa.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Infra.Data.EntityConfigurations.Tarefa
{
    public class TarefaConfiguration : IEntityTypeConfiguration<Tarefas>
    {
        public void Configure(EntityTypeBuilder<Tarefas> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.sNmTitulo).HasMaxLength(20).IsRequired();
            builder.Property(p => p.sDsSLA).HasMaxLength(10).IsRequired();
            builder.Property(p => p.nStSituacao).IsRequired();
            builder.Property(p => p.tDtCadastro).IsRequired();

            builder.HasData(
               new Tarefas
               {
                   Id = 1,
                   sNmTitulo = "Fazer café",
                   sDsSLA = "25,5",
                   nStSituacao = 1,
                   tDtCadastro = DateTime.Now
               }
           );
        }
    }
}
