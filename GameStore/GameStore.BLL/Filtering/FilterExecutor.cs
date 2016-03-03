using System;
using System.Collections.Generic;
using GameStore.BLL.Filtering.Interfaces;

namespace GameStore.BLL.Filtering
{
	public class FilterExecutor<T> where T : class
	{
		private readonly List<IFilter<T>> _filters = new List<IFilter<T>>();

		public void Register(IFilter<T> filter)
		{
			_filters.Add(filter);
		}

		public IEnumerable<Func<T, bool>> GetConditions()
		{
			var conditions = new List<Func<T, bool>>();

			foreach (IFilter<T> filter in _filters)
			{
				if (filter.IsSet)
				{
					conditions.Add(filter.GetCondition());
				}
			}

			return conditions;
		}
	}
}