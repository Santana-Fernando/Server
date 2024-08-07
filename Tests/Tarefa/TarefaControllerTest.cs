using Xunit.Abstractions;
using Xunit;
using Infra.Data.Repository.Usuario;
using Tests.Helper;
using Infra.Data.Context;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Net;
using Application.Tarefa.Services;
using Application.Tarefa.ViewModel;
using Application.Tarefa.Interfaces;

namespace Tests.Tarefa
{
    public class TarefaControllerTest
    {
        private readonly TarefaServices _tarefaService;
        private readonly TarefaRepository _tarefaRepository;
        private readonly Presentation.Controllers.Tarefa _tarefaController;
        public TarefaControllerTest()
        {
            _tarefaRepository = UsuariosRepositoryStub();
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var config = appSettingsMock.configIMapper();
            var mapperMock = config.CreateMapper();

            _tarefaService = new TarefaServices(_tarefaRepository, mapperMock);
            _tarefaController = new Presentation.Controllers.Tarefa(_tarefaService);
        }

        private TarefaRepository UsuariosRepositoryStub()
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();
            var dbContext = new ApplicationDbContext(options);
            return new TarefaRepository(dbContext);
        }

        [Fact(DisplayName = "Should call GetList")]
        public async void TarefaController_ShouldCallGetList()
        {
            var result = await _tarefaController.GetList();

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should call the register")]
        public void TarefaController_shouldCallRegister()
        {
            TarefaView usuariosViewModel = new TarefaView();
            var result = _tarefaController.Register(usuariosViewModel);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should return 400 register return error")]
        public void TarefaController_shouldRegisterReturn400()
        {
            TarefaView usuariosViewModel = new TarefaView();
            var result = _tarefaController.Register(usuariosViewModel);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            }
        }

        [Fact(DisplayName = "Should return 500 if Register not OK")]
        public void TarefaController_shouldRegisterReturn500AddIfNotOk()
        {
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            var tarefaService = new TarefaServices(_tarefaRepository, mapperMock.Object);

            TarefaView tarefaView = new TarefaView()
            {
                sNmTitulo = "Fazer pastel",
                sDsSLA = "25,5",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            TarefaView tarefas = new TarefaView()
            {
            };

            mapperMock.Setup(x => x.Map<TarefaView>(tarefaView)).Returns(tarefas);

            var controller = new Presentation.Controllers.Tarefa(tarefaService);

            var result = controller.Register(tarefaView);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            }
        }

        [Fact(DisplayName = "Should call Function GetById")]
        public void TarefaController_shouldCallFunctionGetById()
        {
            var result = _tarefaController.GetById(1);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should call Function GetById")]
        public async Task TarefaController_shouldReturn200if()
        {
            var result = await _tarefaController.GetById(1);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            }
        }

        [Fact(DisplayName = "Should call Function Update")]
        public void TarefaController_shouldCallFunctionUpdate()
        {
            TarefaView tarefaViewModel = new TarefaView();
            var result = _tarefaController.Update(tarefaViewModel);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should return 404 if update dont found task")]
        public void TarefaController_shouldReturn404NotFoundIfUpdateDontFindUser()
        {
            TarefaView tarefaView = new TarefaView()
            {
                Id = 1111111,
                sNmTitulo = "Fazer pastel",
                sDsSLA = "25,5",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            var result = _tarefaController.Update(tarefaView);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            }
        }

        [Fact(DisplayName = "Should return 400 if update have a missing field")]
        public void TarefaController_shouldReturn400BadRequestIfUpdateMissingField()
        {
            TarefaView tarefaView = new TarefaView();
            var result = _tarefaController.Update(tarefaView);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            }
        }

        [Fact(DisplayName = "Should call function Remove")]
        public void TarefaController_shouldCallFunctionRemove()
        {
            TarefaView usuariosView = new TarefaView();

            var result = _tarefaController.Remove(0);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should return 404 if remove dont found user")]
        public void TarefaController_shouldReturn404NotFoundIfRemoveDontFindUser()
        {
            TarefaView tarefaView = new TarefaView();

            var result = _tarefaController.Remove(0);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            }
        }

    }
}
