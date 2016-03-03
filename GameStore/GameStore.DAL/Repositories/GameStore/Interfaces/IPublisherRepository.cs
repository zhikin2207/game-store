using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore.Interfaces
{
	public interface IPublisherRepository : IGenericRepository<Publisher, int>
	{
		/// <summary>
		/// Get publisher by company name.
		/// </summary>
		/// <param name="company">Company name</param>
		/// <returns>Publisher</returns>
		Publisher GetPublisher(string company);

		bool IsExist(string company);
	}
}