using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DAL.Repositories.GameStore.Interfaces;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore
{
	public class CommentRepository : GenericRepository<Comment, int>, ICommentRepository
	{
		public CommentRepository(BaseDataContext context)
			: base(context)
		{
		}

		public IEnumerable<Comment> Comments(string gameKey)
		{
			return GetList(comment => comment.GameKey == gameKey);
		}

		public Comment GetComment(string gameKey, int id)
		{
			Comment comment = Get(c => c.GameKey == gameKey && c.CommentId == id);
			return comment;
		}

		public override void Add(Comment item)
		{
			item.Date = DateTime.UtcNow;
			base.Add(item);
		}

		public override void Delete(Comment item)
		{
			DeleteCommentRecursively(item);
		}

		private void DeleteCommentRecursively(Comment comment)
		{
			if (comment.Children != null)
			{
				while (comment.Children.Any())
				{
					Comment childComment = comment.Children.First();
					DeleteCommentRecursively(childComment);
				}
			}

			base.Delete(comment);
		}
	}
}