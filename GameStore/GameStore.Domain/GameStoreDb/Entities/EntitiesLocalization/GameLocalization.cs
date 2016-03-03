using System.ComponentModel.DataAnnotations;
using GameStore.Domain.GameStoreDb.Entities.Components;

namespace GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization
{
	public class GameLocalization
	{
		public int GameLocalizationId { get; set; }

		public Language Language { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		public virtual Game Game { get; set; }
	}
}