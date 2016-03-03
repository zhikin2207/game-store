using System;
using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore.Interfaces
{
	public interface IUserRepository : IGenericRepository<User, Guid>
	{
		string NonAuthenticatedRoleName { get; }

		string NonAuthenticatedUserName { get; }

		User GetUser(string email);

		bool ValidateUser(string email, string passwordHash);

		bool IsUserInRoles(string email, params string[] roles);
	}
}