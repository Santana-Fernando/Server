using System;

namespace Domain.Tarefa.Entidades
{
    public class Tarefas
    {
        public int Id { get; set; }
        public string sNmTitulo { get; set; }
        public string sDsSLA { get; set; }
        public DateTime tDtCadastro { get; set; }
        public int nStSituacao { get; set; }
    }
}
