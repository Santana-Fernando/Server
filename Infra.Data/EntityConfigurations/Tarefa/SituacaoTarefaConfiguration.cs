using Domain.Tarefa.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.EntityConfigurations.Tarefa
{
    public class SituacaoTarefaConfiguration : IEntityTypeConfiguration<SituacaoTarefa>
    {
        public void Configure(EntityTypeBuilder<SituacaoTarefa> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.sNmSituacao).HasMaxLength(100).IsRequired();

            builder.HasData(
                new SituacaoTarefa
                {
                    Id = 1,
                   sNmSituacao = "A fazer"
                },
                new SituacaoTarefa
                {
                    Id = 2,
                    sNmSituacao = "Em andamento"
                },
                new SituacaoTarefa
                {
                    Id = 3,
                    sNmSituacao = "Resolvida"
                },
                new SituacaoTarefa
                {
                    Id = 4,
                    sNmSituacao = "Bloqueada"
                },
                new SituacaoTarefa
                {
                    Id = 5,
                    sNmSituacao = "Concluída"
                }
           );
        }
    }
}
