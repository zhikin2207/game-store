using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization;

namespace GameStore.Domain.GameStoreDb.Entities
{
	public class PlatformType
	{
		[Key]
		public int PlatformTypeId { get; set; }

		[Required]
		[StringLength(50)]
		public string Type { get; set; }

		public virtual ICollection<Game> Games { get; set; }

		public virtual ICollection<PlatformLocalization> PlatformLocalizations { get; set; }
	}
}