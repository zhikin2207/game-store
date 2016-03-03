using System.Collections.Generic;
using GameStore.BLL.DTOs;

namespace GameStore.BLL.Services.Interfaces
{
	public interface IGenreService
	{
		IEnumerable<GenreDto> GetGenres();

		void Create(GenreDto genreDto, string parentGenreName);

		GenreDto GetGenre(string name);

		bool IsExist(string name);

		void Update(GenreDto genreDto, string parentGenreName);

		void Delete(string name);
	}
}