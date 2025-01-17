using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Mooc.Application.Contracts;
using Mooc.Application.Contracts.Course.Dto;
using Mooc.Application.Course;
using Mooc.Core.ExceptionHandling;
using Mooc.Model.DBContext;
using Moq;
using NUnit.Framework.Internal;

namespace Mooc.UnitTest.Service 
 {
  [TestFixture]
  public class SessionServiceTest 
  {
    private MoocDBContext _dbContext;
    private Mock<IMapper> _mapperMock;

    private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

    private Mock<ICreateService<ReadSessionDto, CreateSessionDto>> _baseCreateAsyncMock;
    private Mock<SessionService> _sessionServiceMock;
    private SessionService _sessionService;

    [SetUp]
    public void Setup()
    {
      // Use InMemory Database for testing
      var options = new DbContextOptionsBuilder<MoocDBContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;
      _dbContext = new MoocDBContext(options);

      // Mock AutoMapper
      _mapperMock = new Mock<IMapper>();

      //
      _sessionServiceMock = new Mock<SessionService>();

      // Mock base CreateAsync
      _baseCreateAsyncMock = new Mock<ICreateService<ReadSessionDto, CreateSessionDto>>();

      // Mock webhostEnvironment 
      _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
      _webHostEnvironmentMock.Setup(env => env.EnvironmentName).Returns("Development");

      // Initialize SessionService
      _sessionService = new SessionService(_dbContext, _mapperMock.Object, _webHostEnvironmentMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
      _dbContext.Database.EnsureDeleted();
      _dbContext.Dispose();
    }

    // test create function
    [Test]
    public async Task CreateAsync_InvalidCourseInstanceId_ThrowsException()
    {
      // Arrange
      var input = new CreateSessionDto
      {
        Title = "Test Session",
        Description = "Test Description",
        CourseInstanceId = 0, // invalid CourseInstanceId
        TargetOrder = null
      };
  
      // Act
      var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sessionService.CreateAsync(input));

      // Assert
      Assert.That(exception.Message, Is.EqualTo($"CourseInstance with ID {input.CourseInstanceId} not found. Please check the input."));
    }

    // [Test]
    // public async Task CreateAsync_BaseCreateAsyncIsCalledAndReturnsDto()
    // {
    //   // Arrange
    //   var input = new CreateSessionDto
    //   {
    //     Title = "Test Session",
    //     Description = "Test Description",
    //     CourseInstanceId = 1, // valid 
    //     TargetOrder = null
    //   };

    //   var expectedDto = new ReadSessionDto
    //   {
    //     Title = "Test Session",
    //     Description = "Test Description",
    //     CourseInstanceId = 1,
    //     Order = 1,
    //     CreatedByUserId = 1,
    //     CreatedAt = DateTime.Now,
    //   };

    //   // 设置 ValidateCourseInstanceAsync 返回成功，表示 CourseInstanceId 是有效的
    //   _baseCreateAsyncMock
    //       .Setup(service => service.CreateAsync(It.IsAny<CreateSessionDto>()))
    //       .ReturnsAsync(expectedDto);

    //   _sessionServiceMock
    //       .Setup(service => service.ValidateSessionIdAsync(It.IsAny<long>()))
    //       .Returns(Task.CompletedTask);

    //   // Act
    //   var result = await _sessionService.CreateAsync(input);

    //   // Assert:
    //   Assert.That(result.Title, Is.EqualTo(expectedDto.Title));
    //   Assert.That(result.Description, Is.EqualTo(expectedDto.Description));
    //   Assert.That(result.CourseInstanceId, Is.EqualTo(expectedDto.CourseInstanceId));
    //   Assert.That(result.Order, Is.EqualTo(expectedDto.Order));
    //   Assert.That(result.CreatedByUserId, Is.EqualTo(expectedDto.CreatedByUserId));
    //   _baseCreateAsyncMock.Verify(service => service.CreateAsync(It.IsAny<CreateSessionDto>()), Times.Once);
    // }

  }


}
