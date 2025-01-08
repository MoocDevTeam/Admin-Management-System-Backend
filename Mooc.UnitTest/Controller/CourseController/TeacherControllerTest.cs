
﻿using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MoocWebApi.Controllers;
using Mooc.Application.Contracts;
using Mooc.Application.Contracts.Course;
using MoocWebApi.Controllers.Course;
using Mooc.Application.Contracts.Course.Dto;
using Mooc.Application.Contracts.Dto;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
        public async Task GetByPageAsync_ReturnTeachers_WhenTeachersExist()
        {
            // Arrange
            var input = new FilterPagedResultRequestDto
            {
                PageIndex = 1,
                PageSize = 5,
                Sorting = "Expertise"
            };

            var teacherList = new List<TeacherReadDto>
            {


                new TeacherReadDto
                {
                    Id = 1,
                    Title = "Professor",
                    Department = "Mathematics",
                    Introduction = "Experienced in teaching advanced calculus",
                    Expertise = "Mathematics",
                    Office = "Room 101",
                    HiredDate = new DateTime(2010, 6, 1),
                    IsActive = true,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 2,
                    CreatedByUser = "Admin",
                    UpdatedByUser = "Admin"
                },
                new TeacherReadDto
                {
                    Id = 2,
                    Title = "Associate Professor",
                    Department = "Computer Science",
                    Introduction = "Specialized in Artificial Intelligence",
                    Expertise = "AI",
                    Office = "Room 102",
                    HiredDate = new DateTime(2015, 8, 20),
                    IsActive = true,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 2,
                    CreatedByUser = "Admin",
                    UpdatedByUser = "Admin"
                },
                new TeacherReadDto
                {
                    Id = 3,
                    Title = "Lecturer",
                    Department = "Physics",
                    Introduction = "Expert in quantum mechanics",
                    Expertise = "Physics",
                    Office = "Room 103",
                    HiredDate = new DateTime(2012, 4, 15),
                    IsActive = true,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 2,
                    CreatedByUser = "Admin",
                    UpdatedByUser = "Admin"
                },
                new TeacherReadDto
                {
                    Id = 4,
                    Title = "Professor",
                    Department = "Chemistry",
                    Introduction = "Researcher in organic chemistry",
                    Expertise = "Chemistry",
                    Office = "Room 104",
                    HiredDate = new DateTime(2011, 9, 10),
                    IsActive = true,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 2,
                    CreatedByUser = "Admin",
                    UpdatedByUser = "Admin"
                },
                new TeacherReadDto
                {
                    Id = 5,
                    Title = "Assistant Professor",
                    Department = "Biology",
                    Introduction = "Biologist with expertise in genetics",
                    Expertise = "Biology",
                    Office = "Room 105",
                    HiredDate = new DateTime(2016, 11, 30),
                    IsActive = true,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 2,
                    CreatedByUser = "Admin",
                    UpdatedByUser = "Admin"
                },
                new TeacherReadDto
                {
                    Id = 6,
                    Title = "Associate Professor",
                    Department = "Engineering",
                    Introduction = "Expert in mechanical engineering",
                    Expertise = "Engineering",
                    Office = "Room 106",
                    HiredDate = new DateTime(2013, 7, 22),
                    IsActive = true,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 2,
                    CreatedByUser = "Admin",
                    UpdatedByUser = "Admin"
                },
                new TeacherReadDto
                {
                    Id = 7,
                    Title = "Lecturer",
                    Department = "Economics",
                    Introduction = "Specializes in macroeconomics",
                    Expertise = "Economics",
                    Office = "Room 107",
                    HiredDate = new DateTime(2017, 1, 10),
                    IsActive = true,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 2,
                    CreatedByUser = "Admin",
                    UpdatedByUser = "Admin"
                },
                new TeacherReadDto
                {
                    Id = 8,
                    Title = "Professor",
                    Department = "Philosophy",
                    Introduction = "Expert in modern philosophy",
                    Expertise = "Philosophy",
                    Office = "Room 108",
                    HiredDate = new DateTime(2009, 5, 25),
                    IsActive = true,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 2,
                    CreatedByUser = "Admin",
                    UpdatedByUser = "Admin"
                },
                new TeacherReadDto
                {
                    Id = 9,
                    Title = "Assistant Professor",
                    Department = "Literature",
                    Introduction = "Specializes in American literature",
                    Expertise = "Literature",
                    Office = "Room 109",
                    HiredDate = new DateTime(2018, 8, 15),
                    IsActive = true,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 2,
                    CreatedByUser = "Admin",
                    UpdatedByUser = "Admin"
                },
                new TeacherReadDto
                {
                    Id = 10,
                    Title = "Lecturer",
                    Department = "Sociology",
                    Introduction = "Expert in social behavior studies",
                    Expertise = "Sociology",
                    Office = "Room 110",
                    HiredDate = new DateTime(2014, 12, 5),
                    IsActive = true,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 2,
                    CreatedByUser = "Admin",
                    UpdatedByUser = "Admin"
                }

        };

            var pagedResult = new PagedResultDto<TeacherReadDto>
            {
                Items = teacherList,
                Total = teacherList.Count
            };

            // Setup the mocked method to return the paged result
            _teacherServiceMock.Setup(service => service.GetListAsync(input))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _teacherController.GetByPageAsync(input);

            // Assert
            Assert.IsNotNull(result); // Ensure the result is not null
            Assert.AreEqual(teacherList.Count, result.Items.Count); // Ensure the items count is correct
            Assert.AreEqual(pagedResult.Total, result.Total); // Ensure the total count is correct
        }

    }
    
}
