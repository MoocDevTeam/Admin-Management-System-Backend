using Moq;
using Mooc.Application.Contracts.Course;
using MoocWebApi.Controllers.Course;

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
  }
}