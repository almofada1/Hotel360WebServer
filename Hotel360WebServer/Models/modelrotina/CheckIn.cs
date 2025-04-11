using System;

namespace Hotel360WebServer.Models
{
    public class CheckIn
    {
        public string CodigoReserva { get; set; }
        public string LinhaReserva { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime HoraChegada { get; set; }
        public DateTime DataCheckIn { get; set; }
        public DateTime DataCheckOut { get; set; }
        public int NumeroNoites { get; set; }
        public DateTime HoraPartida { get; set; }
        public string CodigoAlojamento { get; set; }
        public string CategoriaAlojamento { get; set; }
        public string CategoriaPreco { get; set; }
        public int NumeroAdultos { get; set; }
        public int NumeroCriancas { get; set; }
        public int NumeroBercos { get; set; }
        public int NumeroAlojamentos { get; set; }
        public float Preco { get; set; }
        public string CodigoEstadoReserva { get; set; }
        public string Observacoes { get; set; }
        public string NomeHospede { get; set; }
        public string ApelidoHospede { get; set; }
        public string CodigoPais { get; set; }
        public string CodigoPackage { get; set; }
        public string DescricaoPackage { get; set; }
        public string NomeEntidade { get; set; }
        public string ApelidoEntidade { get; set; }
        public string CodigoHospede { get; internal set; }
        public string CodigoEntidade { get; internal set; }
        public string Id { get; internal set; }

    }
}
