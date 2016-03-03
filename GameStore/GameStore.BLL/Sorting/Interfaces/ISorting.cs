using System;

namespace GameStore.BLL.Sorting.Interfaces
{
	public interface ISorting<in TEntity> : ISortBase where TEntity : class
	{
		/// <summary>
		/// Gets sort condition.
		/// </summary>
		Func<TEntity, object> SortCondition { get; }

		/// <summary>
		/// Gets a value indicating whether sort order is ascending.
		/// </summary>
		bool IsAscending { get; }
	}
}