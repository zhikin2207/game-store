using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.Repositories.Northwind.Interfaces
{
	public interface IShipperRepository : IGenericRepository<Shipper, int>
	{
	}
}