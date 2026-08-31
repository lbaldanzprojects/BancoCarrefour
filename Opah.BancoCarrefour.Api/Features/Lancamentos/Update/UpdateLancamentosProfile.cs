using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Update;
using AutoMapper;

namespace Opah.BancoCarrefour.Api.Features.Lancamentos.Update;

public class UpdateLancamentosProfile : Profile
{
    #region construtores

    public UpdateLancamentosProfile()
    {
        CreateMap<UpdateLancamentosRequest, UpdateLancamentosCommand>();
        CreateMap<UpdateLancamentosResult, UpdateLancamentosResponse>();
    }

    #endregion
}
