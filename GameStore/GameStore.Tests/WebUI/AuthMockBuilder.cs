using System;
using GameStore.BLL.DTOs;
using GameStore.WebUI.Authentication;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Authentication.Interfaces;
using Moq;

namespace GameStore.Tests.WebUI
{
	public class AuthMockBuilder
	{
		public const string DefaultUserName = "User name";

		private readonly Mock<IUserProvider> _mockUserProvider;
		private readonly UserDto _user;

		private AuthMockBuilder(UserRole userRole)
		{
			_mockUserProvider = new Mock<IUserProvider>();
			_user = new UserDto();

			_mockUserProvider
				.Setup(m => m.IsInRole(It.IsAny<string>()))
				.Returns<string>(role => role == userRole.ToString());

			_mockUserProvider
				.Setup(m => m.IsInRole(It.IsAny<UserRole>()))
				.Returns<UserRole>(role => role.HasFlag(userRole));
		}

		public static AuthMockBuilder Build(UserRole userRole)
		{
			return new AuthMockBuilder(userRole);
		}

		public AuthMockBuilder Authenticate()
		{
			_mockUserProvider.SetupGet(p => p.Identity.IsAuthenticated).Returns(true);
			return this;
		}

		public AuthMockBuilder SetPublisher(string company)
		{
			_user.Publisher = new PublisherDto
			{
				CompanyName = company
			};

			return this;
		}

		public AuthMockBuilder SetName(string name)
		{
			_mockUserProvider.SetupGet(p => p.Identity.Name).Returns(name);
			return this;
		} 

		public Mock<IUserProvider> Get()
		{
			_user.UserGuid = Guid.NewGuid();

			_mockUserProvider.SetupGet(p => p.Identity).Returns(new UserIdentity(_user));

			return _mockUserProvider;
		}
	}
}
