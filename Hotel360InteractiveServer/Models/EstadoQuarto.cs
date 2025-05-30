namespace Hotel360InteractiveServer.Models
{
    public class EstadoQuarto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string CodigoCentral { get; set; }
        public string CorEstado { get; set; }
        public int Ativo { get; set; }
        public bool DisponivelPortal { get; set; }
    }
}
