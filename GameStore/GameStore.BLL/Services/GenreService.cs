using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization;

namespace GameStore.BLL.Services
{
	public class GenreService : IGenreService
	{
		private readonly IGameStoreUnitOfWork _unitOfWork;

		public GenreService(IGameStoreUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<GenreDto> GetGenres()
		{
			IEnumerable<Genre> genres = _unitOfWork.GenreRepository.GetList();
			return Mapper.Map<IEnumerable<GenreDto>>(genres);
		}

		public GenreDto GetGenre(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException(name);
			}

			Genre genre = _unitOfWork.GenreRepository.GetGenre(name);
			return Mapper.Map<GenreDto>(genre);
		}

		public bool IsExist(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException(name);
			}

			return _unitOfWork.GenreRepository.IsExist(name);
		}

		public void Create(GenreDto genreDto, string parentGenreName)
		{
			Genre parentGenre = null;

			if (!string.IsNullOrWhiteSpace(parentGenreName))
			{
				try
				{
					parentGenre = _unitOfWork.GenreRepository.GetGenre(parentGenreName);
				}
				catch (InvalidOperationException)
				{
					parentGenre = null;
				}
			}

			var genre = Mapper.Map<Genre>(genreDto);
			genre.Parent = parentGenre;

			_unitOfWork.GenreRepository.Add(genre);
			_unitOfWork.Save();
		}

		public void Update(GenreDto genreDto, string parentGenreName)
		{
			Genre parentGenre = null;

			if (!string.IsNullOrWhiteSpace(parentGenreName))
			{
				try
				{
					parentGenre = _unitOfWork.GenreRepository.GetGenre(parentGenreName);
				}
				catch (InvalidOperationException)
				{
					parentGenre = null;
				}
			}

			var genre = _unitOfWork.GenreRepository.Get(genreDto.GenreId);
			genre.Parent = parentGenre;
			genre.Name = genreDto.Name;

			if (genre.GenreLocalizations != null)
			{
				genre.GenreLocalizations.Clear();
			}
			
			genre.GenreLocalizations = Mapper.Map<ICollection<GenreLocalization>>(genreDto.GenreLocalizations);

			_unitOfWork.GenreRepository.Update(genre);
			_unitOfWork.Save();
		}

		public void Delete(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException(name);
			}

			Genre genre = _unitOfWork.GenreRepository.GetGenre(name);
			
			_unitOfWork.GenreRepository.Delete(genre);
			_unitOfWork.Save();
		}
	}
}