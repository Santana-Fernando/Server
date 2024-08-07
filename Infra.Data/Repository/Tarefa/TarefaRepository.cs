using Domain.Tarefa.Entidades;
using Domain.Tarefa.Interfaces;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infra.Data.Repository.Usuario
{
    public class TarefaRepository : ITarefa
    {
        private readonly ApplicationDbContext _context;
        public TarefaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Tarefas tarefa)
        {
            _context.Add(tarefa);
            _context.SaveChanges();
        }

        public async Task<Tarefas> GetById(int id)
        {
            return await _context.Tarefas.FindAsync(id);
        }

        public async Task<IEnumerable<Tarefas>> GetList()
        {
            return await _context.Tarefas.ToListAsync();
        }

        public void Update(Tarefas tarefa)
        {
            _context.Update(tarefa);
            _context.SaveChanges();
        }

        public void Remove(Tarefas tarefa)
        {
            _context.Remove(tarefa);
            _context.SaveChanges();
        }
    }
}
