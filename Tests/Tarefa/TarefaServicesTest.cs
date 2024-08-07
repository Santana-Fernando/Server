using Application.Http;
using Application.Tarefa.Interfaces;
using Application.Tarefa.Services;
using Application.Tarefa.ViewModel;
using Application.Validation;
using AutoMapper;
using Infra.Data.Context;
using Infra.Data.Repository.Tarefa;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Tests.Helper;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Tarefa
{
    public class TarefaServicesTest
    {
        private readonly TarefaServices _tarefaService;
        private readonly TarefaRepository _tarefaRepository;
        private readonly IMapper _mapperMock;

        public TarefaServicesTest()
        {
            _tarefaRepository = TarefaRepositoryStub();
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var config = appSettingsMock.configIMapper();
            _mapperMock = config.CreateMapper();

            _tarefaService = new TarefaServices(_tarefaRepository, _mapperMock);
        }

        private TarefaRepository TarefaRepositoryStub()
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();
            var dbContext = new ApplicationDbContext(options);
            return new TarefaRepository(dbContext);
        }

        #region ValidateFields
        [Fact(DisplayName = "Should validate Titulo Name is empty")]
        public void TarefaServices_ShouldValidateFieldTituloIsEmpty()
        {
            TarefaView tarefaView = new TarefaView
            {
                sNmTitulo = string.Empty,
                sDsSLA = "25,5",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            var validationFields = new ValidationFields(tarefaView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "O campo Título é obrigatório");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact(DisplayName = "Should validate field Titulo is Long")]
        public void TarefaServices_ShouldValidateFieldTituloIsLong()
        {
            TarefaView tarefaView = new TarefaView
            {
                sNmTitulo = "fasdfasdfasdfasdfasfhfjhsdfdjflkjilfjlaksdjflksdjlfkjasdlkfjlksdf ljfhklashf lkjkfhasdhfkjasdhkj af çikah fjk ahsjklfh asldjkfhlkj",
                sDsSLA = "25,5",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            var validationFields = new ValidationFields(tarefaView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field sNmTitulo must be a string or array type with a maximum length of '100'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact(DisplayName = "Should validate field sNmTitulo is short")]
        public void TarefaServices_ShouldValidateFieldTituloIsShort()
        {
            TarefaView tarefaView = new TarefaView
            {
                sNmTitulo = "2D",
                sDsSLA = "12",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            var validationFields = new ValidationFields(tarefaView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field sNmTitulo must be a string or array type with a minimum length of '3'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact(DisplayName = "Should validate field SLA is empty")]
        public void TarefaServices_ShouldValidateFieldSLAIsEmpty()
        {
            TarefaView tarefaView = new TarefaView
            {
                sNmTitulo = "2D",
                sDsSLA = string.Empty,
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            var validationFields = new ValidationFields(tarefaView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "O campo SLA é obrigatório");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact(DisplayName = "Should validate field SLA is Long")]
        public void TarefaServices_ShouldValidateFieldSLAIsLong()
        {
            TarefaView tarefaView = new TarefaView
            {
                sNmTitulo = "Fazer torrada",
                sDsSLA = "asdjfklasdfjalsfjasjklfjasljfçlasd",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            var validationFields = new ValidationFields(tarefaView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field sDsSLA must be a string or array type with a maximum length of '5'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact(DisplayName = "Should validate field sDsSLA is Short")]
        public void TarefaServices_ShouldValidateFieldSLAIsShort()
        {
            TarefaView tarefaView = new TarefaView
            {
                sNmTitulo = "Fazer torrada",
                sDsSLA = "1",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            var validationFields = new ValidationFields(tarefaView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field sDsSLA must be a string or array type with a minimum length of '2'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }        

        [Fact(DisplayName = "Should validate fields and return ok")]
        public void TarefaServices_ShouldValidateFielsdAndReturOk()
        {
            TarefaView tarefaView = new TarefaView
            {
                sNmTitulo = "Fazer torrada",
                sDsSLA = "25,5",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            var validationFields = new ValidationFields(tarefaView).IsValidWithErrors();

            Assert.True(validationFields.IsValid);
        }
        #endregion

        [Fact(DisplayName = "Should call the function add")]
        public void TarefaServices_ShouldCallTheFunctionAdd()
        {
            TarefaView tarefaView = new TarefaView();

            var result = _tarefaService.Add(tarefaView);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should return a internal server erro if process falied")]
        public void TarefaServices_ShouldReturnInternalServerErrorIfProcessFalied()
        {
            TarefaView tarefaView = new TarefaView
            {
                sNmTitulo = "Fazer torrada",
                sDsSLA = "25,5",
                sDsCaminhoAnexo = "caminho",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            HttpResponse httpResponse = new HttpResponse();
            var tarefaService = new Mock<ITarefaServices>();

            tarefaService.Setup(x => x.Add(tarefaView)).Returns(httpResponse.Response(HttpStatusCode.InternalServerError, null, string.Empty));

            var result = tarefaService.Object.Add(tarefaView);

            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Fact(DisplayName = "Should return ok if is all ok")]
        public void TarefaServices_ShouldReturnOkIfIdAllOk()
        {
            TarefaView tarefaView = new TarefaView
            {
                sNmTitulo = "Fazer torrada",
                sDsSLA = "25,5",
                sDsCaminhoAnexo = "caminho",
                nStSituacao = 1,
                tDtCadastro = DateTime.Now
            };

            HttpResponse httpResponse = new HttpResponse();
            var tarefaService = new Mock<ITarefaServices>();

            tarefaService.Setup(x => x.Add(tarefaView)).Returns(httpResponse.Response(HttpStatusCode.OK, null, "OK"));

            HttpResponseMessage result = tarefaService.Object.Add(tarefaView);
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        [Fact(DisplayName = "Should call GetById and return a user")]
        public async void TarefaServices_ShouldCallGetByIdAndReturnAUser()
        {
            var result = await _tarefaService.GetById(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Fazer café", result.sNmTitulo);
        }

        [Fact(DisplayName = "Should call GetList and return a users list")]
        public async void TarefaServices_ShouldCallGetListdAndReturnAUserList()
        {
            HttpResponse httpResponse = new HttpResponse();
            var tarefaService = new Mock<ITarefaServices>();

            IEnumerable<TarefaView> lstTarefa = new List<TarefaView>
            {
                new TarefaView()
                {
                    sNmTitulo = "Fazer torrada",
                    sDsSLA = "25,5",
                     sDsCaminhoAnexo = "caminho",
                    nStSituacao = 1,
                    tDtCadastro = DateTime.Now
                }
            };

            tarefaService.Setup(x => x.GetList()).ReturnsAsync(lstTarefa);

            var result = await tarefaService.Object.GetList();
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = "Should call Update and update user")]
        public void TarefaServices_ShouldCallUpdate()
        {
            HttpResponse httpResponse = new HttpResponse();
            var tarefaService = new Mock<ITarefaServices>();

            var tarefa = new TarefaView();
            tarefaService.Setup(x => x.Update(tarefa)).Returns(httpResponse.Response(HttpStatusCode.OK, null, "OK"));

            var statusReturn = tarefaService.Object.Update(tarefa);

            Assert.Equal(System.Net.HttpStatusCode.OK, statusReturn.StatusCode);
        }

        [Fact(DisplayName = "Should return badRequest id Update not found user")]
        public async void TarefaServices_ShouldUpdateReturnNotFound()
        {
            var tarefa = await _tarefaService.GetById(1);

            tarefa.sNmTitulo = "santana";
            tarefa.Id = 1000;

            var statusReturn = _tarefaService.Update(tarefa);

            Assert.NotNull(statusReturn);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, statusReturn.StatusCode);
            Assert.Equal("Tarefa não encontrada!", statusReturn.ReasonPhrase);
        }

        [Fact(DisplayName = "Should return badRequest id Update not found user")]
        public async void TarefaServices_ShouldUpdateReturnBadRequest()
        {
            var tarefa = await _tarefaService.GetById(1);

            tarefa.sNmTitulo = "";

            var statusReturn = _tarefaService.Update(tarefa);

            Assert.NotNull(statusReturn);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, statusReturn.StatusCode);
        }

        [Fact(DisplayName = "Should Call and Remove user")]
        public void TarefaServices_ShouldCallAndRemoveUser()
        {
            HttpResponse httpResponse = new HttpResponse();
            var tarefaService = new Mock<ITarefaServices>();

            tarefaService.Setup(x => x.Remove(0)).Returns(httpResponse.Response(HttpStatusCode.OK, null, "OK"));

            var result = tarefaService.Object.Remove(0);
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        [Fact(DisplayName = "Should return badRequest id Delete not found user")]
        public void TarefaServices_ShouldRemoveReturnBadRequest()
        {
            var statusReturn = _tarefaService.Remove(1000);

            Assert.NotNull(statusReturn);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, statusReturn.StatusCode);
            Assert.Equal("Tarefa não encontrada!", statusReturn.ReasonPhrase);
        }
    }
}
