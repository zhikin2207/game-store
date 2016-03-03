using GameStore.DAL.Repositories.GameStore.Interfaces;
using GameStore.DAL.Repositories.Northwind.Interfaces;
using IOrderRepository = GameStore.DAL.Repositories.GameStore.Interfaces.IOrderRepository;

namespace GameStore.DAL.UnitsOfWork.Interfaces
{
	public interface IGameStoreUnitOfWork : IUnitOfWork
	{
		/// <summary>
		/// Gets game repository.
		/// </summary>
		IGameRepository GameRepository { get; }

		/// <summary>
		/// Gets comment repository.
		/// </summary>
		ICommentRepository CommentRepository { get; }

		/// <summary>
		/// Gets platform repository.
		/// </summary>
		IPlatformTypeRepository PlatformTypeRepository { get; }

		/// <summary>
		/// Gets genre repository.
		/// </summary>
		IGenreRepository GenreRepository { get; }

		/// <summary>
		/// Gets publisher repository.
		/// </summary>
		IPublisherRepository PublisherRepository { get; }

		/// <summary>
		/// Gets order repository.
		/// </summary>
		IOrderRepository OrderRepository { get; }

		/// <summary>
		/// Gets operation history repository.
		/// </summary>
		IOperationHistoryRepository OperationHistoryRepository { get; }

		IShipperRepository ShipperRepository { get; }

		IUserRepository UserRepository { get; }

		IRoleRepository RoleRepository { get; }
	}
}
