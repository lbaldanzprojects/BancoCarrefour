using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Create;
using AutoMapper;

namespace Opah.BancoCarrefour.Api.Features.Lancamentos.Create;

public class CreateLancamentosProfile : Profile
{
    #region construtores

    public CreateLancamentosProfile()
    {
        CreateMap<CreateLancamentosRequest, CreateLancamentosCommand>();
        CreateMap<CreateLancamentosResult, CreateLancamentosResponse>();
    }

    #endregion
}
