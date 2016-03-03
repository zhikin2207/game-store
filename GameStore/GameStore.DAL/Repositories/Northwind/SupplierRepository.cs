using GameStore.DAL.Repositories.Northwind.Interfaces;
using GameStore.Domain;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.Repositories.Northwind
{
	public class SupplierRepository : GenericRepository<Supplier, int>, ISuppliearRepository
	{
		public SupplierRepository(BaseDataContext context)
			: base(context)
		{
		}

		public Supplier GetSupplier(string company)
		{
			return Get(s => s.CompanyName == company);
		}

		public virtual bool IsExist(string company)
		{
			return IsExist(p => p.CompanyName == company);
		}
	}
}