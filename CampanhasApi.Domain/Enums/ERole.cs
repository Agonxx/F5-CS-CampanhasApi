using System.ComponentModel;

namespace CampanhasApi.Domain
{
    public enum ERole
    {
        [Description("Gestor da ONG")]
        GestorONG = 1,
        [Description("Doador")]
        Doador = 2,
    }
}
