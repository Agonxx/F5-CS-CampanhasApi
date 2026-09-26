using System.ComponentModel.DataAnnotations;

namespace CampanhasApi.Domain.DTOs
{
    public class DoacaoRequest
    {
        [Required] public int IdCampanha { get; set; }
        [Required] public decimal ValorDoacao { get; set; }
    }
}
