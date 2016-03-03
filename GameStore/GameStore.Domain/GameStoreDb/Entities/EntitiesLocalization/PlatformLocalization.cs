using System.ComponentModel.DataAnnotations;
using GameStore.Domain.GameStoreDb.Entities.Components;

namespace GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization
{
	public class PlatformLocalization
	{
		public int PlatformLocalizationId { get; set; }

		public Language Language { get; set; }

		[Required]
		[StringLength(50)]
		public string Type { get; set; }

		public virtual PlatformType PlatformType { get; set; }
	}
}