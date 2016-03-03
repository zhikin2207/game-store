using System.Collections.Generic;
using GameStore.BLL.DTOs;

namespace GameStore.BLL.Services.Interfaces
{
	public interface IPublisherService
	{
		/// <summary>
		/// Get publisher by company name.
		/// </summary>
		/// <param name="company">Company name</param>
		/// <returns>Publisher data transfer object</returns>
		PublisherDto GetPublisher(string company);

		/// <summary>
		/// Create publisher.
		/// </summary>
		/// <param name="publisherDto">Publisher model</param>
		void Create(PublisherDto publisherDto);

		IEnumerable<PublisherDto> GetPublishers();

		void Delete(string company);

		void Update(PublisherDto publisherDto);

		bool IsExist(string company);
		IEnumerable<GameDto> GetPublisherGames(string company);
	}
}