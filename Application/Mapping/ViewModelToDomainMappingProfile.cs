using Application.Tarefa.ViewModel;
using AutoMapper;
using Domain.Tarefa.Entidades;

namespace Application.Mapping
{
    public class ViewModelToDomainMappingProfile: Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<TarefaView, Tarefas>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.sNmTitulo, opt => opt.MapFrom(src => src.sNmTitulo))
                .ForMember(dest => dest.sDsSLA, opt => opt.MapFrom(src => src.sDsSLA))
                .ForMember(dest => dest.tDtCadastro, opt => opt.MapFrom(src => src.tDtCadastro))
                .ForMember(dest => dest.nStSituacao, opt => opt.MapFrom(src => src.nStSituacao));
        }
    }
}
