using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.Repositories.Northwind.Interfaces
{
	public interface IProductRepository : IGenericRepository<Product, int>
	{
		Category GetProductCategory(string productKey);

		Supplier GetProductSupplier(string productKey);
	}
}