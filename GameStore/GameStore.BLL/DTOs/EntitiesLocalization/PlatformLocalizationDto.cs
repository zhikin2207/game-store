using System.ComponentModel.DataAnnotations;
using GameStore.BLL.DTOs.Components;

namespace GameStore.BLL.DTOs.EntitiesLocalization
{
	public class PlatformLocalizationDto
	{
		public LanguageDto Language { get; set; }

		[Required]
		[StringLength(50)]
		public string Type { get; set; }
	}
}