using AutoMapper;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Api.Features.SaldoDiario.Get;

public class GetLancamentosProfile : Profile
{
    #region constructors

    public GetLancamentosProfile()
	{
        CreateMap<GetLancamentosRequest, GetLancamentosCommand>().ConvertUsing(p => new GetLancamentosCommand(p.Id));
        CreateMap<GetLancamentosResult, GetLancamentosResponse>();
        CreateMap<LancamentosEntity, GetLancamentosResponse>();
    }

    #endregion
}
