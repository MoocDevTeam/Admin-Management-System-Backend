using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Mooc.Application.Contracts.Course.Dto;
using Mooc.Application.Course;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;
using Mooc.Shared;
using Moq;
using Newtonsoft.Json;
using Sprache;

namespace Mooc.UnitTest.Service
{
    [TestFixture]
    public class TeacherServiceTest 
    {
        private MoocDBContext _dbContext;
        private Mock<IMapper> _mapperMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private TeacherService _teacherService;


        [SetUp]
        public void SetUp()
        {
            // Use InMemory Database for testing
            var options = new DbContextOptionsBuilder<MoocDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDb").Options;
                 _dbContext = new MoocDBContext(options);

            // Mock AutoMapper
            _mapperMock = new Mock<IMapper>();

            // Mock webhostEnvironment 
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _webHostEnvironmentMock.Setup(env => env.EnvironmentName).Returns("Development");

            // Initialize TeacherService
            _teacherService = new TeacherService(_dbContext, _mapperMock.Object, _webHostEnvironmentMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task CreateAsync_Should_Call_ValidateId_And_Return_TeacherReadDto()
        {
            // Arrange
            var userId = 1L;
            var createInput = new CreateOrUpdateTeacherDto { UserId = userId, Title = "Professor", Department = "Finance", Expertise = "xxx", HiredDate = DateTime.UtcNow, IsActive = false, Introduction="2222", Office="room222" };
            var teacherEntity = new Teacher { Id = 1, UserId = userId, Title="Professor", Department="Finance", Expertise="xxx", HiredDate=DateTime.UtcNow, IsActive=false, CreatedAt= DateTime.Now, CreatedByUserId= 1, Introduction="222", Office="Room222" };
            var teacherReadDto = new TeacherReadDto { Id = 1, Title = "Professor" };

            // Add a mock user to the in-memory database
            _dbContext.Users.Add(new User { Id = userId , Avatar ="XXX", Password ="OOO", UserName="Admin"});
            await _dbContext.SaveChangesAsync();

            // Mock AutoMapper behavior
            _mapperMock.Setup(mapper => mapper.Map<Teacher, TeacherReadDto>(It.IsAny<Teacher>())).Returns(teacherReadDto);
            _mapperMock.Setup(mapper => mapper.Map<CreateOrUpdateTeacherDto, Teacher>(It.IsAny<CreateOrUpdateTeacherDto>())).Returns(teacherEntity);

            // Act
            var result = await _teacherService.CreateAsync(createInput);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(teacherReadDto.Id, result.Id);
            Assert.AreEqual(teacherReadDto.Title, result.Title);

            var teacherInDb = await _dbContext.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            Assert.IsNotNull(teacherInDb);
            Assert.AreEqual(userId, teacherInDb.UserId);
            }
        [Test]
        public async Task CreateAsync_Should_ThrowArgumentNullException_When_InputIsNull()
        {
            // Arrange
            CreateOrUpdateTeacherDto input = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _teacherService.CreateAsync(input));
        }
    }



}