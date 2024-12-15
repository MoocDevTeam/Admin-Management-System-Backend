using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mooc.Application.Admin;
using Mooc.Application.Contracts.Admin;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;
using Mooc.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.UnitTest
{
    public class UnitTest3 : BaseTest
    {
        protected override IMapper CreateMap()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<CreateUserDto, User>();
                cfg.CreateMap<UpdateUserDto, User>();
            });
            return config.CreateMapper();
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
            var users = LoadUsersFromJson("./MockData/users.json");
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
            var service = new UserService(context, this.Mapper, this.MockWebHostEnvironment.Object);

            //action
            var result = await service.CreateAsync(newUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserName == newUser.UserName, "Username is correct");
            Assert.That(result.Phone == newUser.Phone, "Phone is correct");
            Assert.That(result.Email == newUser.Email, "Email is corrct");
            Assert.That(result.Age == newUser.Age, "Age is correct");
        }

    }
}
