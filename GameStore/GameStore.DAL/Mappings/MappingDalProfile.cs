using System;
using System.Collections.Generic;
using AutoMapper;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;
using GameStore.Domain.NorthwindDb;
using Order = GameStore.Domain.NorthwindDb.Order;

namespace GameStore.DAL.Mappings
{
	public class MappingDalProfile : Profile
	{
		protected override void Configure()
		{
			// Mapping Northwind entities to GameStore entities
			Mapper.CreateMap<Category, Genre>()
				.ForMember(dest => dest.Name, m => m.MapFrom(src => src.CategoryName))
				.ForMember(dest => dest.GenreId, m => m.MapFrom(src => src.CategoryID))
				.ForMember(dest => dest.Database, m => m.MapFrom(src => DatabaseName.Northwind));

			Mapper.CreateMap<Supplier, Publisher>()
				.ForMember(dest => dest.Database, m => m.MapFrom(src => DatabaseName.Northwind));

			Mapper.CreateMap<Product, Game>()
				.ForMember(dest => dest.Key, m => m.MapFrom(src => src.ProductID))
				.ForMember(dest => dest.Name, m => m.MapFrom(src => src.ProductName))
				.ForMember(dest => dest.Price, m => m.MapFrom(src => src.UnitPrice))
				.ForMember(dest => dest.UnitsInStock, m => m.MapFrom(src => src.UnitsInStock))
				.ForMember(dest => dest.Publisher, m => m.MapFrom(src => src.Supplier))
				.ForMember(dest => dest.Genres, m => m.MapFrom(src => new[] { src.Category }))
				.ForMember(dest => dest.GameHistory, opts => opts.MapFrom(src => new List<GameHistory>()))
				.ForMember(dest => dest.PlatformTypes, opts => opts.MapFrom(src => new List<PlatformType>()))
				.ForMember(dest => dest.Database, m => m.MapFrom(src => DatabaseName.Northwind));

			Mapper.CreateMap<Order, Domain.GameStoreDb.Entities.Order>()
				.ForMember(dest => dest.OrderId, opts => opts.MapFrom(src => src.OrderID))
				.ForMember(dest => dest.BasketGuid, opts => opts.MapFrom(src => Guid.Empty))
				.ForMember(dest => dest.OrderDetails, opts => opts.MapFrom(src => src.Order_Details));

			Mapper.CreateMap<Order_Detail, OrderDetails>()
				.ForMember(dest => dest.Price, opts => opts.MapFrom(src => src.UnitPrice));
		}
	}
}
