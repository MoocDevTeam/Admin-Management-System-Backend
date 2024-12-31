using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Mooc.Application.Admin;
using Mooc.Application.Contracts.Admin;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;
using Mooc.Shared;
using Moq;
using Newtonsoft.Json;

namespace Mooc.UnitTest.Service
{
    public class UserServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private List<User> users;
        private string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MockData", "users.json");

        public UserServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
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
        public async Task CreateAsync_WhenNewUserIsCreated_ShouldReturnAUserDto()
        {
            //arrange 
            var users = LoadUsersFromJson(path);
            var options = new DbContextOptionsBuilder<MoocDBContext>()
           .UseInMemoryDatabase("InMemoryDB_1")
           .Options;
            var newUser = new CreateUserDto()
            {
                Id = 6,
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
                context.Users.AddRange(users);
                context.SaveChanges();
                var service = new UserService(context, _mapper, _mockWebHostEnvironment.Object);
                //act
                var result = await service.CreateAsync(newUser);
                // Assert
                Assert.NotNull(result);
                Assert.AreEqual(newUser.UserName, result.UserName);
                Assert.AreEqual(newUser.Age, result.Age);
                Assert.AreEqual(newUser.Email, result.Email);
            }
        }
        [Test]
        public async Task GetByUserName_WhenUserNameExists_ShouldReturnAUsername()
        {
            // Arrange
            var users = LoadUsersFromJson(path);
            var options = new DbContextOptionsBuilder<MoocDBContext>()
           .UseInMemoryDatabase("InMemoryDB_2")
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
                Assert.AreEqual("A1", result.UserName);
            }
        }
        [Test]
        public async Task UpdateAsync_ShouldUpdateUser_WhenUserNameIsUnique()
        {
            // Arrange
            var users = LoadUsersFromJson(path);
            var options = new DbContextOptionsBuilder<MoocDBContext>()
           .UseInMemoryDatabase("InMemoryDB_3")
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
                Assert.AreEqual(updatedUser.UserName, updatedResult.UserName);
            }
        }
    }
}
