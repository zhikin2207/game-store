using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;

namespace GameStore.Tests.DAL
{
	public static class DbSetMockBuilder
	{
		public static Mock<IDbSet<T>> BuildDbSetMock<T>(IEnumerable<T> sourceList) where T : class
		{
			var queryable = sourceList.AsQueryable();

			var dbSet = new Mock<IDbSet<T>>();
			dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
			dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
			dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
			dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator);

			return dbSet;
		}
	}
}
