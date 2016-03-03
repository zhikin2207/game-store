using System.Collections.Generic;
using GameStore.BLL.DTOs;

namespace GameStore.BLL.Services.Interfaces
{
	public interface ICommentService
	{
		/// <summary>
		/// Get comments by game key.
		/// </summary>
		/// <param name="gameKey">Game key</param>
		/// <returns>List of comments</returns>
		IEnumerable<CommentDto> GetComments(string gameKey);

		/// <summary>
		/// Create new comment
		/// </summary>
		/// <param name="comment">Comment model</param>
		void Create(CommentDto comment);

		/// <summary>
		/// Delete comment by id.
		/// </summary>
		/// <param name="id">Comment id</param>
		void Delete(int id);

		CommentDto GetComment(string gameKey, int id);
	}
}