using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tarefa.ViewModel
{
    public class TarefaView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Título é obrigatório")]
        [MinLength(3)]
        [MaxLength(100)]
        [DisplayName("Título")]
        public string sNmTitulo { get; set; }

        [Required(ErrorMessage = "O campo SLA é obrigatório")]
        [MinLength(2)]
        [MaxLength(5)]
        [DisplayName("SLA")]
        public string sDsSLA { get; set; }
        public string? sDsCaminhoAnexo { get; set; }
        public DateTime tDtCadastro { get; set; }
        public int nStSituacao { get; set; }

    }
}
