using AutoMapper;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Delete;

namespace Opah.BancoCarrefour.Api.Features.Lancamentos.Delete;

public class DeleteLancamentosProfile : Profile
{
    #region construtores

    public DeleteLancamentosProfile()
    {
        CreateMap<DeleteLancamentosRequest, DeleteLancamentosCommand>();
        CreateMap<DeleteLancamentosResult, DeleteLancamentosResponse>();
    }

    #endregion
}
