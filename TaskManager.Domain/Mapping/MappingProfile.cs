using AutoMapper;
using TaskManager.Domain.Domain;
using TaskManager.Infra.Entity;

namespace TaskManager.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TarefaModel, Tarefa>().ReverseMap();

        }
    }
}
