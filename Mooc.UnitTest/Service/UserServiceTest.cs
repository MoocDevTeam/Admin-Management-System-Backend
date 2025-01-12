﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Mooc.Application.Admin;
using Mooc.Application.Contracts.Admin;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;
using Mooc.Shared;
using Moq;
using Newtonsoft.Json;
using System.Net.Mime;

namespace Mooc.UnitTest.Service
{
    public class UserServiceTest
    {
       

        private readonly IMapper _mapper;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MockData", "users.json");

        public UserServiceTest()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<User, UserDto>();
                    cfg.CreateMap<CreateUserDto, User>();
                    cfg.CreateMap<UpdateUserDto, User>();
                });
            _mapper = config.CreateMapper();
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        private List<User> LoadUsersFromJson(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<User>>(json);
            }
        }


        [Test]

        public async Task CreateAsync_WithValidRoleIds_ShouldReturnAUserDto_AndAssignRolesToUser()
        {
            //arrange
            var users = LoadUsersFromJson(path);
            var roles = new List<Role>()
            {
                new Role { Id = 1, RoleName = "Admin", Description = "zzz" },
                new Role { Id = 2, RoleName = "User", Description = "zzz" },
                new Role { Id = 3, RoleName = "Manager", Description = "zzz" }
            };
            var options = new DbContextOptionsBuilder<MoocDBContext>().UseInMemoryDatabase("InMemoryDB_POST").Options;
          //var options = new DbContextOptionsBuilder<MoocDBContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var newUser = new CreateUserDto()
            {
                // Removed Id=6 since it's usually generated by the database -> join table uses composite primary key
                UserName = "A6",
                Password = "123",
                Age = 1,
                Email = "abc@uow.edu.au",
                Gender = Gender.Male,
                Avatar = "123",
                RoleIds = new List<long>() { 1, 2, 3 }

            };

            using (var context = new MoocDBContext(options))
            {
                context.Roles.AddRange(roles); // Add the roles first -> Creating UserRole entries that reference non-existent roles would lead to invalid references.
                context.Users.AddRange(users);
                context.SaveChanges();

                var service = new UserService(context, _mapper, _mockWebHostEnvironment.Object);

                //act
                var result = await service.CreateAsync(newUser);

                //assert
                Assert.NotNull(result);
                Assert.That(result.UserName, Is.EqualTo(newUser.UserName));
                Assert.That(result.Age, Is.EqualTo(newUser.Age));
                Assert.That(result.Email, Is.EqualTo(newUser.Email));

                // Verify that the roles are correctly assigned
                var createdUser = await context.Users
                    .Include(u => u.UserRoles)
                    .FirstOrDefaultAsync(u => u.UserName == newUser.UserName);
                Assert.NotNull(createdUser);
                Assert.AreEqual(createdUser.UserRoles.Count, 3);
                Assert.IsTrue(createdUser.UserRoles.Any(ur => ur.RoleId == 1));
                CollectionAssert.AreEquivalent(newUser.RoleIds, createdUser.UserRoles.Select(ur => ur.RoleId));



            }

        }


        [Test]
        public async Task GetByUserName_WhenUserNameExists_ShouldReturnAUsername()
        {
            // Arrange
            var users = LoadUsersFromJson(path);
            var options = new DbContextOptionsBuilder<MoocDBContext>()
           .UseInMemoryDatabase("InMemoryDB_GET")
           .Options;
            using (var context = new MoocDBContext(options))
            {
                context.Users.AddRange(users);
                context.SaveChanges();
                var service = new UserService(context, _mapper, _mockWebHostEnvironment.Object);
                //act
                var result = await service.GetByUserNameAsync("A1");
                var inValidResult = await service.GetByUserNameAsync("nonExistingUser");
                //Assert
                Assert.NotNull(result);
                Assert.Null(inValidResult);
                Assert.That(result.UserName, Is.EqualTo("A1"));
            }
        }
        [Test]
        public async Task UpdateAsync_ShouldUpdateUser_WhenUserNameIsUnique()
        {
            // Arrange
            var users = LoadUsersFromJson(path);
            var options = new DbContextOptionsBuilder<MoocDBContext>()
           .UseInMemoryDatabase("InMemoryDB_PUT")
           .Options;
            var updatedUser = new UpdateUserDto()
            {
                Id = 5,
                UserName = "A6",
                Password = "123",
                Age = 1,
                Email = "abc@uow.edu.au",
                Gender = Gender.Male,
                Avatar = "123",
            };
            using (var context = new MoocDBContext(options))
            {
                context.Users.AddRange(users);
                context.SaveChanges();
                var service = new UserService(context, _mapper, _mockWebHostEnvironment.Object);
                //act
                var updatedResult = await service.UpdateAsync(5, updatedUser);
                //Assert
                Assert.NotNull(updatedResult);
                Assert.That( updatedResult.UserName, Is.EqualTo(updatedUser.UserName));
            }
        }

        [Test]
        public async Task UpdateAsync_WithValidRoleIds_ShouldUpdateUserRoles()
        {
            //arrange 
            var users = LoadUsersFromJson(path);
            var roles = new List<Role>
            {
                new Role { Id = 1, RoleName = "Admin", Description = "zzz" },
                new Role { Id = 2, RoleName = "User", Description = "zzz" },
                new Role { Id = 3, RoleName = "Manager", Description = "zzz" },
                new Role { Id = 4, RoleName = "Guest", Description = "zzz" }
            };
            var options = new DbContextOptionsBuilder<MoocDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var updatedUser = new UpdateUserDto()
            {
                Id = 2, // Id=2 exists in users.json
                UserName = "UpdatedUser",
                Password = "NewSecurePassword",
                Age = 28,
                Email = "updateduser@uow.edu.au",
                Gender = Gender.Male,
                Avatar = "new_avatar.png",
                RoleIds = new List<long> { 2, 4 }
            };

            //act
            UserDto updatedResult;
            using (var context = new MoocDBContext(options))
            {
                context.Users.AddRange(users);
                context.Roles.AddRange(roles);
                context.SaveChanges();

                var service = new UserService(context, _mapper, _mockWebHostEnvironment.Object);
                updatedResult = await service.UpdateAsync(2, updatedUser);
            }

            //assert
            Assert.NotNull(updatedResult);
            Assert.That(updatedResult.UserName, Is.EqualTo(updatedUser.UserName));

            using (var context = new MoocDBContext(options))
            {
                var findUpdatedUser = await context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == 2);
                Assert.NotNull(findUpdatedUser);
                Assert.AreEqual(2, findUpdatedUser.UserRoles.Count);
                Assert.IsTrue(findUpdatedUser.UserRoles.Any(ur => ur.RoleId == 2));
                Assert.IsTrue(findUpdatedUser.UserRoles.Any(ur => ur.RoleId == 4));
                // Ensure previous roles are removed if not included in RoleIds
                Assert.IsFalse(findUpdatedUser.UserRoles.Any(ur => ur.RoleId == 1 || ur.RoleId == 3));
            }
        }
    }
}
