namespace GameStore.BLL.Services.Interfaces
{
	public interface IOperationHistoryService
	{
		/// <summary>
		/// Write statistic when game was viewed.
		/// </summary>
		/// <param name="gameKey">Game key</param>
		void WriteGameViewStatistics(string gameKey);

		/// <summary>
		/// Write statistic when game was added.
		/// </summary>
		/// <param name="gameKey">Game key</param>
		void WriteGameAddStatistics(string gameKey);

		/// <summary>
		/// Write statistic when game was commented.
		/// </summary>
		/// <param name="gameKey">Game key</param>
		void WriteGameCommentStatistics(string gameKey);
	}
}