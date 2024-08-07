using Xunit.Abstractions;
using Xunit;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Moq;
using Tests.Helper;
using Infra.Data.Repository.Usuario;
using Domain.Tarefa.Entidades;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Tests.Tarefa
{
    public class TarefaRepositoryTest
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly TarefaRepository _tarefaRepository;
        public TarefaRepositoryTest(ITestOutputHelper output)
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();
            _dbContext = new ApplicationDbContext(options);
            _tarefaRepository = new TarefaRepository(_dbContext);

        }


        [Fact(DisplayName = "Should call the function Add")]
        public async Task TarefaRepository_ShouldCallFunctionAdd()
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var tarefa = new Tarefas
            {
                sNmTitulo = "Fazer café",
                sDsSLA = "25,5",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            try
            {
                _tarefaRepository.Add(tarefa);

                var tarefaInserida = await _dbContext.Tarefas.SingleOrDefaultAsync(u => u.Id == tarefa.Id);
                Assert.NotNull(tarefaInserida);
                Assert.True(tarefaInserida.Id > 0);

                new AppSettingsMock().RemoverAllTasks();
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact(DisplayName = "Should call the function GetById")]
        public async Task TarefaRepository_ShouldCallFunctionGetById()
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var usuario = await _tarefaRepository.GetById(1);

                Assert.NotNull(usuario);
                Assert.True(usuario.Id == 1);
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact(DisplayName = "Should call the function GetList")]
        public async Task TarefaRepository_ShouldCallFunctionGetList()
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            
            try
            {
                var usuario = await _tarefaRepository.GetList();

                Assert.NotNull(usuario);
                Assert.True(usuario.ToList().Count > 0);
}
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact(DisplayName = "Should call the function Update")]
        public async Task TarefaRepository_ShouldCallFunctionUpdate()
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var tarefaInserida = await _dbContext.Tarefas.SingleOrDefaultAsync(u => u.sNmTitulo == "Fazer café");

                Assert.NotNull(tarefaInserida);
                Assert.True(tarefaInserida.sNmTitulo == "Fazer café");

                tarefaInserida.sNmTitulo = "Fazer torrada";

                _tarefaRepository.Update(tarefaInserida);

                var tarefaModificado = await _dbContext.Tarefas.SingleOrDefaultAsync(u => u.sNmTitulo == "Fazer torrada");

                Assert.NotNull(tarefaModificado);
                Assert.True(tarefaModificado.sNmTitulo == "Fazer torrada");

                new AppSettingsMock().RemoverAllTasks();
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact(DisplayName = "Should call the function Remove")]
        public async Task TarefaRepository_ShouldCallFunctionRemove()
        {
            new AppSettingsMock().RemoverAllTasks();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var tarefa = new Tarefas
            {
                sNmTitulo = "Fazer torrada",
                sDsSLA = "25,5",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            try
            {
                _tarefaRepository.Add(tarefa);

                var tarefaInserida = await _dbContext.Tarefas.SingleOrDefaultAsync(u => u.sNmTitulo == tarefa.sNmTitulo);

                Assert.NotNull(tarefaInserida);
                Assert.True(tarefaInserida.sNmTitulo == tarefa.sNmTitulo);

                _tarefaRepository.Remove(tarefaInserida);

                var tarefaRemovida = await _dbContext.Tarefas.SingleOrDefaultAsync(u => u.Id == tarefaInserida.Id);
                Assert.Null(tarefaRemovida);
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
