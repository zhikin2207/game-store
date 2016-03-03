using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.DTOs.EntitiesLocalization;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;
using GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization;
using GameStore.Domain.NorthwindDb;
using Order = GameStore.Domain.GameStoreDb.Entities.Order;

namespace GameStore.BLL.Mappings
{
	public class MappingBllProfile : Profile
	{
		protected override void Configure()
		{
			// Reverse mapping domain model to DTO
			Mapper.CreateMap<GameLocalization, GameLocalizationDto>().ReverseMap();
			Mapper.CreateMap<Game, GameDto>()
				.ForMember(dest => dest.IsReadOnly, m => m.MapFrom(src => src.Database != DatabaseName.GameStore))
				.ReverseMap();

			Mapper.CreateMap<Shipper, ShipperDto>()
				.ForMember(dest => dest.ShipperId, m => m.MapFrom(src => src.ShipperID))
				.ReverseMap();

			Mapper.CreateMap<User, UserDto>()
				.ForMember(dest => dest.IsAuthenticated, m => m.MapFrom(src => true))
				.ReverseMap();

			Mapper.CreateMap<GenreLocalization, GenreLocalizationDto>().ReverseMap();
			Mapper.CreateMap<Genre, GenreDto>().ForMember(dest => dest.IsReadOnly, m => m.MapFrom(src => src.Database != DatabaseName.GameStore))
				.ReverseMap();

			Mapper.CreateMap<PublisherLocalization, PublisherLocalizationDto>().ReverseMap();
			Mapper.CreateMap<Publisher, PublisherDto>().ReverseMap();

			Mapper.CreateMap<PlatformLocalization, PlatformLocalizationDto>().ReverseMap();
			Mapper.CreateMap<PlatformType, PlatformTypeDto>().ReverseMap();

			Mapper.CreateMap<Comment, CommentDto>().ReverseMap();
			Mapper.CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
			Mapper.CreateMap<Order, OrderDto>().ReverseMap();
			Mapper.CreateMap<Role, RoleDto>().ReverseMap();
		}
	}
}
