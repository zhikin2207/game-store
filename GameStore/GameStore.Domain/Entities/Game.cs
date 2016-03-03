using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Domain.Entities.Components;

namespace GameStore.Domain.Entities
{
	public class Game
	{
		[Key]
		public string Key { get; set; }

		public int? PublisherId { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		public decimal Price { get; set; }

		public short UnitsInStock { get; set; }

		public bool Discontinued { get; set; }

		public DateTime? DatePublished { get; set; }

		public DatabaseName Database { get; set; }

		public bool IsDeleted { get; set; }

		public virtual Publisher Publisher { get; set; }

		public virtual ICollection<GameHistory> GameHistory { get; set; }

		public virtual ICollection<Genre> Genres { get; set; }

		public virtual ICollection<PlatformType> PlatformTypes { get; set; }

		public virtual ICollection<Comment> Comments { get; set; }
	}
}