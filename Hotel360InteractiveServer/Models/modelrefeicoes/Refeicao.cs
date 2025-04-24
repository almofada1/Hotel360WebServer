namespace Hotel360InteractiveServer.Models
{
    public class Refeicao
    {
        public string CodigoReserva { get; set; }
        public string LinhaReserva { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataCheckIn { get; set; }
        public DateTime HoraDeChegada { get; set; }
        public DateTime DataCheckOut { get; set; }
        public int NumeroNoites { get; set; }
        public DateTime HoraPartida { get; set; }
        public DateTime DataInicioAlojamento { get; set; }
        public DateTime DataFimAlojamento { get; set; }
        public string CodigoAlojamento { get; set; }
        public string CategoriaAlojamento { get; set; }
        public string CategoriaPreco { get; set; }
        public int NumeroAdultos { get; set; }
        public int NumeroCriancas { get; set; }
        public int NumeroBercos { get; set; }
        public int NumeroAlojamentos { get; set; }
        public double Preco { get; set; }
        public string CodigoEstadoReserva { get; set; }
        public string Observacoes { get; set; }
        public string Voucher { get; set; }
        public string CodigoHospede { get; set; }
        public string NomeHospede { get; set; }
        public string ApelidoHospede { get; set; }
        public string CodigoPais { get; set; }
        public string Pais { get; set; }
        public string CodigoPackage { get; set; }
        public string DescricaoPackage { get; set; }
        public DateTime DataRefeicao { get; set; }
        public bool PequenoAlmoco { get; set; }
        public bool Almoco { get; set; }
        public bool Jantar { get; set; }
        public string Observations { get; set; }
        public string CodigoEntidade { get; set; }
        public string NomeEntidade { get; set; }
        public string ApelidoEntidade { get; set; }
        public int TipoEntidade { get; set; }
        public string TipoDocFP { get; set; }
        public string SerieFP { get; set; }
        public int NumDocFP { get; set; }
        public double ValorPendente { get; set; }
    }
}
