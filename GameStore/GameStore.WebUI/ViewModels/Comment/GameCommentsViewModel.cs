using System.Collections.Generic;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.ViewModels.Comment
{
	public class GameCommentsViewModel
	{
		public GameCommentsViewModel()
		{
			NewComment = new CommentViewModel();
		}

		public GameViewModel Game { get; set; }

		public IEnumerable<CommentViewModel> Comments { get; set; }

		public CommentViewModel NewComment { get; set; }
	}
}