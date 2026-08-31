using AutoMapper;
using Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Api.Features.SaldoDiario.Get;

public class GetSaldoDiarioProfile : Profile
{
    #region constructors

    public GetSaldoDiarioProfile()
	{
        CreateMap<GetSaldoDiarioRequest, GetSaldoDiarioCommand>().ConvertUsing(p => new GetSaldoDiarioCommand(p.Id));
        CreateMap<GetSaldoDiarioResult, GetSaldoDiarioResponse>();
        CreateMap<SaldoDiarioEntity, GetSaldoDiarioResponse>();
    }

    #endregion
}
