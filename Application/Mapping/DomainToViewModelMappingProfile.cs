using Application.Tarefa.ViewModel;
using AutoMapper;
using Domain.Tarefa.Entidades;

namespace Application.Mapping
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Tarefas, TarefaView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.sDsCaminhoAnexo, opt => opt.MapFrom(src => src.sDsCaminhoAnexo))
                .ForMember(dest => dest.sNmTitulo, opt => opt.MapFrom(src => src.sNmTitulo))
                .ForMember(dest => dest.sDsSLA, opt => opt.MapFrom(src => src.sDsSLA))
                .ForMember(dest => dest.tDtCadastro, opt => opt.MapFrom(src => src.tDtCadastro))
                .ForMember(dest => dest.nStSituacao, opt => opt.MapFrom(src => src.nStSituacao));
        }
    }
}
