using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Genre;
using Resources;

namespace GameStore.WebUI.Controllers
{
	[AuthorizeUser(UserRole = UserRole.Manager)]
	public class GenreController : BaseController
	{
		private readonly IGenreService _genreService;
		private readonly IGenreViewModelBuilder _genreViewModelBuilder;

		public GenreController(
            IGenreService genreService, 
            IGenreViewModelBuilder genreViewModelBuilder,
			ILoggingService loggingService) : base(loggingService)
		{
			_genreService = genreService;
			_genreViewModelBuilder = genreViewModelBuilder;
		}

		[AllowAnonymous]
		public ActionResult Genres()
		{
			IEnumerable<GenreViewModel> genres = _genreViewModelBuilder.BuildGenresViewModel();
			return View(genres);
		}

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(string name)
        {
            GenreViewModel genreViewModel = _genreViewModelBuilder.BuildGenreViewModel(name);
            return View(genreViewModel);
        }

		[HttpGet]
		public ActionResult Create(string parentGenre)
		{
			CreateEditGenreViewModel createGenreViewModel = _genreViewModelBuilder.BuildCreateGenreViewModel(parentGenre);
			return View(createGenreViewModel);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Create(CreateEditGenreViewModel createGenreViewModel)
		{
			if (ModelState.IsValid)
			{
				if (!_genreService.IsExist(createGenreViewModel.CurrentGenre.Name))
				{
					var genre = Mapper.Map<GenreDto>(createGenreViewModel);
					_genreService.Create(genre, createGenreViewModel.SelectedParentGenre);

					return RedirectToAction("Genres");
				}

				ModelState.AddModelError("CurrentGenre.Name", GlobalResource.Game_GameExistsValidation);
			}

			_genreViewModelBuilder.RebiuldCreateGenreViewModel(createGenreViewModel);

			return View(createGenreViewModel);
		}

		[HttpGet]
		public ActionResult Update(string name)
		{
			CreateEditGenreViewModel updateGenreViewModel = _genreViewModelBuilder.BuildUpdateGenreViewModel(name);
			return View(updateGenreViewModel);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Update(CreateEditGenreViewModel updateGenreViewModel, string currentGenreName)
		{
			if (ModelState.IsValid)
			{
				bool skipExistanceCheck = updateGenreViewModel.CurrentGenre.Name == currentGenreName;

				if (skipExistanceCheck || !_genreService.IsExist(updateGenreViewModel.CurrentGenre.Name))
				{
					var genre = Mapper.Map<GenreDto>(updateGenreViewModel);
					_genreService.Update(genre, updateGenreViewModel.SelectedParentGenre);

					return RedirectToAction("Genres");
				}

				ModelState.AddModelError("CurrentGenre.Name", GlobalResource.Game_GameExistsValidation);
			}

			_genreViewModelBuilder.RebiuldUpdateGenreViewModel(updateGenreViewModel);

			return View(updateGenreViewModel);
		}

        [HttpPost]
        public ActionResult Delete(string name)
        {
            _genreService.Delete(name);

            return RedirectToAction("Genres");
        }
	}
}
