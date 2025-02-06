using Mooc.Application.Contracts.Course;
using Mooc.Shared;
using MoocWebApi.Controllers.Course;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.UnitTest.Controller.CourseControllerTest;

[TestFixture]
public class EnrollmentControllerTest
{
    private Mock<IEnrollmentService> _enrollmentServiceMock;
    private EnrollmentController _enrollmentController;

    [SetUp]
    public void SetUp()
    {
        _enrollmentServiceMock = new Mock<IEnrollmentService>();
        _enrollmentController = new EnrollmentController(_enrollmentServiceMock.Object);
    }

[Test]
    public async Task GetById_ShouldReturnEnrollmentWhenExists()
    {
        // Arrange
        var enrollmentId = 1;
        var enrollment = new EnrollmentDto
        {
            Id = enrollmentId,
            CourseInstanceId = 1001,
            EnrollmentStatus = EnrollmentStatus.Open
        };

        _enrollmentServiceMock
            .Setup(service => service.GetAsync(enrollmentId))
            .ReturnsAsync(enrollment);

        // Act
        var result = await _enrollmentController.GetbyId(enrollmentId);

        // Assert
        Assert.NotNull(result);
        Assert.That(result.Id, Is.EqualTo(enrollmentId));
        Assert.That(result.CourseInstanceId, Is.EqualTo(1001));
        Assert.That(result.EnrollmentStatus, Is.EqualTo(EnrollmentStatus.Open));
        _enrollmentServiceMock.Verify(service => service.GetAsync(enrollmentId), Times.Once);
    }

    [Test]
    public async Task Add_ShouldReturnTrueWhenEnrollmentIsAddedSuccessfully()
    {
        // Arrange
        var input = new CreateEnrollmentDto
        {
            CourseInstanceId = 1001,
            EnrollmentStatus = EnrollmentStatus.Open
        };

        var createdEnrollment = new EnrollmentDto
        {
            Id = 1,
            CourseInstanceId = 1001,
            EnrollmentStatus = EnrollmentStatus.Open
        };

        _enrollmentServiceMock
            .Setup(service => service.CreateAsync(input))
            .ReturnsAsync(createdEnrollment);

        // Act
        var result = await _enrollmentController.Add(input);

        // Assert
        Assert.IsTrue(result);
        _enrollmentServiceMock.Verify(service => service.CreateAsync(input), Times.Once);
    }

    [Test]
    public async Task Delete_ShouldReturnTrueWhenEnrollmentIsDeletedSuccessfully()
    {
        // Arrange
        var enrollmentId = 1;

        _enrollmentServiceMock
            .Setup(service => service.DeleteAsync(enrollmentId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _enrollmentController.Delete(enrollmentId);

        // Assert
        Assert.IsTrue(result);
        _enrollmentServiceMock.Verify(service => service.DeleteAsync(enrollmentId), Times.Once);
    }

    [Test]
    public async Task GetByEnrollmentId_ShouldSimulateNotFoundBehavior()
    {

        _enrollmentServiceMock
            .Setup(service => service.GetAsync(It.IsAny<long>()))
            .ReturnsAsync((EnrollmentDto?)null);

        // Act
        var result = await _enrollmentController.GetbyId(999);

        // Assert
        Assert.That(result, Is.Null, "Expected logical NotFound equivalent (null)");
        _enrollmentServiceMock.Verify(service => service.GetAsync(999), Times.Once);
    }
}








