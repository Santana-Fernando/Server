using Application.Tarefa.ViewModel;
using Domain.Tarefa.Entidades;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Application.Tarefa.Interfaces
{
    public interface ITarefaServices
    {
        HttpResponseMessage Add(TarefaView tarefa);
        Task<IEnumerable<TarefaView>> GetList();
        Task<IEnumerable<SituacaoTarefa>> GetListSituacao();
        Task<TarefaView> GetById(int id);
        HttpResponseMessage Update(TarefaView tarefa);
        HttpResponseMessage Remove(int tarefa);
    }
}
