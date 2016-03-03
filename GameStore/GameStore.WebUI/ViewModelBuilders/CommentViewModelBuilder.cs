using System;
using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Comment;

namespace GameStore.WebUI.ViewModelBuilders
{
	public class CommentViewModelBuilder : ICommentViewModelBuilder
	{
		private readonly ICommentService _commentService;
		private readonly IGameService _gameService;

		public CommentViewModelBuilder(ICommentService commentService, IGameService gameService)
		{
			_commentService = commentService;
			_gameService = gameService;
		}

		public GameCommentsViewModel BuildGameCommentsViewModel(string gameKey)
		{
			if (string.IsNullOrWhiteSpace(gameKey))
			{
				throw new ArgumentNullException("gameKey");
			}

			GameDto gameDto = _gameService.GetGame(gameKey);
			IEnumerable<CommentDto> commentDtos = _commentService.GetComments(gameKey);

			return new GameCommentsViewModel
			{
				Game = Mapper.Map<GameViewModel>(gameDto),
				Comments = Mapper.Map<IEnumerable<CommentViewModel>>(commentDtos)
			};
		}
	}
}