using System.ComponentModel.DataAnnotations;

namespace CampanhasApi.Domain.Entities
{
    public class Doacao
    {
        [Key] public int Id { get; set; }
        [Required] public int IdCampanha { get; set; }
        [Required] public int IdDoador { get; set; }
        [Required] public decimal ValorDoacao { get; set; }
        [Required] public DateTime DoadoEm { get; set; } = DateTime.UtcNow;

        public Campanha Campanha { get; set; }
    }
}
