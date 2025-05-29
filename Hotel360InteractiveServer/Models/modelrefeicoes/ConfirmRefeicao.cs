using System.ComponentModel.DataAnnotations;

namespace Hotel360InteractiveServer.Models
{
    public enum tipoRefeicao
    {
        PequenoAlmoco,
        Almoco,
        Jantar
    }

    public class ConfirmRefeicao
    {
        [Key]
        [MaxLength(50)]
        public string CodigoRefeicao { get; set; } = null!;

        [Required]
        public DateTime DataRefeicao { get; set; }

        [Required]
        public string TipoRefeicao { get; set; }

        [Required]
        public bool Confirmed { get; set; }
    }
}
