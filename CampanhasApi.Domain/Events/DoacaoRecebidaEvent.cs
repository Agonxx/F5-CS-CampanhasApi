namespace Shared.Contracts.Events;

public record DoacaoRecebidaEvent(
    int DoacaoId,
    int CampanhaId,
    int DoadorId,
    decimal Valor,
    DateTime DoadoEm
);
