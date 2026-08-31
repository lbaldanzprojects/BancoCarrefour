using AutoMapper;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Delete;

public class DeleteLancamentosProfile : Profile
{
    #region construtores

    public DeleteLancamentosProfile()
    {
        CreateMap<LancamentosEntity, DeleteLancamentosResult>();
    }

    #endregion
}
