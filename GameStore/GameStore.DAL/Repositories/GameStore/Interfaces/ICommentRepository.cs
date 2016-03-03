using System.Collections.Generic;
using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore.Interfaces
{
	public interface ICommentRepository : IGenericRepository<Comment, int>
	{
		/// <summary>
		/// Get comments by game key.
		/// </summary>
		/// <param name="gameKey">Game key</param>
		/// <returns>List of comments</returns>
		IEnumerable<Comment> Comments(string gameKey);

		Comment GetComment(string gameKey, int id);
	}
}