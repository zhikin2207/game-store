using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Domain.Entities.Components;

namespace GameStore.Domain.Entities
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
	}
}