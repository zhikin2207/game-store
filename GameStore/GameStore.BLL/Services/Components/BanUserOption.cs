namespace GameStore.BLL.Services.Components
{
	/// <summary>
	/// Ban user display options.
	/// </summary>
	public enum BanUserOption
	{
		/// <summary>
		/// Ban user for 1 hour.
		/// </summary>
		Hour,

		/// <summary>
		/// Ban user for 1 day.
		/// </summary>
		Day,

		/// <summary>
		/// Ban user for 1 week.
		/// </summary>
		Week,

		/// <summary>
		/// Ban user for 1 month.
		/// </summary>
		Month,

		/// <summary>
		/// Ban user permanently.
		/// </summary>
		Permanent
	}
}