using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Components;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Services
{
	public class UserService : IUserService
	{
		private readonly IGameStoreUnitOfWork _unitOfWork;

		public UserService(IGameStoreUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public string NonAuthenticatedRoleName
		{
			get
			{
				return _unitOfWork.UserRepository.NonAuthenticatedRoleName;
			}
		}

		public string NonAuthenticatedUserName
		{
			get
			{
				return _unitOfWork.UserRepository.NonAuthenticatedUserName;
			}
		}

	    public IEnumerable<UserDto> GetUsers()
	    {
	        IEnumerable<User> users = _unitOfWork.UserRepository.GetList();
	        return Mapper.Map<IEnumerable<UserDto>>(users);
	    }

		public void Create(UserDto userDto, string roleName)
		{
			Role role = _unitOfWork.RoleRepository.GetRole(roleName);

			var user = Mapper.Map<User>(userDto);
			user.Password = HashPassword(user.Password);
			user.Role = role;

			_unitOfWork.UserRepository.Add(user);
			_unitOfWork.Save();
		}
		
		public void Ban(Guid userGuid, BanUserOption banOption)
		{
			User user = _unitOfWork.UserRepository.Get(userGuid);
			user.IsBanned = true;

			switch (banOption)
			{
				case BanUserOption.Hour:
					user.BanReleaseAt = DateTime.UtcNow.AddHours(1);
					break;
				case BanUserOption.Day:
					user.BanReleaseAt = DateTime.UtcNow.AddDays(1);
					break;
				case BanUserOption.Week:
					user.BanReleaseAt = DateTime.UtcNow.AddDays(7);
					break;
				case BanUserOption.Month:
					user.BanReleaseAt = DateTime.UtcNow.AddMonths(1);
					break;
				default:
					// :)
					user.BanReleaseAt = DateTime.UtcNow.AddYears(1000);
					break;
			}

			_unitOfWork.UserRepository.Update(user);
			_unitOfWork.Save();
		}

		public void Unban(Guid userGuid)
		{
			User user = _unitOfWork.UserRepository.Get(userGuid);
			user.IsBanned = false;

			_unitOfWork.UserRepository.Update(user);
			_unitOfWork.Save();
		}

		public void Delete(Guid userGuid)
		{
			User user = _unitOfWork.UserRepository.Get(userGuid);

			_unitOfWork.UserRepository.Delete(user);
			_unitOfWork.Save();
		}

		public bool ValidateUser(string email, string password)
		{
			string passwordHash = HashPassword(password);

			bool isUserValid = _unitOfWork.UserRepository.ValidateUser(email, passwordHash);

			if (isUserValid)
			{
				ValidateUserForBan(email);
			}

			return isUserValid;
		}

		public UserDto GetUser(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				throw new ArgumentNullException(email, "User email can not be null");
			}

			User user = _unitOfWork.UserRepository.GetUser(email);
			return Mapper.Map<UserDto>(user);
		}

	    public UserDto GetUser(Guid userGuid)
	    {
	        User user = _unitOfWork.UserRepository.Get(userGuid);
	        return Mapper.Map<UserDto>(user);
	    }

		public IEnumerable<RoleDto> GetRoles()
		{
			IEnumerable<Role> roles = _unitOfWork.RoleRepository.GetList();
			return Mapper.Map<IEnumerable<RoleDto>>(roles);
		}

		public void ChangeUserRole(Guid userGuid, int newRoleId)
		{
			User user = _unitOfWork.UserRepository.Get(userGuid);
			user.RoleId = newRoleId;

			_unitOfWork.UserRepository.Update(user);
			_unitOfWork.Save();
		}

		public void ChangeUserLanguage(Guid userGuid, string lang)
		{
			User user = _unitOfWork.UserRepository.Get(userGuid);
			user.Language = lang;

			_unitOfWork.UserRepository.Update(user);
			_unitOfWork.Save();
		}

		public bool IsUserInRole(string email, params string[] roles)
		{
			return _unitOfWork.UserRepository.IsUserInRoles(email, roles);
		}

		public IEnumerable<PublisherDto> GetPublishers()
		{
			IEnumerable<Publisher> publishers = _unitOfWork.PublisherRepository.GetList();
			return Mapper.Map<IEnumerable<PublisherDto>>(publishers);
		}

		public void ChangeUserPublisher(Guid userGuid, string companyName)
		{
			User user = _unitOfWork.UserRepository.Get(userGuid);
			Publisher publisher = _unitOfWork.PublisherRepository.GetPublisher(companyName);

			user.Publisher = publisher;

			_unitOfWork.UserRepository.Update(user);
			_unitOfWork.Save();
		}

		private string HashPassword(string password)
		{
			var bytes = Encoding.UTF8.GetBytes(password);

			using (MD5 md5Hash = MD5.Create())
			{
				var hash = md5Hash.ComputeHash(bytes);
				return ToHexString(hash);
			}
		}

		private string ToHexString(byte[] bytes)
		{
			var result = new StringBuilder();

			for (int i = 0; i < bytes.Length; i++)
			{
				result.Append(bytes[i].ToString("x2"));
			}

			return result.ToString();
		}

		private void ValidateUserForBan(string email)
		{
			User user = _unitOfWork.UserRepository.GetUser(email);

			if (user.IsBanned)
			{
				if (DateTime.UtcNow > user.BanReleaseAt)
				{
					user.IsBanned = false;
					user.BanReleaseAt = null;

					_unitOfWork.UserRepository.Update(user);
					_unitOfWork.Save();
				}
			}
		}
	}
}