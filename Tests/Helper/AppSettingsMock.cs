using Application.Tarefa.ViewModel;
using AutoMapper;
using Domain.Tarefa.Entidades;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Linq;

namespace Tests.Helper
{
    public class AppSettingsMock
    {
        public Mock<IConfiguration> configurationMockStub()
        {
            const string jwtKey = "ChaveSuperSecreta123";
            const string jwtIssuer = "FERNANDO";
            const string jwtAudience = "AplicacaoWebAPI";
            const int jwtExpirationInMinutes = 30;

            var configurationMock = new Mock<IConfiguration>();

            configurationMock.Setup(x => x["Jwt:Key"]).Returns(jwtKey);
            configurationMock.Setup(x => x["Jwt:Issuer"]).Returns(jwtIssuer);
            configurationMock.Setup(x => x["Jwt:Audience"]).Returns(jwtAudience);
            configurationMock.Setup(x => x["Jwt:ExpirationInMinutes"]).Returns(jwtExpirationInMinutes.ToString());

            return configurationMock;
        }

        public DbContextOptions<ApplicationDbContext> OptionsDatabaseStub()
        {
            const string defaultConnectionString = "Data Source=localhost;User ID=sa;Password=Fern@nd01331;Database=TaskRegister;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(defaultConnectionString)
                .Options;

            return options;
        }

        public MapperConfiguration configIMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Tarefas, TarefaView>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.sNmTitulo, opt => opt.MapFrom(src => src.sNmTitulo))
                    .ForMember(dest => dest.sDsSLA, opt => opt.MapFrom(src => src.sDsSLA))
                    .ForMember(dest => dest.tDtCadastro, opt => opt.MapFrom(src => src.tDtCadastro))
                    .ForMember(dest => dest.nStSituacao, opt => opt.MapFrom(src => src.nStSituacao));

                cfg.CreateMap<TarefaView, Tarefas>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.sNmTitulo, opt => opt.MapFrom(src => src.sNmTitulo))
                    .ForMember(dest => dest.sDsSLA, opt => opt.MapFrom(src => src.sDsSLA))
                    .ForMember(dest => dest.tDtCadastro, opt => opt.MapFrom(src => src.tDtCadastro))
                    .ForMember(dest => dest.nStSituacao, opt => opt.MapFrom(src => src.nStSituacao));
            });

            return config;
        }

        public void RemoverAllTasks()
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var tarefasParaRemover = dbContext.Tarefas.Where(t => t.Id != 1).ToList();

                var Tarefa = new Tarefas
                {
                    Id = 1,
                    sNmTitulo = "Fazer café",
                    sDsSLA = "25,5",
                    nStSituacao = 1,
                    tDtCadastro = DateTime.Now
                };

                if (tarefasParaRemover.Count > 0)
                {
                    dbContext.Tarefas.RemoveRange(tarefasParaRemover);
                    dbContext.Tarefas.Update(Tarefa);
                    dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Tarefas', RESEED, 1)");
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
