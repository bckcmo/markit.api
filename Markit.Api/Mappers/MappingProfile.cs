using System.Collections.Generic;
using AutoMapper;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Mappers
{
    public class MappingProfile : Profile 
    {
        public MappingProfile() {
            CreateMap<UserEntity, User>();
            CreateMap<ShoppingListEntity, ShoppingList>();
            CreateMap<StoreEntity, Store>()
                .ForMember(dest => dest.Coordinate,
                    opt => opt.MapFrom(src =>
                        new Coordinate
                        {
                            Latitude = src.Latitude,
                            Longitude = src.Longitude
                        }));
            
            CreateMap<ListTagEntity, ListTag>()
                .ForMember(dest => dest.Tag,
                    opt => opt.MapFrom(src =>
                        new Tag
                        {
                            Id = src.TagId
                        }));
        }
    }
}