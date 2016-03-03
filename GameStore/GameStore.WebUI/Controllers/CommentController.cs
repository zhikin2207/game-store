using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Authentication;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Filters;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Comment;

namespace GameStore.WebUI.Controllers
{
	public class CommentController : BaseController
	{
		protected const string GameDoesNotExistTemplate = "Requested game does not exist: {0}";
		protected const string GameHasEmptyKey = "Requested game has empty key";

		private readonly ICommentService _commentService;
		private readonly IOperationHistoryService _operationHistoryService;
		private readonly ICommentViewModelBuilder _commentViewModelBuilder;

		public CommentController(
			ICommentService commentService,
			IOperationHistoryService operationHistoryService,
			ICommentViewModelBuilder commentViewModelBuilder,
			ILoggingService loggingService) : base(loggingService)
		{
			_commentService = commentService;
			_operationHistoryService = operationHistoryService;
			_commentViewModelBuilder = commentViewModelBuilder;
		}

		[LogError(ExceptionType = typeof(ArgumentNullException), LogMessage = GameHasEmptyKey)]
		[LogError(ExceptionType = typeof(DbUpdateException), LogMessage = GameDoesNotExistTemplate, Parameter = "gameKey")]
		public ActionResult Comments(string gameKey)
		{
			GameCommentsViewModel viewModel = _commentViewModelBuilder.BuildGameCommentsViewModel(gameKey);

			if (Request.IsAjaxRequest())
			{
				return PartialView("_Comments", viewModel.Comments.Where(c => c.Parent == null));
			}

			return View(viewModel);
		}

		[HttpPost]
		[AuthorizeUser(UserRole = UserRole.Moderator | UserRole.User | UserRole.Guest | UserRole.Publisher)]
		[ActionName("NewComment")]
		[LogError(ExceptionType = typeof(ArgumentNullException), LogMessage = GameHasEmptyKey)]
		[LogError(ExceptionType = typeof(InvalidOperationException), LogMessage = GameDoesNotExistTemplate, Parameter = "gameKey")]
		public ActionResult Create(string gameKey, CommentViewModel newComment)
		{
			newComment.GameKey = gameKey;

			var comment = Mapper.Map<CommentDto>(newComment);

			if (!CurrentUser.IsInRole(UserRole.Guest))
			{
				comment.UserGuid = ((UserIdentity)User.Identity).CurrentUser.UserGuid;
			}

			_commentService.Create(comment);

			_operationHistoryService.WriteGameCommentStatistics(gameKey);

			return RedirectToAction("Comments", new { gameKey });
		}

		[HttpPost]
		[AuthorizeUser(UserRole = UserRole.Moderator)]
		[LogError(ExceptionType = typeof(InvalidOperationException), LogMessage = GameDoesNotExistTemplate, Parameter = "gameKey")]
		public ActionResult Delete(int id, string gameKey)
		{
			_commentService.Delete(id);

			return RedirectToAction("Comments", new { gameKey });
		}
	}
}