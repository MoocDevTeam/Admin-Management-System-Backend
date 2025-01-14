using Moq;
using Mooc.Application.Contracts.Course;
using MoocWebApi.Controllers.Course;
using Mooc.Application.Contracts.Dto;
using Mooc.Application.Contracts.Course.Dto;

namespace Mooc.UnitTest.Controller.CourseController
{
  public class SessionrControllerTest
  {
    private readonly Mock<ISessionService> _sessionServiceMock;
    private readonly SessionController _sessionController;

    public SessionrControllerTest()
    {
      _sessionServiceMock = new Mock<ISessionService>();
      _sessionController = new SessionController(_sessionServiceMock.Object);
    }

    // Test get paginated method
    [Test]
    // Valid Input PageIndex and PageSize
    public async Task GetByPageAsync_ShouldReturnsCorrectPagedResult_WhenPageIndexAndPageSizeAreValid() 
    {
      // Arrange
      var input = new FilterPagedResultRequestDto
      {
        PageIndex = 1,
        PageSize = 5,
      };

      var sessionList = new List<ReadSessionDto>
      {
          new ReadSessionDto { Id = 1, Title = "Session 1" },
          new ReadSessionDto { Id = 2, Title = "Session 2" },
          new ReadSessionDto { Id = 3, Title = "Session 3" },
          new ReadSessionDto { Id = 4, Title = "Session 4" },
          new ReadSessionDto { Id = 5, Title = "Session 5" }
      };

      var expectedResult = new PagedResultDto<ReadSessionDto>
      {
        Items = sessionList,
        Total = 5
      };

      // Mock the service to return the expectedResult
      _sessionServiceMock.Setup(service => service.GetListAsync(input))
        .ReturnsAsync(expectedResult);

      // Act
      var result = await _sessionController.GetByPageAsync(input);

      // Assert
      Assert.That(result, Is.Not.Null); 
      Assert.That(result.Total, Is.EqualTo(expectedResult.Total)); 
      Assert.That(result.Items.Count, Is.EqualTo(expectedResult.Items.Count));
      for (int i = 0; i < expectedResult.Items.Count; i++)
      {
        Assert.That(result.Items[i].Id, Is.EqualTo(expectedResult.Items[i].Id));
        Assert.That(result.Items[i].Title, Is.EqualTo(expectedResult.Items[i].Title));
      }

      _sessionServiceMock.Verify(s => s.GetListAsync(input), Times.Once());
    }

    // Illegal Input Test (PageIndex <= 0)
    [Test]
    public async Task GetByPageAsync_ShouldThrowArgumentException_WhenPageIndexIsLessThanOrEqualToZero() 
    {
      // Arrange
      var invalidInput = new FilterPagedResultRequestDto
      {
        PageIndex = 0,  // Invalid PageIndex
        PageSize = 5
      };

      ArgumentException exceptionResult = null;  
      PagedResultDto<ReadSessionDto> pagedResult = null;

      // Act 
      try
      {
        pagedResult = await _sessionController.GetByPageAsync(invalidInput);
      }
      catch (ArgumentException ex)
      {
        exceptionResult = ex;
      }

      // Assert
      Assert.That(pagedResult, Is.Null);
      Assert.That(exceptionResult, Is.Not.Null); // Make sure the exception is caught
      Assert.That("PageIndex must be greater than 0.", Is.EqualTo(exceptionResult.Message));
      _sessionServiceMock.Verify(s => s.GetListAsync(invalidInput), Times.Never());
    }

    // Illegal Input Test (PageSize <= 0)
    [Test]
    public async Task GetByPageAsync_ShouldThrowArgumentException_WhenPageSizeIsLessThanOrEqualToZero()
    {
      // Arrange
      var invalidInput = new FilterPagedResultRequestDto
      {
        PageIndex = 1, 
        PageSize = 0    // Invalid PageSize
      };

      ArgumentException exceptionResult = null;
      PagedResultDto<ReadSessionDto> pagedResult = null;

      // Act 
      try
      {
        pagedResult = await _sessionController.GetByPageAsync(invalidInput);
      }
      catch (ArgumentException ex)
      {
        exceptionResult = ex;
      }

      // Assert
      Assert.That(pagedResult, Is.Null);
      Assert.That(exceptionResult, Is.Not.Null);  
      Assert.That("PageSize must be greater than 0.", Is.EqualTo(exceptionResult.Message));
      _sessionServiceMock.Verify(s => s.GetListAsync(invalidInput), Times.Never());
    }

    //PageIndex returns null data if it exceeds the actual data.
    [Test]
    public async Task GetByPageAsync_ShouldReturnEmpty_WhenPageIndexIsOutOfRange()
    {
      // Arrange
      var input = new FilterPagedResultRequestDto
      {
        PageIndex = 2, // This is out of range since we only have 5 sessions
        PageSize = 5
      };

      var sessionList = new List<ReadSessionDto>
      {
          new ReadSessionDto { Id = 1, Title = "Session 1" },
          new ReadSessionDto { Id = 2, Title = "Session 2" },
          new ReadSessionDto { Id = 3, Title = "Session 3" },
          new ReadSessionDto { Id = 4, Title = "Session 4" },
          new ReadSessionDto { Id = 5, Title = "Session 5" }
      };

      var expectedResult = new PagedResultDto<ReadSessionDto>
      {
        Items = new List<ReadSessionDto>(), // Expecting an empty list because PageIndex is out of range
        Total = 5
      };

      // Mocking the service method to return the expected result
      _sessionServiceMock.Setup(s => s.GetListAsync(It.IsAny<FilterPagedResultRequestDto>()))
        .ReturnsAsync(expectedResult);

      // Act
      var result = await _sessionController.GetByPageAsync(input);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Items.Count, Is.EqualTo(expectedResult.Items.Count));
      Assert.That(result.Total, Is.EqualTo(expectedResult.Total));
      _sessionServiceMock.Verify(s => s.GetListAsync(input), Times.Once());
    }


    //Correct paging data slicing
    [Test]
    public async Task GetByPageAsync_ShouldReturnCorrectPagedResult_WhenPageIndexAndPageSizeAreValid()
    {
      // Arrange
      var input = new FilterPagedResultRequestDto
      {
        PageIndex = 1,
        PageSize = 2
      };

      var sessionList = new List<ReadSessionDto>
      {
          new ReadSessionDto { Id = 1, Title = "Session 1" },
          new ReadSessionDto { Id = 2, Title = "Session 2" },
          new ReadSessionDto { Id = 3, Title = "Session 3" },
          new ReadSessionDto { Id = 4, Title = "Session 4" },
          new ReadSessionDto { Id = 5, Title = "Session 5" }
      };

      var expectedResult = new PagedResultDto<ReadSessionDto>
      {
        Items = new List<ReadSessionDto>
        {
            new ReadSessionDto { Id = 1, Title = "Session 1" },
            new ReadSessionDto { Id = 2, Title = "Session 2" }
        },
        Total = 5
      };

      // Mocking the service method to return the expected result
      _sessionServiceMock.Setup(s => s.GetListAsync(It.IsAny<FilterPagedResultRequestDto>()))
        .ReturnsAsync(expectedResult);

      // Act
      var result = await _sessionController.GetByPageAsync(input);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Total, Is.EqualTo(expectedResult.Total)); 
      Assert.That(result.Items.Count, Is.EqualTo(expectedResult.Items.Count));
      for (int i = 0; i < expectedResult.Items.Count; i++)
      {
        Assert.That(result.Items[i].Id, Is.EqualTo(expectedResult.Items[i].Id));  // Verify Id
        Assert.That(result.Items[i].Title, Is.EqualTo(expectedResult.Items[i].Title));  // Verify Title
      }
      _sessionServiceMock.Verify(s => s.GetListAsync(input), Times.Once());
    }

    //When Filter is Provided
    [Test]
    public async Task GetByPageAsync_ShouldFilterData_WhenFilterIsProvided()
    {
      // Arrange
      var input = new FilterPagedResultRequestDto
      {
        PageIndex = 1,
        PageSize = 5,
        Filter = "Session 1"
      };

      var sessionList = new List<ReadSessionDto>
      {
          new ReadSessionDto { Id = 1, Title = "Session 1" },
          new ReadSessionDto { Id = 2, Title = "Session 2" },
          new ReadSessionDto { Id = 3, Title = "Session 3" },
      };

      var expectedResult = new PagedResultDto<ReadSessionDto>
      {
        Items = new List<ReadSessionDto>
        {
            new ReadSessionDto { Id = 1, Title = "Session 1" }  // Only "Session 1" should be returned
        },
        Total = 1  // Only one session matches the filter
      };

      // Mocking the service method to return the expected result
      _sessionServiceMock.Setup(s => s.GetListAsync(It.IsAny<FilterPagedResultRequestDto>()))
        .ReturnsAsync(expectedResult);

      // Act
      var result = await _sessionController.GetByPageAsync(input);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Items.Count, Is.EqualTo(expectedResult.Items.Count));  
      Assert.That(result.Total, Is.EqualTo(expectedResult.Total));  
      Assert.That(result.Items[0].Id, Is.EqualTo(expectedResult.Items[0].Id));  
      Assert.That(result.Items[0].Title, Is.EqualTo(expectedResult.Items[0].Title));  
      _sessionServiceMock.Verify(s => s.GetListAsync(input), Times.Once());
    }

    //No filtering when Filter is empty
    [Test]
    public async Task GetListAsync_ShouldNotFilterData_WhenFilterIsEmpty()
    {
      // Arrange
      var input = new FilterPagedResultRequestDto
      {
        PageIndex = 1,
        PageSize = 5,
        Filter = ""  // Empty filter
      };

      var sessionList = new List<ReadSessionDto>
      {
          new ReadSessionDto { Id = 1, Title = "Session 1" },
          new ReadSessionDto { Id = 2, Title = "Session 2" },
          new ReadSessionDto { Id = 3, Title = "Session 3" },
      };

      var expectedResult = new PagedResultDto<ReadSessionDto>
      {
        Items = sessionList,  // All the session items should be returned when the filter is empty
        Total = 3  // Total number of sessions
      };

      // Mocking the service method to return the expected result
      _sessionServiceMock.Setup(s => s.GetListAsync(It.IsAny<FilterPagedResultRequestDto>()))
        .ReturnsAsync(expectedResult);

      // Act
      var result = await _sessionController.GetByPageAsync(input);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Items.Count, Is.EqualTo(expectedResult.Items.Count));
      Assert.That(result.Total, Is.EqualTo(expectedResult.Total));

      for (int i = 0; i < expectedResult.Items.Count; i++)
      {
        Assert.That(result.Items[i].Id, Is.EqualTo(expectedResult.Items[i].Id));
        Assert.That(result.Items[i].Title, Is.EqualTo(expectedResult.Items[i].Title));
      }
    }
  }
}