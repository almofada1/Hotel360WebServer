using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Hotel360WebServer.Models
{
    public class Quarto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }
        public int Extensao { get; set; }
        public int Lotacao { get; set; }
        public int Numcamaextra { get; set; }
        public int Numberco { get; set; }
        public string Piso { get; set; }
        public string Observarvacoes { get; set; }
        public string Estado { get; set; }
        public bool AcessivelDeficientes { get; set; }
        public bool Selecionado { get; set; }
        public Image imagem { get; set; }


    }
}
