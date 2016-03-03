using GameStore.WebUI.ViewModels.Comment;

namespace GameStore.WebUI.ViewModelBuilders.Interfaces
{
	public interface ICommentViewModelBuilder
	{
		/// <summary>
		/// Build game comments view model by game key.
		/// </summary>
		/// <param name="gameKey">Game key</param>
		/// <returns>Game comments view model</returns>
		GameCommentsViewModel BuildGameCommentsViewModel(string gameKey);
	}
}