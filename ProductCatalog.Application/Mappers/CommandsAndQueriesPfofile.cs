using AutoMapper;
using ProductCatalog.Application.Commands;
using ProductCatalog.Domain.Core.Entities;

namespace ProductCatalog.Application.Mappers
{
    public class CommandsAndQueriesPfofile : Profile
    {
        public CommandsAndQueriesPfofile()
        {
            CreateMap<CreateProductWithImagesCommand, Product>()
                .ForMember(
                dest => dest.Title,
                src => src.MapFrom(s => s.Title))
                .ForMember(
                dest => dest.Description,
                src => src.MapFrom(s => s.Description));
        }
    }
}
