using System.ComponentModel.DataAnnotations;
using GameStore.Domain.GameStoreDb.Entities.Components;

namespace GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization
{
	public class GenreLocalization
	{
		public int GenreLocalizationId { get; set; }

		public Language Language { get; set; }

		[StringLength(50)]
		public string Name { get; set; }

		public virtual Genre Genre { get; set; }
	}
}