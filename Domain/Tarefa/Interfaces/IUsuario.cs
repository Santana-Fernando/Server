using Domain.Tarefa.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Tarefa.Interfaces
{
    public interface ITarefa
    {
        void Add(Tarefas tarefa);
        Task<IEnumerable<Tarefas>> GetList();
        Task<IEnumerable<SituacaoTarefa>> GetListSituacao();
        Task<Tarefas> GetById(int nCdTarefa);
        void Update(Tarefas tarefa);
        void Remove(Tarefas tarefa);
    }
}
