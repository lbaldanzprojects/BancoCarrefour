using AutoMapper;
using Opah.BancoCarrefour.Api.Common;
using Opah.BancoCarrefour.Api.Common.Validation;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Create;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Delete;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.List;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Opah.BancoCarrefour.Api.Features.Lancamentos.Create;
using Opah.BancoCarrefour.Api.Features.Lancamentos.Delete;
using Opah.BancoCarrefour.Api.Features.Lancamentos.Get;
using Opah.BancoCarrefour.Api.Features.Lancamentos.Update;

namespace Opah.BancoCarrefour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LancamentosController : BaseController
    {
        #region atributos

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<LancamentosController> _logger;

        #endregion

        #region construtores

        public LancamentosController(IMediator mediator,
            IMapper mapper,
            ILogger<LancamentosController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region métodos

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateLancamentosResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLancamentosAsync([FromBody] CreateLancamentosRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Criação de um registro de lançamentos de crédito / débito.");

            var validator = new CreateLancamentosRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Falha na validação do processo de criação do lançamento: {Errors}", validationResult.Errors);

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
                var command = _mapper.Map<CreateLancamentosCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Lançamento criado com sucesso com Id: {Id}", response.Id);

                return Created(string.Empty, new ApiResponseWithData<CreateLancamentosResponse>
                {
                    Success = true,
                    Message = $"Registro de lançamento {response.Id} criado com sucesso!",
                    Data = _mapper.Map<CreateLancamentosResponse>(response)
                });
            }
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateLancamentosResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLancamentosAsync([FromBody] UpdateLancamentosRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Atualizando um registro de lançamento.");

            var validator = new UpdateLancamentosRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Falha na validação do processo de atualização do lançamento: {Errors}", validationResult.Errors);

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
                var command = _mapper.Map<UpdateLancamentosCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Lançamento atualizado com sucesso com Id: {Id}", response.Id);

                return Created(string.Empty, new ApiResponseWithData<UpdateLancamentosResponse>
                {
                    Success = true,
                    Message = $"Registro de lançamento {response.Id} atualizado com sucesso!",
                    Data = _mapper.Map<UpdateLancamentosResponse>(response)
                });
            }
        }

        [HttpDelete("delete")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLancamentosAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Excluindo um registro de lançamento.");

            var request = new DeleteLancamentosRequest { Id = id };
            var validator = new DeleteLancamentosRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Falha na validação do processo de exclusão do lançamento: {Errors}", validationResult.Errors);

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
                var command = _mapper.Map<DeleteLancamentosCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Lançamento excluído com sucesso com Id: {Id}", response.Id);

                return Created(string.Empty, new ApiResponseWithData<DeleteLancamentosResponse>
                {
                    Success = true,
                    Message = $"Registro de lançamento {response.Id} excluído com sucesso!",
                    Data = _mapper.Map<DeleteLancamentosResponse>(response)
                });
            }
        }

        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetLancamentosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountBydIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando um registro de lançamento.");

            var request = new GetLancamentosRequest { Id = id };
            var validator = new GetLancamentosRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Falha na validação do processo de busca do lançamento: {Errors}", validationResult.Errors);

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
                var command = _mapper.Map<GetLancamentosCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                if (response is null)
                {
                    _logger.LogWarning("Registro de lançamento não encontrado com Id: {Id}", id);

                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = $"Registro de lançamento não encontrado com Id: {id}"
                    });
                }
                else
                {
                    _logger.LogInformation("Registro de lançamento retornado com sucesso com Id: {Id}", response.Id);

                    return Ok(
                        _mapper.Map<GetLancamentosResponse>(response),
                        $"Registro de lançamento {response.Id} retornado com sucesso!"
                    );
                }
            }
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(PaginatedResponse<GetLancamentosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListLancamentosAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Listando todos os lançamentos.");

            var command = new ListLancamentosCommand();
            var response = await _mediator.Send(command, cancellationToken);
            var result = _mapper.Map<IEnumerable<GetLancamentosResponse>>(response).AsQueryable();
            var paginatedList = await PaginatedList<GetLancamentosResponse>.GetAsync(result, pageNumber, pageSize);

            _logger.LogInformation("Foram retornados {Count} lançamentos.", response.Count());

            return OkPaginated(paginatedList);
        }

        #endregion
    }
}
