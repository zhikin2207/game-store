using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Repositories.GameStore;
using GameStore.DAL.Repositories.Northwind;
using GameStore.DAL.RepositoryDecorators.Components;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.RepositoryDecorators
{
	public class PublisherRepositoryDecorator : PublisherRepository
	{
		private readonly BaseDataContext _gameStroreContext;

		private readonly SupplierRepository _supplierRepository;

		public PublisherRepositoryDecorator(BaseDataContext gameContext, BaseDataContext northwindContext)
			: base(gameContext)
		{
			_gameStroreContext = gameContext;
			_supplierRepository = new SupplierRepository(northwindContext);
		}

		public override Publisher GetPublisher(string company)
		{
			Sync();

			Publisher publisher;

			try
			{
				publisher = base.GetPublisher(company);
			}
			catch (InvalidOperationException)
			{
				Supplier supplier = _supplierRepository.GetSupplier(company);
				publisher = Mapper.Map<Publisher>(supplier);
			}

			return publisher;
		}

		public override IEnumerable<Publisher> GetList()
		{
			Sync();

			IEnumerable<Publisher> gameStorePublishers = base.GetList();
			IEnumerable<Publisher> northwindPublishers = GetNorhwindPublishers();

			return gameStorePublishers.Union(northwindPublishers, new PublishersComparer());
		}

		public override bool IsExist(string company)
		{
			Sync();

			bool isExist = base.IsExist(company);

			if (isExist)
			{
				return true;
			}

			return _supplierRepository.IsExist(company);
		}

		private void Sync()
		{
			IEnumerable<Publisher> gameStorePublishers = GetList(p => p.Database == DatabaseName.Northwind);
			IEnumerable<Publisher> northwindPublishers = GetNorhwindPublishers();

			IEnumerable<Publisher> redundantPublishers = gameStorePublishers.Except(northwindPublishers, new PublishersComparer());

			if (redundantPublishers.Any())
			{
				foreach (Publisher publisher in redundantPublishers)
				{
					Delete(publisher);
				}

				_gameStroreContext.SaveChanges();
			}
		}

		private IEnumerable<Publisher> GetNorhwindPublishers()
		{
			IEnumerable<Supplier> suppliers = _supplierRepository.GetList();
			return Mapper.Map<IEnumerable<Publisher>>(suppliers);
		}
	}
}