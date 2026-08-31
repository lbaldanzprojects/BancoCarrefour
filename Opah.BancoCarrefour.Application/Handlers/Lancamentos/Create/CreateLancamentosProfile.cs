using AutoMapper;
using Opah.BancoCarrefour.Domain.Entidades;

namespace Opah.BancoCarrefour.Application.Handlers.Lancamentos.Create;

public class CreateLancamentosProfile : Profile
{
	#region construtores

	public CreateLancamentosProfile()
    {
        CreateMap<CreateLancamentosCommand, LancamentosEntity>();
        CreateMap<LancamentosEntity, CreateLancamentosResult>();
    }

    #endregion
}
