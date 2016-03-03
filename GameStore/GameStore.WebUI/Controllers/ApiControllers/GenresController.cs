using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Filters;
using GameStore.WebUI.ViewModels.Genre;

namespace GameStore.WebUI.Controllers.ApiControllers
{
	[AuthorizeAPIUser(UserRole = UserRole.Manager)]
	public class GenresController : ApiController
	{
		private readonly IGenreService _genreService;

		public GenresController(IGenreService genreService)
		{
			_genreService = genreService;
		}

		[AllowAnonymous]
		[ActionName("Default")]
		public HttpResponseMessage Get()
		{
			IEnumerable<GenreDto> genres = _genreService.GetGenres();
			return Request.CreateResponse(HttpStatusCode.OK, genres);
		}

		[AllowAnonymous]
		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(ArgumentNullException), StatusCode = HttpStatusCode.InternalServerError)]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.NotFound)]
		public HttpResponseMessage Get(string key)
		{
			GenreDto genre = _genreService.GetGenre(key);
			return Request.CreateResponse(HttpStatusCode.OK, genre);
		}

		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.InternalServerError)]
		public HttpResponseMessage Post([FromBody] CreateEditGenreViewModel createEditGenreViewModel)
		{
			if (ModelState.IsValid)
			{
				if (!_genreService.IsExist(createEditGenreViewModel.CurrentGenre.Name))
				{
					var genre = Mapper.Map<GenreDto>(createEditGenreViewModel);
					_genreService.Create(genre, createEditGenreViewModel.SelectedParentGenre);

					return Request.CreateResponse(HttpStatusCode.OK);
				}
			}

			return Request.CreateResponse(HttpStatusCode.InternalServerError);
		}
		
		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.InternalServerError)]
		public HttpResponseMessage Put([FromBody] CreateEditGenreViewModel createEditGenreViewModel, [FromUri] string oldGenreGame)
		{
			if (ModelState.IsValid)
			{
				bool skipExistanceCheck = createEditGenreViewModel.CurrentGenre.Name == oldGenreGame;

				if (skipExistanceCheck || !_genreService.IsExist(createEditGenreViewModel.CurrentGenre.Name))
				{
					var genre = Mapper.Map<GenreDto>(createEditGenreViewModel);
					_genreService.Update(genre, createEditGenreViewModel.SelectedParentGenre);

					return Request.CreateResponse(HttpStatusCode.OK);
				}
			}

			return Request.CreateResponse(HttpStatusCode.InternalServerError);
		}

		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.NotFound)]
		public HttpResponseMessage Delete(string key)
		{
			_genreService.Delete(key);
			return Request.CreateResponse(HttpStatusCode.OK);
		}
	}
}