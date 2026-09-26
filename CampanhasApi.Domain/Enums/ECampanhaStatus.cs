using System.ComponentModel;

namespace CampanhasApi.Domain
{
    public enum ECampanhaStatus
    {
        [Description("Ativa")]
        Ativa = 1,
        [Description("Concluída")]
        Concluida = 2,
        [Description("Cancelada")]
        Cancelada = 3,
    }
}
