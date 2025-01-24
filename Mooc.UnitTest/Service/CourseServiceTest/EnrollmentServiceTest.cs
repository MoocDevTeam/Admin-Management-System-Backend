using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mooc.Application.Contracts.Course;
using Mooc.Application.Course;
using Mooc.Core.Security;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;
using Mooc.Shared;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.UnitTest.Service.CourseServiceTest;

public class EnrollmentServiceTest
{
    private readonly IMapper _mapper;

    public EnrollmentServiceTest()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Enrollment, EnrollmentDto>();
            cfg.CreateMap<CreateEnrollmentDto, Enrollment>();
            cfg.CreateMap<UpdateEnrollmentDto, Enrollment>();
                
        });

        _mapper = config.CreateMapper();
    }

    private DbContextOptions<MoocDBContext> GetDbContextOptions(string dbName)
    {
        return new DbContextOptionsBuilder<MoocDBContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

    [Test]
    public async Task CreateAsync_ShouldCreateEnrollmentSuccessfully()
    {
        // Arrange
        var mockCurrentUser = new Mock<ICurrentUser>();
        mockCurrentUser.Setup(c => c.Id).Returns(1); 

        var options = GetDbContextOptions("CreateEnrollmentTestDB");
        using (var context = new MoocDBContext(options))
        {
            var service = new EnrollmentService(context, _mapper)
            {
                CurrentUser = mockCurrentUser.Object 
            };

            var input = new CreateEnrollmentDto
            {
                EnrollmentStatus = EnrollmentStatus.Open,
                EnrollStartDate = DateTime.UtcNow,
                EnrollEndDate = DateTime.UtcNow.AddMonths(1),
                CourseInstanceId = 1
            };

            // Act
            var result = await service.CreateAsync(input);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.EnrollmentStatus, Is.EqualTo(EnrollmentStatus.Open));
            Assert.That(result.EnrollStartDate, Is.EqualTo(input.EnrollStartDate).Within(TimeSpan.FromSeconds(1)));
            Assert.That(result.EnrollEndDate, Is.EqualTo(input.EnrollEndDate).Within(TimeSpan.FromSeconds(1)));

            var enrollmentInDb = await context.Enrollment.FirstOrDefaultAsync(e => e.Id == result.Id);
            Assert.NotNull(enrollmentInDb);
            
        }
    }


    [Test]
    public async Task GetAsync_ShouldReturnEnrollmentWhenExists()
    {
        // Arrange
        var options = GetDbContextOptions("GetEnrollmentTestDB");
        using (var context = new MoocDBContext(options))
        {
            var enrollment = new Enrollment
            {
                Id = 1,
                EnrollmentStatus = EnrollmentStatus.Open,
                EnrollStartDate = DateTime.Now.AddMonths(-1),
                EnrollEndDate = DateTime.Now,
                CourseInstanceId = 1,
                CreatedByUserId = 1,
                CreatedAt = DateTime.UtcNow
            };

            context.Enrollment.Add(enrollment);
            context.SaveChanges();

            var service = new EnrollmentService(context, _mapper);

            // Act
            var result = await service.GetAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.EnrollmentStatus, Is.EqualTo(EnrollmentStatus.Open));
        }
    }

    [Test]
    public async Task GetAsync_ShouldReturnNullWhenEnrollmentDoesNotExist()
    {
        // Arrange
        var options = GetDbContextOptions("GetEnrollmentNotExistTestDB");
        using (var context = new MoocDBContext(options))
        {
            var service = new EnrollmentService(context, _mapper);

            // Act
            var result = await service.GetAsync(99); 

            // Assert
            Assert.IsNull(result);
        }
    }
}

