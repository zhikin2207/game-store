using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.DTOs.Components;
using GameStore.BLL.DTOs.EntitiesLocalization;
using GameStore.BLL.Filtering.Filters;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.BLL.Sorting.Interfaces;
using GameStore.WebUI.Mappings.Converters;
using GameStore.WebUI.ViewModels.Account;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Basic.EntitiesLocalization;
using GameStore.WebUI.ViewModels.Filter;
using GameStore.WebUI.ViewModels.Game;
using GameStore.WebUI.ViewModels.Genre;
using GameStore.WebUI.ViewModels.Order;
using GameStore.WebUI.ViewModels.Publisher;
using GameStore.WebUI.ViewModels.Sorting;

namespace GameStore.WebUI.Mappings
{
	public class MappingWebProfile : Profile
	{
		protected override void Configure()
		{
			// reverse mapping DTO to ViewModel
			Mapper.CreateMap<GameLocalizationDto, GameLocalizationViewModel>().ReverseMap();
			Mapper.CreateMap<GameDto, GameViewModel>().ForMember(dest => dest.DisplayName, m => m.MapFrom(src =>
				src.GameLocalizations.Any(pl => pl.Language == GetCurrentLanguage())
					? src.GameLocalizations.First(pl => pl.Language == GetCurrentLanguage()).Name
					: src.Name))
				.ForMember(dest => dest.DisplayDescription, m => m.MapFrom(src =>
					src.GameLocalizations.Any(pl => pl.Language == GetCurrentLanguage())
						? src.GameLocalizations.First(pl => pl.Language == GetCurrentLanguage()).Description
						: src.Description))
				.ReverseMap();
			Mapper.CreateMap<CreateEditGameViewModel, GameDto>().ConvertUsing<CreateGameViewModelConverter>();

			Mapper.CreateMap<CommentDto, CommentViewModel>().ReverseMap();

			Mapper.CreateMap<GenreLocalizationDto, GenreLocalizationViewModel>().ReverseMap();
			Mapper.CreateMap<GenreDto, GenreViewModel>()
				.ForMember(dest => dest.DisplayName, m => m.MapFrom(src =>
					src.GenreLocalizations.Any(lg => lg.Language == GetCurrentLanguage())
						? src.GenreLocalizations.First(lg => lg.Language == GetCurrentLanguage()).Name
						: src.Name))
				.ReverseMap();
			Mapper.CreateMap<CreateEditGenreViewModel, GenreDto>().ConvertUsing<GenreViewModelConverter>();

			Mapper.CreateMap<PublisherLocalizationDto, PublisherLocalizationViewModel>().ReverseMap();
			Mapper.CreateMap<PublisherDto, PublisherViewModel>()
				.ForMember(dest => dest.DisplayCompanyName, m => m.MapFrom(src =>
					src.PublisherLocalizations.Any(pl => pl.Language == GetCurrentLanguage())
						? src.PublisherLocalizations.First(pl => pl.Language == GetCurrentLanguage()).CompanyName
						: src.CompanyName))
				.ForMember(dest => dest.DisplayDescription, m => m.MapFrom(src =>
					src.PublisherLocalizations.Any(pl => pl.Language == GetCurrentLanguage())
						? src.PublisherLocalizations.First(pl => pl.Language == GetCurrentLanguage()).Description
						: src.Description))
				.ReverseMap();
			Mapper.CreateMap<CreateEditPublisherViewModel, PublisherDto>().ConvertUsing<PublisherViewModelConverter>();

			Mapper.CreateMap<PlatformLocalizationDto, PlatformLocalizationViewModel>().ReverseMap();
			Mapper.CreateMap<PlatformTypeDto, PlatformTypeViewModel>().ForMember(dest => dest.DisplayType, m => m.MapFrom(src =>
				src.PlatformLocalizations.Any(lg => lg.Language == GetCurrentLanguage())
					? src.PlatformLocalizations.First(lg => lg.Language == GetCurrentLanguage()).Type
					: src.Type))
				.ReverseMap();


			Mapper.CreateMap<OrderDetailsDto, OrderDetailsViewModel>().ReverseMap();
			Mapper.CreateMap<ShipperDto, ShipperViewModel>().ReverseMap();
			Mapper.CreateMap<OrderDto, OrderViewModel>().ReverseMap();
			Mapper.CreateMap<UserDto, UserViewModel>().ReverseMap();
			Mapper.CreateMap<RoleDto, RoleViewModel>().ReverseMap();

			// ViewModel to DTO
			Mapper.CreateMap<RegisterViewModel, UserDto>();

			// ViewModels to Sorting
			Mapper.CreateMap<GameSortingViewModel, ISortBase>().ConvertUsing<GameSortingViewModelConverter>();

			// reverse mapping Filters to FiltersViewModel
			Mapper.CreateMap<GamesByPriceFilterViewModel, GamesByPriceFilter>();
			Mapper.CreateMap<GamesByGenreFilterViewModel, GamesByGenreFilter>();
			Mapper.CreateMap<GamesByPlatformFilterViewModel, GamesByPlatformFilter>();
			Mapper.CreateMap<GamesByPublisherFilterViewModel, GamesByPublisherFilter>();
			Mapper.CreateMap<GamesByNameFilterViewModel, GamesByNameFilter>();
			Mapper.CreateMap<GamesByDateFilterViewModel, GamesByDateFilter>();
			Mapper.CreateMap<GamesByExistanceFilterViewModel, GamesByExistanceFilter>();

			// mapping ViewModel to Filters collection
			Mapper.CreateMap<AllGamesViewModel, IEnumerable<IFilterBase>>().ConvertUsing<AllGamesViewModelConverter>();
		}

		private LanguageDto GetCurrentLanguage()
		{
			string languageName = Thread.CurrentThread.CurrentUICulture.Name;

			LanguageDto language;
			if (!Enum.TryParse(languageName, true, out language))
			{
				language = LanguageDto.En;
			}

			return language;
		}
	}
}