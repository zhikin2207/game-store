using System;
using System.Collections.Generic;
using GameStore.BLL.DTOs.EntitiesLocalization;

namespace GameStore.BLL.DTOs
{
	public class GameDto
	{
		public string Key { get; set; }

		public int? PublisherId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public decimal Price { get; set; }

		public short UnitsInStock { get; set; }

		public bool Discontinued { get; set; }

		public DateTime DatePublished { get; set; }

		public bool IsDeleted { get; set; }

		public bool IsReadOnly { get; set; }

		public ICollection<GameLocalizationDto> GameLocalizations { get; set; }
	}
}