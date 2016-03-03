namespace GameStore.Domain.GameStoreDb.Entities.Components
{
	/// <summary>
	/// Operation type for OperationHistory table.
	/// </summary>
	public enum OperationType
	{
		/// <summary>
		/// Operation type when game was added.
		/// </summary>
		Add,

		/// <summary>
		/// Operation type when game was viewed.
		/// </summary>
		View,

		/// <summary>
		/// Operation type when game was commented.
		/// </summary>
		Comment
	}
}