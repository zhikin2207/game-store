using System;
using System.ComponentModel.DataAnnotations;
using GameStore.Domain.Entities.Components;

namespace GameStore.Domain.Entities
{
	public abstract class OperationHistory
	{
		[Key]
		public int OperationHistoryId { get; set; }

		public OperationType Type { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime Date { get; set; }
	}
}