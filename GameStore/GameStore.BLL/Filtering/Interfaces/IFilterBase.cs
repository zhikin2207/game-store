namespace GameStore.BLL.Filtering.Interfaces
{
	public interface IFilterBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether filter is ready to be applied.
		/// </summary>
		bool IsSet { get; set; }
	}
}