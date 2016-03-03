using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
	public class PlatformType
	{
		[Key]
		public int PlatformTypeId { get; set; }

		[Required]
		[StringLength(50)]
		public string Type { get; set; }

		public virtual ICollection<Game> Games { get; set; }
	}
}