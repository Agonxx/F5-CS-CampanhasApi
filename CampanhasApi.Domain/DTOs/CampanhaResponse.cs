namespace CampanhasApi.Domain.DTOs
{
    public class CampanhaResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal MetaFinanceira { get; set; }
        public decimal ValorArrecadado { get; set; }
        public ECampanhaStatus Status { get; set; }
    }
}
