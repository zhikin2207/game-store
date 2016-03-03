using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.GameStoreDb.Entities
{
	public class Comment
	{
		[Key]
		public int CommentId { get; set; }

		[ForeignKey("Parent")]
		public int? ParentCommentId { get; set; }

		[ForeignKey("User")]
		public Guid? UserGuid { get; set; }

		[Required]
		public string GameKey { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Body { get; set; }

		[Required]
		public DateTime Date { get; set; }

		public virtual Comment Parent { get; set; }

		public virtual ICollection<Comment> Children { get; set; }

		public virtual Game Game { get; set; }

		public virtual User User { get; set; }
	}
}