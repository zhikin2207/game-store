using System.ComponentModel.DataAnnotations;
using GameStore.BLL.DTOs.Components;

namespace GameStore.BLL.DTOs.EntitiesLocalization
{
	public class GameLocalizationDto
	{
		public LanguageDto Language { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }
	}
}