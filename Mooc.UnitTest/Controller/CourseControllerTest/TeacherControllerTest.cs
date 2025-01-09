
﻿using Moq;
using Mooc.Application.Contracts.Course;
using MoocWebApi.Controllers.Course;
using Mooc.Application.Contracts.Course.Dto;
using Mooc.Application.Contracts.Dto;


namespace Mooc.UnitTest.Controller.CourseController
{
    public class TeacherControllerTest 
    {
        private readonly Mock<ITeacherService> _teacherServiceMock;
        private readonly TeacherController _teacherController;

        public TeacherControllerTest() 
        {
            _teacherServiceMock = new Mock<ITeacherService>();
            _teacherController = new TeacherController(_teacherServiceMock.Object);
        }
        //Test getPagedResult method
        [Test]
        public async Task GetByPageAsync_ReturnExpectedNumberAndTeachers_WhenTeachersExist()
        {
            // Arrange
            var input = new FilterPagedResultRequestDto
            {
                PageIndex = 1,
                PageSize = 5,
                Sorting = "Id",
                Filter="5"
            
            };

            var teacherList = new List<TeacherReadDto>
            {
                new TeacherReadDto { Id = 1, Title = "Doctor"},
                new TeacherReadDto { Id = 2, Title = "Professor"}
            };

            var pagedResult = new PagedResultDto<TeacherReadDto>
            {
                Items = teacherList,
                Total = 1
            };

            // Setup the mocked method to return the paged result
            _teacherServiceMock.Setup(service => service.GetListAsync(input))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _teacherController.GetByPageAsync(input);

            // Assert
            Assert.IsNotNull(result); // Ensure the result is not null
            Assert.AreEqual(2, result.Items.Count); // Ensure the items count is correct
            Assert.AreEqual(1, result.Total); // Ensure the total count is correct
            _teacherServiceMock.Verify( s  => s.GetListAsync(input), Times.Once());
        }

        [Test]
        public async Task GetByPageAsync_ReturnTotalNumberWithNoTeachers_WhenTeacherNotExists()
        {
            //arrange
            var input = new FilterPagedResultRequestDto
            {
                PageSize = 10,
                PageIndex = 1,
                Sorting = "Id",
                Filter = "10"
            };

            var teacherList = new List<TeacherReadDto>();
            var pagedResult = new PagedResultDto<TeacherReadDto>
            {
                Items = teacherList,
                Total = 0
            };
            //set up mocked method to return the paged result
            _teacherServiceMock.Setup(s => s.GetListAsync(input))
                .ReturnsAsync(pagedResult);
            //Act
            var result = await _teacherController.GetByPageAsync(input);

            //Assert
            Assert.IsNotNull (result);
            Assert.AreEqual (0, result.Items.Count); //should return an empty list
            Assert.AreEqual(0, result.Total); // no teachers found
            _teacherServiceMock.Verify(s => s.GetListAsync(input), Times.Once); //should be used once.

        }

        [Test]
        public async Task GetByPageAsync_ShouldReturnEmptyResult_WhenInputIsInvalid()
        {
            // Arrange
            var invalidInput = new FilterPagedResultRequestDto
            {
                PageIndex = -1,  // Invalid PageIndex
                PageSize = 5     // Valid PageSize
            };

            // Re-mock the service to prevent interference from other tests
            _teacherServiceMock.Reset();  // Reset any previous mocks or behaviors
            _teacherServiceMock.Setup(s => s.GetListAsync(It.IsAny<FilterPagedResultRequestDto>()))
                .ReturnsAsync(new PagedResultDto<TeacherReadDto> { Items = new List<TeacherReadDto>(), Total = 0 });

            // Act
            var result = await _teacherController.GetByPageAsync(invalidInput);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Items);  // Check if the list is empty
            Assert.AreEqual(0, result.Total);  // Ensure the total count is 0

            // Verify that GetListAsync was never called
            _teacherServiceMock.Verify(s => s.GetListAsync(It.IsAny<FilterPagedResultRequestDto>()), Times.Never);
        }
    }
}
