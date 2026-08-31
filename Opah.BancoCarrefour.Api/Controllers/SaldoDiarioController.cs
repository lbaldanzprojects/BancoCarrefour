using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Opah.BancoCarrefour.Api.Common;
using Opah.BancoCarrefour.Api.Common.Validation;
using Opah.BancoCarrefour.Api.Features.SaldoDiario.Get;
using Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;
using Opah.BancoCarrefour.Application.Handlers.SaldoDiario.List;

namespace Opah.BancoCarrefour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaldoDiarioController : BaseController
    {
        #region atributos

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<SaldoDiarioController> _logger;

        #endregion

        #region construtores

        public SaldoDiarioController(IMediator mediator,
            IMapper mapper,
            ILogger<SaldoDiarioController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region métodos

        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaldoDiarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSaldoDiarioByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando um registro de saldo diário.");

            var request = new GetSaldoDiarioRequest { Id = id };
            var validator = new GetSaldoDiarioRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Falha na validação do processo de busca do saldo diário: {Errors}", validationResult.Errors);

                var validationError = validationResult.Errors
                    .Select(e => (ValidationError)e);

                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Errors = validationError
                });
            }
            else
            {
                var command = _mapper.Map<GetSaldoDiarioCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                if (response is null)
                {
                    _logger.LogWarning("Saldo diário não encontrado com Id: {Id}", id);

                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = $"Saldo diário não encontrado com Id: {id}"
                    });
                }
                else
                {
                    _logger.LogInformation("Saldo diário retornado com sucesso com Id: {Id}", response.Id);

                    return Ok(
                        _mapper.Map<GetSaldoDiarioResponse>(response),
                        $"Saldo diário {response.Id} retornado com sucesso!"
                    );
                }
            }
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(PaginatedResponse<GetSaldoDiarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListSaldoDiarioAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? dataInicio = null,
            [FromQuery] DateTime? dataFim = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Listando todos os saldos diários.");

            if (dataInicio.HasValue && dataFim.HasValue && dataInicio > dataFim)
                return BadRequest("Data de início não pode ser posterior à data de fim.");

            var command = new ListSaldoDiarioCommand
            {
                DataInicio = dataInicio,
                DataFim = dataFim
            };
            var response = await _mediator.Send(command, cancellationToken);
            var result = _mapper.Map<IEnumerable<GetSaldoDiarioResponse>>(response).AsQueryable();
            var paginatedList = await PaginatedList<GetSaldoDiarioResponse>.GetAsync(result, pageNumber, pageSize);

            _logger.LogInformation("Foram retornados {Count} saldos diários.", response.Count());

            return OkPaginated(paginatedList);
        }

        #endregion
    }
}
