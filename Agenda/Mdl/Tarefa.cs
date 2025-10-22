using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Mdl
{
    public class Tarefa
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get;set; }
        public string Descricao { get; set; }
    }
}
