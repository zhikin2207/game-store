using GameStore.DAL.Repositories.Northwind.Interfaces;
using GameStore.Domain;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.Repositories.Northwind
{
	public class ShipperRepository : GenericRepository<Shipper, int>, IShipperRepository
	{
		public ShipperRepository(BaseDataContext context)
			: base(context)
		{
		}
	}
}