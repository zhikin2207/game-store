using System;
using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Services
{
	public class CommentService : ICommentService
	{
		private readonly IGameStoreUnitOfWork _unitOfWork;

		public CommentService(IGameStoreUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<CommentDto> GetComments(string gameKey)
		{
			if (string.IsNullOrWhiteSpace(gameKey))
			{
				throw new ArgumentNullException("gameKey");
			}

			IEnumerable<Comment> comments = _unitOfWork.CommentRepository.Comments(gameKey);
			return Mapper.Map<IEnumerable<CommentDto>>(comments);
		}

		public CommentDto GetComment(string gameKey, int id)
		{
			if (string.IsNullOrEmpty(gameKey))
			{
				throw new ArgumentNullException("gameKey");
			}

			Comment comment = _unitOfWork.CommentRepository.GetComment(gameKey, id);
			return Mapper.Map<CommentDto>(comment);
		}

		public void Create(CommentDto commentDto)
		{
			var comment = Mapper.Map<Comment>(commentDto);
			_unitOfWork.CommentRepository.Add(comment);
			_unitOfWork.Save();
		}

		public void Delete(int id)
		{
			Comment comment = _unitOfWork.CommentRepository.Get(id);
			_unitOfWork.CommentRepository.Delete(comment);
			_unitOfWork.Save();
		}
	}
}