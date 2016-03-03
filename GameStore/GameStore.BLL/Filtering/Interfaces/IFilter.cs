using System;

namespace GameStore.BLL.Filtering.Interfaces
{
	public interface IFilter<in TEntity> : IFilterBase where TEntity : class
	{
		/// <summary>
		/// Get filter condition.
		/// </summary>
		/// <returns>Condition to filter</returns>
		Func<TEntity, bool> GetCondition();
	}
}