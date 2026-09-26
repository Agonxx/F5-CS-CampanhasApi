using System.ComponentModel.DataAnnotations;

namespace CampanhasApi.Domain.Entities
{
    public class Campanha
    {
        [Key] public int Id { get; set; }
        [Required][MaxLength(150)] public string Titulo { get; set; }
        [Required][MaxLength(1000)] public string Descricao { get; set; }
        [Required] public DateTime DataInicio { get; set; }
        [Required] public DateTime DataFim { get; set; }
        [Required] public decimal MetaFinanceira { get; set; }
        [Required] public decimal ValorArrecadado { get; set; }
        [Required] public ECampanhaStatus Status { get; set; } = ECampanhaStatus.Ativa;
        [Required] public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
