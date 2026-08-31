# Cadastra lançamentos de teste na API.
# Uso: .\cadastrar-lancamentos.ps1
# Pré-requisito: docker compose up -d --build (API respondendo em localhost:8080)

$baseUrl = "http://localhost:8080/api/Lancamentos/create"

$payloads = @(
    @{ valor = 150.00; tipo = 1; descricao = "Venda balcão - cliente 001"; dataLancamento = "2026-08-30T10:00:00Z" },
    @{ valor = 80.00;  tipo = 2; descricao = "Pagamento fornecedor - insumos"; dataLancamento = "2026-08-30T11:00:00Z" },
    @{ valor = 200.00; tipo = 1; descricao = "Venda balcão - cliente 002"; dataLancamento = "2026-08-30T14:30:00Z" },
    @{ valor = 45.50;  tipo = 2; descricao = "Compra de material de limpeza"; dataLancamento = "2026-08-30T16:15:00Z" },
    @{ valor = 300.00; tipo = 1; descricao = "Venda balcão (dia anterior)"; dataLancamento = "2026-08-29T09:00:00Z" },
    @{ valor = 120.75; tipo = 2; descricao = "Pagamento de energia (dia anterior)"; dataLancamento = "2026-08-29T13:20:00Z" },
    @{ valor = 5000.00; tipo = 1; descricao = "Recebimento de contrato grande"; dataLancamento = "2026-08-30T09:45:00Z" },
    @{ valor = 19.99;  tipo = 1; descricao = "Venda avulsa"; dataLancamento = "2026-08-30T17:50:00Z" },
    @{ valor = 0;      tipo = 1; descricao = "Lançamento inválido - valor zero"; dataLancamento = "2026-08-30T18:00:00Z" },
    @{ valor = 50.00;  tipo = 2; descricao = ""; dataLancamento = "2026-08-30T18:10:00Z" },
    @{ valor = 12.30;  tipo = 1; descricao = "Venda avulsa - cartão"; dataLancamento = "2026-08-28T08:15:00Z" },
    @{ valor = 899.90; tipo = 1; descricao = "Venda de lote - atacado"; dataLancamento = "2026-08-28T10:40:00Z" },
    @{ valor = 275.00; tipo = 2; descricao = "Pagamento de aluguel"; dataLancamento = "2026-08-28T09:00:00Z" },
    @{ valor = 33.33;  tipo = 2; descricao = "Compra de embalagens"; dataLancamento = "2026-08-28T15:10:00Z" },
    @{ valor = 1250.00; tipo = 1; descricao = "Recebimento de cliente corporativo"; dataLancamento = "2026-08-27T11:00:00Z" },
    @{ valor = 410.60; tipo = 2; descricao = "Pagamento de fornecedor - matéria-prima"; dataLancamento = "2026-08-27T14:25:00Z" },
    @{ valor = 0.50;   tipo = 1; descricao = "Venda avulsa - valor mínimo"; dataLancamento = "2026-08-27T16:00:00Z" },
    @{ valor = 15000.00; tipo = 1; descricao = "Recebimento de contrato anual"; dataLancamento = "2026-08-26T09:30:00Z" },
    @{ valor = 620.45; tipo = 2; descricao = "Pagamento de folha - freelancer"; dataLancamento = "2026-08-26T13:00:00Z" },
    @{ valor = 58.20;  tipo = 1; descricao = "Venda balcão - cliente 003"; dataLancamento = "2026-08-31T08:20:00Z" },
    @{ valor = 142.10; tipo = 2; descricao = "Compra de suprimentos de escritório"; dataLancamento = "2026-08-31T09:10:00Z" },
    @{ valor = 3300.00; tipo = 1; descricao = "Venda de lote - cliente atacadista"; dataLancamento = "2026-08-31T10:50:00Z" },
    @{ valor = 76.80;  tipo = 2; descricao = "Pagamento de internet e telefonia"; dataLancamento = "2026-08-31T11:30:00Z" },
    @{ valor = 999.99; tipo = 1; descricao = "Recebimento via boleto"; dataLancamento = "2026-08-31T15:00:00Z" },
    @{ valor = 210.00; tipo = 2; descricao = "Pagamento de manutenção de equipamento"; dataLancamento = "2026-08-31T16:40:00Z" }
)

$contador = 0
foreach ($payload in $payloads) {
    $contador++
    $json = $payload | ConvertTo-Json

    Write-Host "`n[$contador/$($payloads.Count)] Enviando: $($payload.descricao)" -ForegroundColor Cyan

    try {
        $resposta = Invoke-RestMethod -Uri $baseUrl -Method Post -Body $json -ContentType "application/json"
        Write-Host "  -> 201 Created | Id: $($resposta.data.id)" -ForegroundColor Green
    }
    catch {
        $status = $_.Exception.Response.StatusCode.value__
        Write-Host "  -> Falhou (esperado para os casos invalidos) | Status: $status" -ForegroundColor Yellow
    }
}

Write-Host "`nConcluido. Confira as tabelas Lancamentos, OutboxMessages e (em alguns segundos) SaldoDiario." -ForegroundColor Cyan