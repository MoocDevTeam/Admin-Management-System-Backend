using Mooc.Application.Admin;
using Mooc.Application.Contracts.Admin;
using Mooc.Shared;
using MoocWebApi.Controllers.Admin;
using Moq;

namespace Mooc.UnitTest;

[TestFixture]
public class MoocUserControllerTest
{
    private readonly MoocUserController _moocUserController;
    private readonly Mock<IMoocUserService> _moocUserServiceMock;
    
    public MoocUserControllerTest()
    {
        _moocUserServiceMock = new Mock<IMoocUserService>();
        _moocUserController = new MoocUserController(_moocUserServiceMock.Object);
    }
    
    [SetUp]
    public void SetUp()
    {
    }

    [Test]
    public async Task GetUserByUserNameAsync_ShouldReturnUser()
    {
        //arrange
        var useDto = new UserDto()
        {
            Id = 1,
            UserName = "A5",
            Password = "123",
            Age = 1,
            Email = "abc@uow.edu.au",
            Phone = "0401499796",
            Address = "Jane Street",
            Gender = Gender.Male,
            Avatar = "123",
        };
        _moocUserServiceMock.Setup(s => s.GetByUserNameAsync("A5")).Returns(Task.FromResult(useDto));
        
        //act
        var userResult = await _moocUserController.GetMoocUserByUserName("A5");
        
        //Assert
        Assert.AreEqual(userResult.Id, useDto.Id);
    }
    
    
    //arrange
    //act
    //Assert
    
    
    
    
    
    
    
    
    
}