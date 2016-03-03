using System.ComponentModel.DataAnnotations;
using GameStore.Domain.GameStoreDb.Entities.Components;

namespace GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization
{
	public class PublisherLocalization
	{
		public int PublisherLocalizationId { get; set; }

		public Language Language { get; set; }

		[Required]
		[StringLength(40)]
		public string CompanyName { get; set; }

		public string Description { get; set; }

		public virtual Publisher Publisher { get; set; }
	}
}