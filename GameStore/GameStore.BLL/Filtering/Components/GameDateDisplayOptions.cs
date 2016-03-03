namespace GameStore.BLL.Filtering.Components
{
	/// <summary>
	/// Date filter display options (by date added).
	/// </summary>
	public enum GameDateDisplayOptions
	{
		/// <summary>
		/// Display option for all games.
		/// </summary>
		All = 0,

		/// <summary>
		/// Display option for games added last week.
		/// </summary>
		LastWeek = 1,

		/// <summary>
		/// Display option for games added last month.
		/// </summary>
		LastMonth = 2,

		/// <summary>
		/// Display option for games added last year.
		/// </summary>
		LastYear = 3,

		/// <summary>
		/// Display option for games added last 2 years.
		/// </summary>
		TwoYears = 4,

		/// <summary>
		/// Display option for games added last 3 years.
		/// </summary>
		ThreeYears = 5
	}
}