using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Domain.GameStoreDb.Entities.Components;
using GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization;

namespace GameStore.Domain.GameStoreDb.Entities
{
	public class Publisher
	{
		[Key]
		public int PublisherId { get; set; }

		[Required]
		[StringLength(40)]
		public string CompanyName { get; set; }

		public string Description { get; set; }

		public string HomePage { get; set; }

		public DatabaseName Database { get; set; }

		public virtual ICollection<Game> Games { get; set; }

		public virtual ICollection<PublisherLocalization> PublisherLocalizations { get; set; }
	}
}