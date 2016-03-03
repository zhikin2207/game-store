using System;
using GameStore.DAL.Repositories.GameStore;
using GameStore.DAL.Repositories.GameStore.Interfaces;
using GameStore.DAL.Repositories.Northwind;
using GameStore.DAL.Repositories.Northwind.Interfaces;
using GameStore.DAL.RepositoryDecorators;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.NorthwindDb;
using GameStore.Domain.GameStoreDb;

using IOrderRepository = GameStore.DAL.Repositories.GameStore.Interfaces.IOrderRepository;

namespace GameStore.DAL.UnitsOfWork
{
	public class GameStoreUnitOfWork : IGameStoreUnitOfWork, IDisposable
	{
		private readonly GameStoreContext _gameStoreContext;
		private readonly NorthwindEntities _northwindContext;

		#region Repositories

		private readonly Lazy<IGameRepository> _gameRepository;
		private readonly Lazy<ICommentRepository> _commentRepository;
		private readonly Lazy<IPlatformTypeRepository> _platformTypeRepository;
		private readonly Lazy<IGenreRepository> _genreRepository;
		private readonly Lazy<IPublisherRepository> _publisherRepository;
		private readonly Lazy<IOrderRepository> _orderRepository;
		private readonly Lazy<IOperationHistoryRepository> _operationHistoryRepository;
		private readonly Lazy<IShipperRepository> _shipperRepository;
		private readonly Lazy<IUserRepository> _userRepository;
		private readonly Lazy<IRoleRepository> _roleRepository;

		public GameStoreUnitOfWork()
		{
			_gameStoreContext = new GameStoreContext();
			_northwindContext = new NorthwindEntities();

			_gameRepository = new Lazy<IGameRepository>(() => new GameRepositoryDecorator(_gameStoreContext, _northwindContext));
			_genreRepository = new Lazy<IGenreRepository>(() => new GenreRepositoryDecorator(_gameStoreContext, _northwindContext));
			_publisherRepository = new Lazy<IPublisherRepository>(() => new PublisherRepositoryDecorator(_gameStoreContext, _northwindContext));
			_orderRepository = new Lazy<IOrderRepository>(() => new OrderRepositoryDecorator(_gameStoreContext, _northwindContext));
			_commentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(_gameStoreContext));
			_platformTypeRepository = new Lazy<IPlatformTypeRepository>(() => new PlatformTypeRepository(_gameStoreContext));
			_operationHistoryRepository = new Lazy<IOperationHistoryRepository>(() => new OperationHistoryRepository(_gameStoreContext));
			_shipperRepository = new Lazy<IShipperRepository>(() => new ShipperRepository(_northwindContext));
			_userRepository = new Lazy<IUserRepository>(() => new UserRepository(_gameStoreContext));
			_roleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(_gameStoreContext));
		}

		public IGameRepository GameRepository
		{
			get
			{
				return _gameRepository.Value;
			}
		}

		public ICommentRepository CommentRepository
		{
			get
			{
				return _commentRepository.Value;
			}
		}

		public IPlatformTypeRepository PlatformTypeRepository
		{
			get
			{
				return _platformTypeRepository.Value;
			}
		}

		public IGenreRepository GenreRepository
		{
			get
			{
				return _genreRepository.Value;
			}
		}

		public IPublisherRepository PublisherRepository
		{
			get
			{
				return _publisherRepository.Value;
			}
		}

		public IOrderRepository OrderRepository
		{
			get
			{
				return _orderRepository.Value;
			}
		}

		public IOperationHistoryRepository OperationHistoryRepository
		{
			get
			{
				return _operationHistoryRepository.Value;
			}
		}

		public IShipperRepository ShipperRepository
		{
			get
			{
				return _shipperRepository.Value;
			}
		}

		public IUserRepository UserRepository
		{
			get
			{
				return _userRepository.Value;
			}
		}

		public IRoleRepository RoleRepository
		{
			get
			{
				return _roleRepository.Value;
			}
		}

		#endregion

		public void Save()
		{
			_gameStoreContext.SaveChanges();
			_northwindContext.SaveChanges();
		}

		public void Dispose()
		{
			_gameStoreContext.Dispose();
			_northwindContext.Dispose();
		}
	}
}