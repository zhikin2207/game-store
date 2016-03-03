using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.Domain.GameStoreDb.Entities.Components;
using GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization;

namespace GameStore.Domain.GameStoreDb.Entities
{
	public class Genre
	{
		[Key]
		public int GenreId { get; set; }

		[ForeignKey("Parent")]
		public int? ParentGenreId { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		public DatabaseName Database { get; set; }

		public virtual Genre Parent { get; set; }

		public virtual ICollection<Genre> Children { get; set; }

		public virtual ICollection<GenreLocalization> GenreLocalizations { get; set; }

		public virtual ICollection<Game> Games { get; set; }
	}
}