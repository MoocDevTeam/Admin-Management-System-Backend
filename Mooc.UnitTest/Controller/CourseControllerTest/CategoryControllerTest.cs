using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mooc.Application.Contracts.Course.Dto;
using Mooc.Application.Contracts.Course;
using Mooc.Core.ExceptionHandling;
using MoocWebApi.Controllers.Course;

namespace Mooc.UnitTest.Controller.CourseController;

[TestFixture]
public class CategoryControllerTest
{
    private Mock<ICategoryService> _categoryServiceMock;
    private CategoryController _categoryController;

    [SetUp]
    public void SetUp()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _categoryController = new CategoryController(_categoryServiceMock.Object);
    }

    [Test]
    public async Task Add_ShouldReturnTrueWhenCategoryIsAddedSuccessfully()
    {
        // Arrange
        var input = new CreateCategoryDto { CategoryName = "Engineer", Description = "Test Description" };
        var createdCategory = new CategoryDto { Id = 1, CategoryName = "Engineer", Description = "Test Description" };

        _categoryServiceMock
            .Setup(service => service.CreateAsync(input))
            .ReturnsAsync(createdCategory);

        // Act
        var result = await _categoryController.Add(input);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(createdCategory.Id, result.Id);
        Assert.AreEqual(createdCategory.CategoryName, result.CategoryName);
        Assert.AreEqual(createdCategory.Description, result.Description);
        _categoryServiceMock.Verify(service => service.CreateAsync(input), Times.Once);
    }

    
    [Test]
    public async Task Add_WhenCategoryNameExists_ShouldThrowException()
    {
        // Arrange
        var input = new CreateCategoryDto
        {
            CategoryName = "Engineer", 
            Description = "Descriptions"
        };

        
        _categoryServiceMock
            .Setup(s => s.CreateAsync(It.Is<CreateCategoryDto>(c => c.CategoryName == "Engineer")))
            .ThrowsAsync(new Exception("Category name already exists"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _categoryController.Add(input));

       
        Assert.That(ex.Message, Is.EqualTo("Category name already exists"));

        
        _categoryServiceMock.Verify(s => s.CreateAsync(It.IsAny<CreateCategoryDto>()), Times.Once);
    }


    [Test]
    public async Task Delete_ShouldReturnTrueWhenCategoryIsDeletedSuccessfully()
    {
        // Arrange
        var categoryId = 1;

        _categoryServiceMock
            .Setup(service => service.DeleteAsync(categoryId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _categoryController.Delete(categoryId);

        // Assert
        Assert.IsTrue(result);
        _categoryServiceMock.Verify(service => service.DeleteAsync(categoryId), Times.Once);
    }



}

