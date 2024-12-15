using Autofac.Core;
using Mooc.Application.Admin;
using Mooc.Application.Contracts.Admin;
using Mooc.Application.Contracts.Dto;
using Mooc.Shared;
using MoocWebApi.Controllers.Admin;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using  Mooc.Model.DBContext;
using Mooc.Model.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Mooc.Core.ExceptionHandling;
using NLog.Web.LayoutRenderers;



namespace Mooc.UnitTest
{
    public class UserServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly List<User> users;
        private readonly string path = "./MockData/users.json";
        public UserServiceTests()
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

        private static List<User> LoadUsersFromJson(string filePath)
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
                Phone = "0401499796",
                Address = "Jane Street",
                Gender = Gender.Male,
                Avatar = "123",
                RoleIds = new List<long>() { 1, 2, 3 }
            };

            using var context = new MoocDBContext(options); 
            context.Users.AddRange(users);
            await context.SaveChangesAsync();
            var service = new UserService(context, _mapper, _mockWebHostEnvironment.Object);
           
            //action
            var result = await service.CreateAsync(newUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserName == newUser.UserName, "Username is correct");
            Assert.That(result.Phone == newUser.Phone, "Phone is correct");
            Assert.That(result.Email == newUser.Email, "Email is corrct");
            Assert.That(result.Age == newUser.Age, "Age is correct");
        }


        [Test]
        public async Task GetByUserName_WhenUserNameExists_ShouldReturnAUsername()
        {
            // Arrange
            var users = LoadUsersFromJson(path);
            var options = new DbContextOptionsBuilder<MoocDBContext>()
           .UseInMemoryDatabase("InMemoryDB_2")
           .Options;
            var context = new MoocDBContext(options);
            
            context.Users.AddRange(users);
            context.SaveChanges();
            var service = new UserService(context, _mapper, _mockWebHostEnvironment.Object);
            //action
            var result = await service.GetByUserNameAsync("A1");
            var inValidResult = await service.GetByUserNameAsync("nonExistingUser");

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNull(inValidResult);
            Assert.That(result.UserName == "A1", "UserName is Corrct");
            

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
                Phone = "0401499796",
                Address = "Jane Street",
                Gender = Gender.Male,
                Avatar = "123",
            };

            var context = new MoocDBContext(options);
            
                context.Users.AddRange(users);
                context.SaveChanges();
                var service = new UserService(context, _mapper, _mockWebHostEnvironment.Object);
                //action

                var updatedResult = await service.UpdateAsync(5, updatedUser);
                //Assert
                Assert.IsNotNull(updatedResult);
                Assert.That(updatedResult.UserName == updatedUser.UserName, "UserName is correct");
           
        }

    }
}