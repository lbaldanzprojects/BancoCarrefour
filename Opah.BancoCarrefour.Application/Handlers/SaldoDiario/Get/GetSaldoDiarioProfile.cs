using AutoMapper;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Application.Handlers.SaldoDiario.Get;

public class GetSaldoDiarioProfile : Profile
{
    #region construtores

    public GetSaldoDiarioProfile()
    {
        CreateMap<GetSaldoDiarioCommand, SaldoDiarioEntity>();
        CreateMap<SaldoDiarioEntity, GetSaldoDiarioResult>();
    }

    #endregion
}
