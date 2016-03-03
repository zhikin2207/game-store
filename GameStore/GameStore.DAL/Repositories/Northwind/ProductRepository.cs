using GameStore.DAL.Repositories.Northwind.Interfaces;
using GameStore.Domain;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.Repositories.Northwind
{
	public class ProductRepository : GenericRepository<Product, int>, IProductRepository
	{
		public ProductRepository(BaseDataContext context)
			: base(context)
		{
		}

		public Product GetProduct(string productKey)
		{
			return Get(product => productKey == product.ProductID.ToString());
		}

		public virtual Category GetProductCategory(string productKey)
		{
			return GetProduct(productKey).Category;
		}

		public virtual Supplier GetProductSupplier(string productKey)
		{
			return GetProduct(productKey).Supplier;
		}
	}
}