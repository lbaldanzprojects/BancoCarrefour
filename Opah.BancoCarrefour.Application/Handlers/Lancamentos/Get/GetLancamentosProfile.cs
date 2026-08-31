using AutoMapper;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Get;

public class GetLancamentosProfile : Profile
{
	#region construtores

	public GetLancamentosProfile()
    {
        CreateMap<GetLancamentosCommand, LancamentosEntity>();
        CreateMap<LancamentosEntity, GetLancamentosResult>();
    }

    #endregion
}
