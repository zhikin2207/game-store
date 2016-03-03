using GameStore.DAL.Repositories.Northwind.Interfaces;
using GameStore.Domain;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.Repositories.Northwind
{
	public class CategoryRepository : GenericRepository<Category, int>, ICategoryRepository
	{
		public CategoryRepository(BaseDataContext context)
			: base(context)
		{
		}

		public Category GetCategory(string name)
		{
			return Get(c => c.CategoryName == name);
		}

		public bool IsExist(string name)
		{
			return IsExist(c => c.CategoryName == name);
		}
	}
}