using System;
using System.Collections.Generic;
using GameStore.BLL.DTOs.EntitiesLocalization;

namespace GameStore.BLL.DTOs
{
	public class PublisherDto
	{
		public int PublisherId { get; set; }

		public Guid? UserGuid { get; set; }

		public string CompanyName { get; set; }

		public string Description { get; set; }

		public string HomePage { get; set; }

		public ICollection<PublisherLocalizationDto> PublisherLocalizations { get; set; }
	}
}