using System.Collections.Generic;
using GameStore.BLL.DTOs.EntitiesLocalization;

namespace GameStore.BLL.DTOs
{
	public class PlatformTypeDto
	{
		public int PlatformTypeId { get; set; }

		public string Type { get; set; }

		public ICollection<PlatformLocalizationDto> PlatformLocalizations { get; set; }
	}
}
