
using Mooc.Application.Admin;
using Mooc.Application.Contracts.Admin;
using Mooc.Shared;
using Moq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;

namespace Mooc.UnitTest
{
    public class UserServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
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
                Phone = "0401499796",
                Address = "Jane Street",
                Gender = Gender.Male,
                Avatar = "123",
                RoleIds = new List<long>() { 1, 2, 3 }
            };

               var context = new MoocDBContext(options);
            
                context.Users.AddRange(users);
                context.SaveChanges();

                var service = new UserService(context, _mapper, _mockWebHostEnvironment.Object);
                //act
                var result = await service.CreateAsync(newUser);

                // Assert
                Assert.NotNull(result);
                Assert.That(result.UserName, Is.EqualTo(newUser.UserName));
                Assert.That(result.Phone, Is.EqualTo(newUser.Phone));
                Assert.That(result.Age, Is.EqualTo(newUser.Age));
                Assert.That(result.Email, Is.EqualTo(newUser.Email));
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
                Assert.That(result.UserName, Is.EqualTo("A1"));
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
                Phone = "0401499796",
                Address = "Jane Street",
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
                Assert.That(updatedResult.UserName, Is.EqualTo(updatedUser.UserName));
            }

        }

    }
}