using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.Repositories.Northwind.Interfaces
{
	public interface ICategoryRepository : IGenericRepository<Category, int>
	{
		Category GetCategory(string name);

		bool IsExist(string name);
	}
}