using System;
using System.ComponentModel.DataAnnotations;
using GameStore.Domain.GameStoreDb.Entities.Components;

namespace GameStore.Domain.GameStoreDb.Entities
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