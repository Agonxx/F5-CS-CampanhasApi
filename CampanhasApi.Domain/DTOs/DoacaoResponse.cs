namespace CampanhasApi.Domain.DTOs
{
    public class DoacaoResponse
    {
        public int Id { get; set; }
        public int IdCampanha { get; set; }
        public decimal ValorDoacao { get; set; }
        public DateTime DoadoEm { get; set; }
    }
}
