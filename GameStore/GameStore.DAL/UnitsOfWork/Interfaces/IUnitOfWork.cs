namespace GameStore.DAL.UnitsOfWork.Interfaces
{
	public interface IUnitOfWork
	{
		/// <summary>
		/// Saves current state.
		/// </summary>
		void Save();
	}
}