using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.BLL.Services.Interfaces;
using GameStore.BLL.Sorting.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Filters;
using GameStore.WebUI.ViewModels.Game;

namespace GameStore.WebUI.Controllers.ApiControllers
{
	[AuthorizeAPIUser(UserRole = UserRole.Manager)]
	public class GamesController : ApiController
	{
		public const string GenresLink = "Genres";

		private readonly IGameService _gameService;
		private readonly IOperationHistoryService _operationHistoryService;

		public GamesController(IGameService gameService, IOperationHistoryService operationHistoryService)
		{
			_gameService = gameService;
			_operationHistoryService = operationHistoryService;
		}

		[AllowAnonymous]
		[ActionName("Default")]
		public HttpResponseMessage Get([FromUri]AllGamesViewModel allGamesViewModel)
		{
			var filters = Mapper.Map<IEnumerable<IFilterBase>>(allGamesViewModel);
			var sorting = Mapper.Map<ISortBase>(allGamesViewModel.Sorting);

			IEnumerable<GameDto> games;
			if (allGamesViewModel.Paging.ItemsPerPage == 0)
			{
				games = _gameService.GetGames(filters, sorting);
			}
			else
			{
				games = _gameService.GetGames(
					filters,
					sorting,
					allGamesViewModel.Paging.CurrentPage,
					allGamesViewModel.Paging.ItemsPerPage);
			}

			return Request.CreateResponse(HttpStatusCode.OK, games);
		}

		[AllowAnonymous]
		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.NotFound)]
		public HttpResponseMessage Get(string key)
		{
			GameDto game = _gameService.GetGame(key);
			return Request.CreateResponse(HttpStatusCode.OK, game);
		}

		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.InternalServerError)]
		public HttpResponseMessage Post([FromBody]CreateEditGameViewModel createGameViewModel)
		{
			if (ModelState.IsValid)
			{
				if (!_gameService.IsExist(createGameViewModel.Game.Key))
				{
					var game = Mapper.Map<GameDto>(createGameViewModel);

					_gameService.Create(
						game,
						createGameViewModel.SelectedPublisherCompanyName,
						createGameViewModel.SelectedGenreNames,
						createGameViewModel.SelectedPlatformTypeNames);

					_operationHistoryService.WriteGameAddStatistics(createGameViewModel.Game.Key);

					return Request.CreateResponse(HttpStatusCode.OK);
				}
			}
			return Request.CreateResponse(HttpStatusCode.InternalServerError);
		}

		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.InternalServerError)]
		public HttpResponseMessage Put([FromBody]CreateEditGameViewModel createEditPublisherViewModel)
		{
			if (ModelState.IsValid)
			{
				var game = Mapper.Map<GameDto>(createEditPublisherViewModel);

				_gameService.Update(
					game,
					createEditPublisherViewModel.SelectedPublisherCompanyName,
					createEditPublisherViewModel.SelectedGenreNames,
					createEditPublisherViewModel.SelectedPlatformTypeNames);

				return Request.CreateResponse(HttpStatusCode.OK);
			}

			return Request.CreateResponse(HttpStatusCode.InternalServerError);
		}

		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.NotFound)]
		public HttpResponseMessage Delete(string key)
		{
			_gameService.Delete(key);
			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpGet]
		[AllowAnonymous]
		public HttpResponseMessage Genres(string key)
		{
			IEnumerable<GenreDto> genres = _gameService.GetGameGenres(key);
			return Request.CreateResponse(HttpStatusCode.OK, genres);
		} 
	}
}