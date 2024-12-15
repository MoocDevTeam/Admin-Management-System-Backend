using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Mooc.Application.Admin;
using Mooc.Application.Contracts.Admin;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;
using Moq;
using System.Linq.Expressions;

namespace Mooc.UnitTest
{
    public class TestMoocDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }


    public class UnitTestService
    {
        /// <summary>
        /// use in memory Database--test:GetByUserNameAsync
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test()
        {
            var options = new DbContextOptionsBuilder<MoocDBContext>().UseInMemoryDatabase("testdb")
                .Options;

            using var moocDBContext = new MoocDBContext(options);
            moocDBContext.Users.Add(new User() { Id = 1, UserName = "a", Password = "pwd", Age = 0, Gender = Shared.Gender.Male });
            await moocDBContext.SaveChangesAsync();

            var mockMapper = new Mock<IMapper>();
            //this.Mapper.Map<UserDto>(user);
            mockMapper.Setup(x => x.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto() { Id = 1, UserName = "a" });
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            IUserService userService = new UserService(moocDBContext, mockMapper.Object, mockWebHostEnvironment.Object);
            var userDto = await userService.GetByUserNameAsync("a");
            Assert.IsNotNull(userDto);
            Assert.That(userDto.Id == 1, "id is correct");
            Assert.That(userDto.UserName == "a", "username is correct");
        }

        /// <summary>
        /// use in memory Database
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestCreateUserAsync()
        {
            var options = new DbContextOptionsBuilder<MoocDBContext>().UseInMemoryDatabase("testdb")
                .Options;

            using var moocDBContext = new MoocDBContext(options);

            CreateUserDto userdto = new CreateUserDto();
            userdto.Id = 1;
            userdto.UserName = "a";
            userdto.Age = 30;
            userdto.Password = "password";
            userdto.Email = "lily@gmail.com";
            userdto.Address = "brisbane";
            var mockMapper = new Mock<IMapper>();
            //this.Mapper.Map<TEntity, TGetOutputDto>(entity)
            mockMapper.Setup(x => x.Map<User, UserDto>(It.IsAny<User>())).Returns(new UserDto()
            {
                Id = 1,
                UserName = "a",
                Age = 30,
                Password = "password",
                Email = "lily@gmail.com",
                Address = "brisbane"
            });
            // this.Mapper.Map<TCreateInput, TEntity>(createInput);
            mockMapper.Setup(x => x.Map<CreateUserDto,User>(userdto)).Returns(new User() { Id = 1, UserName = "a",Age=30,
                Password = "password", Email = "lily@gmail.com", Address= "brisbane"
            });

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            IUserService userService = new UserService(moocDBContext, mockMapper.Object, mockWebHostEnvironment.Object);
            var user = await userService.CreateAsync(userdto);
            Assert.IsNotNull(user);
            Assert.That(user.Email == "lily@gmail.com", "email is correct");
            Assert.That(user.UserName == "a", "username is correct");
        }

        /// <summary>
        /// mock seems not support labmdb expression?
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test1()
        {
            var mockDBset = new Mock<DbSet<User>>();


            var mockContext = new Mock<TestMoocDBContext>();

            var entities = new List<User>()
            {
                new User(){ Id=1,UserName="a"},
                new User(){ Id=2,UserName="b"},
            }.AsQueryable();
            mockDBset.As<IQueryable<User>>().Setup(m => m.Provider).Returns(entities.Provider);
            mockDBset.As<IQueryable<User>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockDBset.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            mockDBset.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());

            mockDBset.Setup(x => x.First()).Returns(entities.First());
            //mockDBset.Setup(x => x.FirstOrDefaultAsync(x => x.Id == 1, It.IsAny<CancellationToken>())).ReturnsAsync(entities.First());
            //var user = await mockDBset.Object.FirstOrDefaultAsync(x => x.Id == 1);
            var user = mockDBset.Object.First();

            Assert.IsNotNull(user);
            Assert.That(user.Id == 1, "id is correct");
            Assert.That(user.UserName == "a", "username is correct");
        }
    }
}
