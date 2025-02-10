using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Mooc.Application.Contracts.Course.Dto;
using Mooc.Application.Course;
using Mooc.Core.ExceptionHandling;
using Mooc.Core.Security;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.UnitTest.Service.CourseServiceTest;

public class CategoryServiceTest
{
    private readonly IMapper _mapper;

    public CategoryServiceTest()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Category, CategoryDto>()
                 .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));
            cfg.CreateMap<CreateCategoryDto, Category>();
            cfg.CreateMap<UpdateCategoryDto, Category>();

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
    public async Task GetByCategoryNameAsync_ShouldReturnCategoryWhenExists()
    {
        // Arrange
        var categories = new List<Category>
    {
        new Category { Id = 1, CategoryName = "Category1", Description = "Test Category"},
        new Category { Id = 2, CategoryName = "Category2", Description = "Another Test Category"}
    };
        var options = GetDbContextOptions("InMemoryCategoryDB_GetByName");

        using (var context = new MoocDBContext(options))
        {
            context.Category.AddRange(categories);
            context.SaveChanges();

            var service = new CategoryService(context, _mapper);

            // Act
            var result = await service.GetByCategoryNameAsync("Category1");

            // Assert
            Assert.NotNull(result);
            Assert.That(result.CategoryName, Is.EqualTo("Category1"));
            Assert.That(result.Description, Is.EqualTo("Test Category"));
        }
    }


    [Test]
    public async Task CreateAsync_ShouldCreateCategorySuccessfully()
    {
        // Arrange
        var mockCurrentUser = new Mock<ICurrentUser>();
        mockCurrentUser.Setup(c => c.Id).Returns(1); 

        var options = GetDbContextOptions("CreateCategoryTestDB");
        using (var context = new MoocDBContext(options))
        {
            var service = new CategoryService(context, _mapper)
            {
                CurrentUser = mockCurrentUser.Object 
            };

            var input = new CreateCategoryDto
            {
                CategoryName = "NewCategory",
                Description = "Description"
            };

            // Act
            var result = await service.CreateAsync(input);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.CategoryName, Is.EqualTo("NewCategory"));
            Assert.That(result.Description, Is.EqualTo("Description"));

            var categoryInDb = await context.Category.FirstOrDefaultAsync(c => c.CategoryName == "NewCategory");
            Assert.NotNull(categoryInDb);
            Assert.That(categoryInDb.CreatedByUserId, Is.EqualTo(1)); 
        }
    }


    [Test]
    public void CreateAsync_ShouldThrowExceptionWhenCategoryNameExists()
    {
        // Arrange
        var options = GetDbContextOptions("DuplicateCategoryTestDB");
        using (var context = new MoocDBContext(options))
        {
            context.Category.Add(new Category { Id = 1, CategoryName = "DuplicateCategory", Description = "Description" });
            context.SaveChanges();

            var service = new CategoryService(context, _mapper);
            var input = new CreateCategoryDto { CategoryName = "DuplicateCategory", Description = "Description" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await service.CreateAsync(input));
            Assert.That(ex.Message, Is.EqualTo("DuplicateCategory already exists"));
        }
    }
}


