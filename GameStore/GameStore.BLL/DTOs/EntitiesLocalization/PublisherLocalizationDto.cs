using GameStore.BLL.DTOs.Components;

namespace GameStore.BLL.DTOs.EntitiesLocalization
{
	public class PublisherLocalizationDto
	{
		public int PublisherLocalizationId { get; set; }

		public LanguageDto Language { get; set; }

		public string CompanyName { get; set; }

		public string Description { get; set; }
	}
}