using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mooc.Application.Contracts.Course;
using Mooc.Application.Contracts.Dto;
using Mooc.Model.Entity;
using MoocWebApi.Controllers.Course;
using Moq;
namespace Mooc.UnitTest.Controller.CourseController
{
    public class MoocCourseControllerTest
    {
        private MoocCourseController _controller;
        private Mock<IMoocCourseService> _courseServiceMock;
        [SetUp]
        public void SetUp()
        {
            _courseServiceMock = new Mock<IMoocCourseService>();
            _controller = new MoocCourseController(_courseServiceMock.Object);
        }
        [Test]
        public async Task CourseGetByPageAsync_ShouldReturnPagedMenus()
        {
            // Arrange
            var input = new FilterPagedResultRequestDto();
            var courses = new List<CourseDto>
            {
                new CourseDto { Id = 1, Title = "React",  CourseCode = "100", CoverImage = "xxx.png", Description = "React", CreatedByUserId = 1, UpdatedByUserId = 1,CategoryId=1,  },
                new CourseDto { Id = 2, Title = "<ASP.NET>",CourseCode = "101", CoverImage = "xxx.png", Description = ".Net", CreatedByUserId = 1, UpdatedByUserId = 1, CategoryId=1,  }
            };
            var pagedResult = new PagedResultDto<CourseDto>(courses.Count, courses);
            _courseServiceMock.Setup(service => service.GetListAsync(input)).ReturnsAsync(pagedResult);
            // Act
            var result = await _controller.GetByPageAsync(input);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Total);
            Assert.AreEqual(2, result.Items.Count);
            _courseServiceMock.Verify(service => service.GetListAsync(input), Times.Once);
        }
        [Test]
        public async Task Add_ShouldReturnTrueWhenMenuAddedSuccessfully()
        {
            // Arrange
            var input = new CreateCourseDto
            {
                Title = "Java",
                Description = "Java",
                CourseCode = "100",
                CategoryId = 1
            };
            var createdCourse = new CourseDto { Id = 1, Title = "Java" };
            _courseServiceMock.Setup(service => service.CreateAsync(input)).ReturnsAsync(createdCourse);
            // Act
            var result = await _controller.Add(input);
            // Assert
            Assert.IsTrue(result);
            _courseServiceMock.Verify(service => service.CreateAsync(input), Times.Once);
        }
        [Test]
        public async Task Update_ShouldReturnTrueWhenMenuUpdatedSuccessfully()
        {
            // Arrange
            var input = new UpdateCourseDto
            {
                Id = 1,
                Title = "Python",
                Description = "Updated Description",
            };
            var course = new CourseDto
            {
                Id = 1,
                Title = "Updated course",
                Description = "Updated Description",
            };
            _courseServiceMock.Setup(service => service.GetAsync(input.Id))
        .ReturnsAsync(course);
            _courseServiceMock.Setup(service => service.UpdateAsync(It.IsAny<long>(), It.IsAny<UpdateCourseDto>()))
                .Returns(Task.FromResult(course));
            // Act
            var result = await _controller.Update(input);
            // Assert
            Assert.IsTrue(result);
            _courseServiceMock.Verify(service => service.UpdateAsync(input.Id, input), Times.Once);
        }
        [Test]
        public async Task Delete_ShouldReturnTrueWhenMenuDeletedSuccessfully()
        {
            // Arrange
            var courseId = 1;
            var existingCourse = new CourseDto
            {
                Id = courseId,
                Title = "Sample Course",
                Description = "Sample Description",
            };
            _courseServiceMock.Setup(service => service.GetAsync(courseId))
                .ReturnsAsync(existingCourse);
            _courseServiceMock.Setup(service => service.DeleteAsync(courseId)).Returns(Task.CompletedTask);
            // Act
            var result = await _controller.Delete(courseId);
            // Assert
            Assert.IsTrue(result);
            _courseServiceMock.Verify(service => service.DeleteAsync(courseId), Times.Once);
        }
        [Test]
        public async Task GetByIdAsync_ShouldReturnCourseWhenExists()
        {
            // Arrange
            var courseId = 1L;
            var course = new CourseDto
            {
                Id = courseId,
                Title = "Course1",
                Description = "Description1",
            };

            _courseServiceMock.Setup(service => service.GetByIdAsync(courseId)).ReturnsAsync(course);

            // Act
            var result = await _controller.GetByIdAsync(courseId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(courseId, result.Id);
            Assert.AreEqual("Course1", result.Title);

            _courseServiceMock.Verify(service => service.GetByIdAsync(courseId), Times.Once);
        }

    }
}
