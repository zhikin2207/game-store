namespace GameStore.WebUI.ViewModels.Filter.Interfaces
{
	public interface IViewModelFilter
	{
		/// <summary>
		/// Gets a value indicating whether filter is ready to be applied.
		/// </summary>
		bool IsSet { get; }
	}
}