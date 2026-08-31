namespace Opah.BancoCarrefour.Messaging;

public static class RabbitMqTopology
{
    public const string EventosExchange = "opah.eventos";
    public const string DeadLetterExchange = "opah.eventos.dlx";

    public const string LancamentoRegistradoRoutingKey = "lancamento.registrado";

    public const string SaldoDiarioConsolidacaoQueue = "saldo-diario.consolidacao";
    public const string SaldoDiarioConsolidacaoDlq = "saldo-diario.consolidacao.dlq";
}
