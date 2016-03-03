using System;
using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization;

namespace GameStore.BLL.Services
{
	public class PublisherService : IPublisherService
	{
		private readonly IGameStoreUnitOfWork _unitOfWork;

		public PublisherService(IGameStoreUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public PublisherDto GetPublisher(string company)
		{
			if (string.IsNullOrWhiteSpace(company))
			{
				throw new ArgumentNullException("company");
			}

			Publisher publisher = _unitOfWork.PublisherRepository.GetPublisher(company);
			return Mapper.Map<PublisherDto>(publisher);
		}

		public IEnumerable<PublisherDto> GetPublishers()
		{
			IEnumerable<Publisher> publishers = _unitOfWork.PublisherRepository.GetList();
			return Mapper.Map<IEnumerable<PublisherDto>>(publishers);
		}

		public void Create(PublisherDto publisherDto)
		{
			var publisher = Mapper.Map<Publisher>(publisherDto);
			_unitOfWork.PublisherRepository.Add(publisher);
			_unitOfWork.Save();
		}

		public void Delete(string company)
		{
			if (string.IsNullOrWhiteSpace(company))
			{
				throw new ArgumentNullException("company");
			}

			Publisher publisher = _unitOfWork.PublisherRepository.GetPublisher(company);
			_unitOfWork.PublisherRepository.Delete(publisher);
			_unitOfWork.Save();
		}

		public void Update(PublisherDto publisherDto)
		{
			Publisher publisher = _unitOfWork.PublisherRepository.Get(publisherDto.PublisherId);

			publisher.CompanyName = publisherDto.CompanyName;
			publisher.Description = publisherDto.Description;
			publisher.HomePage = publisherDto.HomePage;
			publisher.PublisherLocalizations = Mapper.Map<ICollection<PublisherLocalization>>(publisherDto.PublisherLocalizations);

			_unitOfWork.PublisherRepository.Update(publisher);
			_unitOfWork.Save();
		}

		public bool IsExist(string company)
		{
			if (string.IsNullOrWhiteSpace(company))
			{
				throw new ArgumentNullException(company);
			}

			return _unitOfWork.PublisherRepository.IsExist(company);
		}

		public IEnumerable<GameDto> GetPublisherGames(string company)
		{
			Publisher publisher = _unitOfWork.PublisherRepository.GetPublisher(company);
			return Mapper.Map<IEnumerable<GameDto>>(publisher.Games);
		}
	}
}
