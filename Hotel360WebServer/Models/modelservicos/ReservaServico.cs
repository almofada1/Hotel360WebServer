using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hotel360WebServer.Models
{
    class ReservaServico
    {
        public int nBercos { get; set; }
        public int nCamasCasal { get; set; }
        public int nCamasInd { get; set; }
        public string CodigoReserva { get; set; }
        public string LinhaReserva { get; set; }
        public string EstadoReserva { get; set; }
        public int NumeroAdultos { get; set; }
        public int NumeroCriancas { get; set; }
        public int NumeroBebes { get; set; }

        
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime HoraChegada { get; set; }
        public DateTime HoraPartida { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string CodigoAlojamento { get; set; }
        
        public string Observacoes { get; set; }
        public string NomeHospede { get; set; }
        public string ApelidoHospede { get; set; }
        public string listaServicos { get; set; }
    }
}
