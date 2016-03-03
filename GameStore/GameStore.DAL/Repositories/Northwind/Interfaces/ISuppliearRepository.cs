using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.Repositories.Northwind.Interfaces
{
	public interface ISuppliearRepository : IGenericRepository<Supplier, int>
	{
		Supplier GetSupplier(string company);

		bool IsExist(string company);
	}
}