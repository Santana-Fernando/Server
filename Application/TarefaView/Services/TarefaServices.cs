using Application.Http;
using Application.Tarefa.ViewModel;
using Application.Tarefa.Interfaces;
using Application.Validation;
using AutoMapper;
using Domain.Tarefa.Entidades;
using Domain.Tarefa.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Tarefa.Services
{
    public class TarefaServices: ITarefaServices
    {
        private ITarefa _tarefaRepository;
        private IMapper _mapper;
        public TarefaServices(ITarefa tarefaRepository, IMapper mapper)
        {
            _tarefaRepository = tarefaRepository;
            _mapper = mapper;
        }

        public HttpResponseMessage Add(TarefaView tarefa)
        {
            HttpResponse httpResponse = new HttpResponse();
            var validationFields = new ValidationFields(tarefa).IsValidWithErrors();
            List<string> errorMessages = new List<string>(validationFields.ErrorMessages.Select(result => result.ErrorMessage));

            try
            {
                if (!validationFields.IsValid)
                {
                    string messageError = string.Join(", ", errorMessages);
                    return httpResponse.Response(HttpStatusCode.BadRequest,
                        new StringContent(JsonSerializer.Serialize(errorMessages)),
                        messageError);
                }

                var tarefaMap = _mapper.Map<Tarefas>(tarefa);
                _tarefaRepository.Add(tarefaMap);

                return httpResponse.Response(HttpStatusCode.OK, null, "OK");
            }
            catch (Exception ex)
            {
                return httpResponse.Response(HttpStatusCode.InternalServerError, new StringContent(JsonSerializer.Serialize(ex.Message)), "Internal server error");
            }
        }

        public async Task<TarefaView> GetById(int id)
        {
            Tarefas tarefa = await _tarefaRepository.GetById(id);
            return _mapper.Map<TarefaView>(tarefa);
        }

        public async Task<IEnumerable<TarefaView>> GetList()
        {
            var tarefa = await _tarefaRepository.GetList();
            return _mapper.Map<IEnumerable<TarefaView>>(tarefa);
        }

        public async Task<IEnumerable<SituacaoTarefa>> GetListSituacao()
        {
            return await _tarefaRepository.GetListSituacao();
        }

        public HttpResponseMessage Update(TarefaView tarefa)
        {
            HttpResponse httpResponse = new HttpResponse();
            var validationFields = new ValidationFields(tarefa).IsValidWithErrors();
            List<string> errorMessages = new List<string>(validationFields.ErrorMessages.Select(result => result.ErrorMessage));

            if (!validationFields.IsValid)
            {
                string messageError = string.Join(", ", errorMessages);

                return httpResponse.Response(HttpStatusCode.BadRequest,
                    new StringContent(JsonSerializer.Serialize(errorMessages)),
                    messageError);
            }

            var tarefaParaAtualizar = _tarefaRepository.GetById(tarefa.Id).Result;
            
            if(tarefaParaAtualizar != null)
            {
                tarefaParaAtualizar.sNmTitulo = tarefa.sNmTitulo;
                tarefaParaAtualizar.sDsSLA = tarefa.sDsSLA;
                tarefaParaAtualizar.sDsCaminhoAnexo = tarefa.sDsCaminhoAnexo;
                tarefaParaAtualizar.nStSituacao = tarefa.nStSituacao;

                _tarefaRepository.Update(tarefaParaAtualizar);
                return httpResponse.Response(HttpStatusCode.OK, null, "OK");
            }
                
            return httpResponse.Response(HttpStatusCode.NotFound, null, "Tarefa não encontrada!");
        }

        public HttpResponseMessage Remove(int id)
        {
            HttpResponse httpResponse = new HttpResponse();
            
            var tarefaRemove = _tarefaRepository.GetById(id).Result;
            if (tarefaRemove != null)
            {
                _tarefaRepository.Remove(tarefaRemove);
                return httpResponse.Response(HttpStatusCode.OK, null, "OK");
            }

            return httpResponse.Response(HttpStatusCode.NotFound, null, "Tarefa não encontrada!");
        }
    }
}
