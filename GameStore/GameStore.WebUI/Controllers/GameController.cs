using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net.Mime;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Filtering.Filters;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Filters;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Game;
using Resources;

namespace GameStore.WebUI.Controllers
{
	public class GameController : BaseController
	{
		protected const string GameFilePathTemplate = "~/App_Data/GameSetups/{0}.zip";
		protected const string GameFileNameTemplate = "{0}.zip";

		protected const string GameDoesNotExistTemplate = "Requested game does not exist: {0}";
		protected const string GameHasEmptyKey = "Requested game has empty key";

		private readonly IGameService _gameService;
		private readonly IOperationHistoryService _operationHistoryService;
		private readonly IGameViewModelBuilder _gameViewModelBuilder;

		public GameController(
			IGameService gameService,
			IOperationHistoryService operationHistoryService,
			IGameViewModelBuilder gameViewModelBuilder,
			ILoggingService loggingService) : base(loggingService)
		{
			_gameService = gameService;
			_operationHistoryService = operationHistoryService;
			_gameViewModelBuilder = gameViewModelBuilder;
		}

		[ActionName("AllGames")]
		public ActionResult Games(AllGamesViewModel viewModel)
		{
			if (CurrentUser.IsInRole(UserRole.User | UserRole.Guest))
			{
				viewModel.ExistanceFilter.HideDeletedGames = true;
			}

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			return View(viewModel);
		}

        [LogError(ExceptionType = typeof(ArgumentNullException), LogMessage = GameHasEmptyKey)]
        [LogError(ExceptionType = typeof(DbUpdateException), LogMessage = GameDoesNotExistTemplate, Parameter = "key")]
        public ActionResult Details(string key)
        {
            _operationHistoryService.WriteGameViewStatistics(key);

            GameDetailsViewModel gameDetailsViewModel = _gameViewModelBuilder.BuildGameDetailsViewModel(key);
            return View(gameDetailsViewModel);
        }

		[HttpGet]
		[AuthorizeUser(UserRole = UserRole.Manager)]
		[ActionName("New")]
		public ActionResult Create()
		{
			CreateEditGameViewModel createGameViewModel = _gameViewModelBuilder.BuildCreateGameViewModel();
			return View(createGameViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AuthorizeUser(UserRole = UserRole.Manager)]
        [ActionName("New")]
		public ActionResult Create(CreateEditGameViewModel createGameViewModel)
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

					return RedirectToAction("AllGames");
				}

				ModelState.AddModelError("Game.Key", GlobalResource.Game_GameExistsValidation);
			}

			_gameViewModelBuilder.RebuildCreateGameViewModel(createGameViewModel);

			return View(createGameViewModel);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
		[AuthorizeUser(UserRole = UserRole.Manager | UserRole.Publisher)]
		public ActionResult Update(CreateEditGameViewModel editGameViewModel)
		{
			if (ModelState.IsValid)
			{
				var game = Mapper.Map<GameDto>(editGameViewModel);

				_gameService.Update(
					game,
					editGameViewModel.SelectedPublisherCompanyName,
					editGameViewModel.SelectedGenreNames,
					editGameViewModel.SelectedPlatformTypeNames);

				return RedirectToAction("AllGames");
			}

			_gameViewModelBuilder.RebuildEditGameViewModel(editGameViewModel);

			return View(editGameViewModel);
		}

		[HttpGet]
        [AuthorizeUser(UserRole = UserRole.Manager | UserRole.Publisher)]
		[LogError(ExceptionType = typeof(ArgumentNullException), LogMessage = GameHasEmptyKey)]
		[LogError(ExceptionType = typeof(InvalidOperationException), LogMessage = GameDoesNotExistTemplate, Parameter = "key")]
		public ActionResult Update(string key)
		{
			CreateEditGameViewModel updateGameViewModel = _gameViewModelBuilder.BuildEditGameViewModel(key);

			if (!CurrentUser.IsInRole(UserRole.Manager) && !IsUserSpecificPublisher(updateGameViewModel.SelectedPublisherCompanyName))
			{
				return RedirectToAction("AllGames");
			}

			return View(updateGameViewModel);
		}

		[HttpPost]
		[AuthorizeUser(UserRole = UserRole.Manager)]
		[ActionName("Remove")]
		[LogError(ExceptionType = typeof(ArgumentNullException), LogMessage = GameHasEmptyKey)]
		[LogError(ExceptionType = typeof(InvalidOperationException), LogMessage = GameDoesNotExistTemplate, Parameter = "key")]
		public ActionResult Delete(string key)
		{
			_gameService.Delete(key);

			return RedirectToAction("AllGames");
		}

		[LogError(ExceptionType = typeof(InvalidOperationException), LogMessage = GameDoesNotExistTemplate, Parameter = "key", View = "Http404")]
		public ActionResult Download(string key)
		{
			string serverFilePath = Server.MapPath(string.Format(GameFilePathTemplate, key));
			if (!System.IO.File.Exists(serverFilePath))
			{
				throw new InvalidOperationException();
			}

			return File(
				serverFilePath,
				MediaTypeNames.Application.Octet,
				string.Format(GameFileNameTemplate, key));
		}

		[ChildActionOnly]
		[OutputCache(Duration = 300)]
		public ContentResult GamesNumber()
		{
			var filters = new List<IFilterBase>();

			if (CurrentUser.IsInRole(UserRole.User | UserRole.Guest))
			{
				filters.Add(new GamesByExistanceFilter { IsSet = true });
			}

			return Content(_gameService.Count(filters).ToString());
		}
	}
}