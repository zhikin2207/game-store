using System;
using System.Linq;
using GameStore.DAL.Repositories.GameStore.Interfaces;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore
{
	public class UserRepository : GenericRepository<User, Guid>, IUserRepository
	{
		private const string GuestRoleName = "Guest";
		private const string GuestUserName = "Guest";

		public UserRepository(BaseDataContext context)
			: base(context)
		{
		}

		public string NonAuthenticatedRoleName
		{
			get
			{
				return GuestRoleName;
			}
		}

		public string NonAuthenticatedUserName
		{
			get
			{
				return GuestUserName;
			}
		}

		public User GetUser(string email)
		{
			return Get(u => u.Email == email);
		}

		public bool ValidateUser(string email, string passwordHash)
		{
			return IsExist(u => u.Email.ToLower() == email.ToLower() && u.Password == passwordHash);
		}

		public bool IsUserInRoles(string email, params string[] roles)
		{
			try
			{
				User user = Get(u => u.Email == email);

				return user.Role != null && roles.Contains(user.Role.Name);
			}
			catch (InvalidOperationException)
			{
				return roles.Contains(GuestRoleName);
			}
		}

		public override void Add(User item)
		{
			item.CreatedAt = DateTime.UtcNow;
			item.UserGuid = Guid.NewGuid();

			base.Add(item);
		}
	}
}