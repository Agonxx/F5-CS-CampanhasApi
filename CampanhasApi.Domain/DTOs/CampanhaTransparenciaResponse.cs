namespace CampanhasApi.Domain.DTOs
{
    public class CampanhaTransparenciaResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public decimal MetaFinanceira { get; set; }
        public decimal ValorArrecadado { get; set; }
    }
}
