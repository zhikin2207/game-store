using GameStore.WebUI.ViewModels.Game;

namespace GameStore.WebUI.ViewModelBuilders.Interfaces
{
	public interface IGameViewModelBuilder
	{
		/// <summary>
		/// Build GameDetailsViewModel by game key.
		/// </summary>
		/// <param name="gameKey">Game key</param>
		/// <returns>Game details view model</returns>
		GameDetailsViewModel BuildGameDetailsViewModel(string gameKey);

		/// <summary>
		/// Build CreateGameViewModel.
		/// </summary>
		/// <returns>Create game view model</returns>
		CreateEditGameViewModel BuildCreateGameViewModel();

		/// <summary>
		/// Build EditGameViewModel by game key.
		/// </summary>
		/// <param name="gameKey">Game key</param>
		/// <returns>Edit game view model</returns>
		CreateEditGameViewModel BuildEditGameViewModel(string gameKey);

		/// <summary>
		/// Rebuild CreateGameViewModel (Populate genres, platforms, publishers).
		/// </summary>
		/// <param name="createGameViewModel">View model to create the game</param>
		void RebuildCreateGameViewModel(CreateEditGameViewModel createGameViewModel);

		/// <summary>
		/// Rebuild EditGameViewModel (Populate genres, platforms, publishers).
		/// </summary>
		/// <param name="editGameViewModel">View model to update the game</param>
		void RebuildEditGameViewModel(CreateEditGameViewModel editGameViewModel);

		/// <summary>
		/// Rebuild AllGamesViewModel using filters.
		/// </summary>
		/// <param name="allGamesViewModel">View model for all games</param>
		void RebuildAllGamesViewModel(AllGamesViewModel allGamesViewModel);
	}
}