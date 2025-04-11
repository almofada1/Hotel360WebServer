using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel360WebServer.Models
{
    public class EstadoQuarto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string CodigoCentral { get; set; }
        public string CorEstado { get; set; }
        public int Ativo { get; set; }
    }
}
