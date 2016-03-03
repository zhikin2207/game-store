namespace GameStore.BLL.Sorting.Components
{
	/// <summary>
	/// Sort filter display options.
	/// </summary>
	public enum GameSortDisplayOptions
	{
		/// <summary>
		/// Display option for the most viewed games.
		/// </summary>
		MostViewed = 0,

		/// <summary>
		/// Display option for the most commented games.
		/// </summary>
		MostCommented = 1,

		/// <summary>
		/// Display option for the most expensive games.
		/// </summary>
		PriceDesc = 2,

		/// <summary>
		/// Display option for the cheapest games.
		/// </summary>
		PriceAsc = 3,

		/// <summary>
		/// Display option for new games.
		/// </summary>
		New = 4
	}
}