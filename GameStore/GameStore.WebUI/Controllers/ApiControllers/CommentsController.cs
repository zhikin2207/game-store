using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.Controllers.ApiControllers
{
	public class CommentsController : ApiController
	{
		private readonly ICommentService _commentService;

		public CommentsController(ICommentService commentService)
		{
			_commentService = commentService;
		}

		[ActionName("Default")]
		public HttpResponseMessage Get(string key)
		{
			IEnumerable<CommentDto> comments = _commentService.GetComments(key);
			return Request.CreateResponse(HttpStatusCode.OK, comments);
		}

		[ActionName("Default")]
		public HttpResponseMessage Get(string key, int id)
		{
			CommentDto comment = _commentService.GetComment(key, id);
			return Request.CreateResponse(HttpStatusCode.OK, comment);
		}

		[ActionName("Default")]
		[AuthorizeAPIUser(UserRole = UserRole.Guest | UserRole.User | UserRole.Moderator | UserRole.Publisher)]
		public HttpResponseMessage Post([FromUri]string key, [FromBody]CommentViewModel viewModel)
		{
			viewModel.GameKey = key;

			var comment = Mapper.Map<CommentDto>(viewModel);
			_commentService.Create(comment);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[ActionName("Default")]
		[AuthorizeAPIUser(UserRole = UserRole.Moderator)]
		public HttpResponseMessage Delete(int id)
		{
			_commentService.Delete(id);
			return Request.CreateResponse(HttpStatusCode.OK);
		}
	}
}