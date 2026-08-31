using AutoMapper;
using Opah.BancoCarrefour.Application.Handlers.Lancamentos.Create;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Update;

public class UpdateLancamentosProfile : Profile
{
	#region construtores

	public UpdateLancamentosProfile()
    {
        CreateMap<UpdateLancamentosCommand, LancamentosEntity>();
        CreateMap<LancamentosEntity, UpdateLancamentosResult>();
    }

    #endregion
}
