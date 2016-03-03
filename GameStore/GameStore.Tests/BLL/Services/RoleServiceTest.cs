using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Mappings;
using GameStore.BLL.Services;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.BLL.Services
{
	[TestClass]
	public class RoleServiceTest
	{
		private Mock<IGameStoreUnitOfWork> _mockUnitOfWork;
		private IRoleService _roleService;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			Mapper.Initialize(cfg => cfg.AddProfile<MappingBllProfile>());
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockUnitOfWork = new Mock<IGameStoreUnitOfWork>();
			_roleService = new RoleService(_mockUnitOfWork.Object);
		}

		#region GetRoles method

		[TestMethod]
		public void GetRoles_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.RoleRepository.GetList()).Returns(Enumerable.Empty<Role>());

			IEnumerable<RoleDto> roles = _roleService.GetRoles();

			Assert.IsNotNull(roles);
		}

		[TestMethod]
		public void GetRoles_Maps_Roles_Appropriately()
		{
			IEnumerable<Role> roles = new[]
			{
				new Role { RoleId = 1, Name = "role-1", IsSystem = true },
				new Role { RoleId = 2, Name = "role-2", IsSystem = false },
				new Role { RoleId = 2, Name = "role-3", IsSystem = false }
			};

			_mockUnitOfWork.Setup(u => u.RoleRepository.GetList()).Returns(roles);

			IEnumerable<RoleDto> roleDtos = _roleService.GetRoles();

			Assert.AreEqual(3, roleDtos.Count());
			for (int i = 0; i < roleDtos.Count(); i++)
			{
				Assert.AreEqual(roles.ElementAt(i).RoleId, roleDtos.ElementAt(i).RoleId);
				Assert.AreEqual(roles.ElementAt(i).Name, roleDtos.ElementAt(i).Name);
				Assert.AreEqual(roles.ElementAt(i).IsSystem, roleDtos.ElementAt(i).IsSystem);
			}
		}

		#endregion

		#region GetRole method

		[TestMethod]
		public void GetRole_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(s => s.RoleRepository.Get(It.IsAny<int>())).Returns(new Role());

			RoleDto result = _roleService.GetRole(1);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void GetRole_Maps_Role_Correctly()
		{
			var role = new Role { RoleId = 1, Name = "role-1", IsSystem = true };

			_mockUnitOfWork.Setup(s => s.RoleRepository.Get(It.IsAny<int>())).Returns(role);

			RoleDto result = _roleService.GetRole(1);

			Assert.AreEqual(role.RoleId, result.RoleId);
			Assert.AreEqual(role.Name, result.Name);
			Assert.AreEqual(role.IsSystem, result.IsSystem);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
		public void GetRole_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(s => s.RoleRepository.Get(It.IsAny<int>())).Throws<Exception>();

			_roleService.GetRole(1);
		}

		#endregion

		#region Create method

		[TestMethod]
		public void Create_Maps_Role_Appropriately()
		{
			Role roleResult = null;

			var roleDto = new RoleDto { RoleId = 1, Name = "role-1", IsSystem = true };

			_mockUnitOfWork.Setup(m => m.RoleRepository.Add(It.IsAny<Role>())).Callback<Role>(role =>
			{
				roleResult = role;
			});

			_roleService.Create(roleDto);

			Assert.AreEqual(roleDto.RoleId, roleResult.RoleId);
			Assert.AreEqual(roleDto.Name, roleResult.Name);
			Assert.AreEqual(roleDto.IsSystem, roleResult.IsSystem);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Create_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.RoleRepository.Add(It.IsAny<Role>())).Throws<Exception>();

			_roleService.Create(new RoleDto());
		}

		[TestMethod]
		public void Create_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.RoleRepository.Add(It.IsAny<Role>()));

			_roleService.Create(new RoleDto());

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		#endregion

		#region Update method

		[TestMethod]
		public void Update_Maps_Role_Appropriately()
		{
			Role roleResult = null;

			var roleDto = new RoleDto { RoleId = 1, Name = "role-1" };

			_mockUnitOfWork.Setup(m => m.RoleRepository.Get(It.IsAny<int>())).Returns(new Role());
			_mockUnitOfWork.Setup(m => m.RoleRepository.Update(It.IsAny<Role>())).Callback<Role>(role =>
			{
				roleResult = role;
			});

			_roleService.Update(roleDto);

			Assert.AreEqual(roleDto.Name, roleResult.Name);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Update_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.RoleRepository.Get(It.IsAny<int>())).Returns(new Role());
			_mockUnitOfWork.Setup(m => m.RoleRepository.Update(It.IsAny<Role>())).Throws<Exception>();

			_roleService.Update(new RoleDto());
		}

		[TestMethod]
		public void Update_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.RoleRepository.Get(It.IsAny<int>())).Returns(new Role());

			_roleService.Update(new RoleDto());

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		#endregion

		#region Delete method

		[TestMethod]
		public void Delete_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.RoleRepository.Get(It.IsAny<int>())).Returns(new Role());

			_roleService.Delete(1);

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Delete_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.RoleRepository.Get(It.IsAny<int>())).Returns(new Role());
			_mockUnitOfWork.Setup(m => m.RoleRepository.Delete(It.IsAny<Role>())).Throws<Exception>();

			_roleService.Delete(1);
		}

		#endregion
	}
}