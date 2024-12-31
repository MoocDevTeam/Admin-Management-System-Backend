using Mooc.Application.Contracts.Admin;
using Mooc.Application.Contracts.Dto;
using Mooc.Shared;
using MoocWebApi.Controllers.Admin;
using Moq;

namespace Mooc.UnitTest.Controller
{
    public class RoleControllerTest
    {
        private readonly RoleController _controller;
        private readonly Mock<IRoleService> _roleServiceMock;

        public RoleControllerTest()
        {
            _roleServiceMock = new Mock<IRoleService>();
            _controller = new RoleController(_roleServiceMock.Object);
        }

        [Test]
        public async Task GetByPageAsync_WhenPageRoleExit_ShouldReturnPageRoles()
        {
            // Arrange
            var input = new FilterPagedResultRequestDto();
            var roles = new List<RoleDto>
            {
                new RoleDto { Id = 1, RoleName ="admin1",   Description="full power" },
                new RoleDto { Id = 2, RoleName ="admin2",   Description="full power" },
                new RoleDto { Id = 3, RoleName ="teacher1", Description="limited power" },
                new RoleDto { Id = 4, RoleName ="teacher2", Description="limited power" },
                new RoleDto { Id = 5, RoleName ="teacher3", Description="limited power" },
            };
            var pagedResult = new PagedResultDto<RoleDto>
            {
                Items = roles,
                Total = 5,
            };
            _roleServiceMock.Setup(service => service.GetListAsync(input)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetByPageAsync(input);

            // Assert
            Assert.That(pagedResult, Is.EqualTo(result));
            Assert.That(pagedResult.Total, Is.EqualTo(result.Total));
            _roleServiceMock.Verify(s => s.GetListAsync(input), Times.Once);
        }

        [Test]
        public async Task Add_WhenRoleIsAdded_ShouldReturnTrue()
        {
            // Arrange
            var input = new CreateRoleDto()
            {
                Id = 1,
                RoleName = "teacher1",
                Description = "limited power"
            };
            var roleDto = new RoleDto()
            {
                Id = 1,
                RoleName = "teacher2",
                Description = "limited power"
            };
            _roleServiceMock.Setup(s => s.CreateAsync(input)).Returns(Task.FromResult(roleDto));

            // Act
            var roleDtoadded = await _roleServiceMock.Object.CreateAsync(input);
            var result = await _controller.Add(input);

            // Assert
            Assert.That(roleDto.RoleName, Is.EqualTo(roleDtoadded.RoleName));
            Assert.That(roleDto.Description, Is.EqualTo(roleDtoadded.Description));
            Assert.IsTrue(result);
            _roleServiceMock.Verify(s => s.CreateAsync(input), Times.AtLeastOnce);
        }
     
        [Test]
        public async Task Update_WhenRoleIsUpdated_ShouldReturnTrue()
        {
            //positive
            //arrange
            var input = new UpdateRoleDto()
            {
                Id = 1,
                RoleName = "teacher1",
                Description = "limited power"
            };
            var output = new RoleDto()
            {
                Id = 1,
                RoleName = "teacher1",
                Description = "limited power"
            };

            //act
            _roleServiceMock.Setup(s => s.UpdateAsync(input.Id, input)).Returns(Task.FromResult(output));
            var updatedRole = await _roleServiceMock.Object.UpdateAsync(input.Id, input);

            //Assert
            Assert.That(output.Description, Is.EqualTo(updatedRole.Description));
            Assert.That(output.RoleName, Is.EqualTo(updatedRole.RoleName));
            _roleServiceMock.Verify(s => s.UpdateAsync(input.Id, input), Times.Once);
        }
        [Test]
        public async Task Update_WhenRoleIsUpdated_ShouldReturnFalse()
        {
            //arrange
            var input = new UpdateRoleDto()
            {
                Id = 1,
                RoleName = "teacher1",
                Description = "limited power"

            };
            var output = new RoleDto()
            {
                Id = 1,
                RoleName = "teacher1",
                Description = "limited power"
            };
            _roleServiceMock.Setup(s => s.UpdateAsync(input.Id, input)).Returns(Task.FromResult(output));
            _controller.ModelState.AddModelError("Password", "Password is null ");
            //act
            var updatedRole = await _roleServiceMock.Object.UpdateAsync(input.Id, input);
            //Assert
            Assert.IsTrue(updatedRole.RoleName == "teacher1");
            _roleServiceMock.Verify(s => s.UpdateAsync(input.Id, input), Times.Once);
        }
        [Test]
        [TestCase(3, 7)]
        public async Task Delete_WhenRoleIsDeleted_ShouldReturnTrue(int id, int id2)
        {
            //valid Id case
            //Arrange
            var validId = id;
            var input = new FilterPagedResultRequestDto();
            var rolesBeforeDelete = new List<RoleDto>
            {
                new RoleDto { Id = 1, RoleName ="admin1",   Description="full power" },
                new RoleDto { Id = 2, RoleName ="admin2",   Description="full power" },
                new RoleDto { Id = 3, RoleName ="teacher1", Description="limited power" },
                new RoleDto { Id = 4, RoleName ="teacher2", Description="limited power" },
                new RoleDto { Id = 5, RoleName ="teacher3", Description="limited power" },
            };
            var rolesAfterDeleteValid = from listItem in rolesBeforeDelete where id != validId select listItem;
            var pagedResult = new PagedResultDto<RoleDto>
            {
                Items = rolesAfterDeleteValid as List<RoleDto>,
                Total = 4,
            };
            _roleServiceMock.Setup(service => service.DeleteAsync(validId)).Returns(Task.FromResult(pagedResult));
            _roleServiceMock.Setup(service => service.GetListAsync(input)).ReturnsAsync(pagedResult);//Act
            var ValidResult = _controller.Delete(validId);
            var listCountResult = await _controller.GetByPageAsync(input);

            //Assert
            Assert.That(listCountResult.Total, Is.EqualTo(4));
            Assert.IsTrue(ValidResult.Result);
            _roleServiceMock.Verify(s => s.DeleteAsync(validId), Times.Once);

            //invalid Id case
            //arrange
            var invalidId = id2;
            var input2 = new FilterPagedResultRequestDto();
            var rolesAfterDeleteInvalid = from listItem in rolesBeforeDelete where id != invalidId select listItem;
            var pagedResultInvalid = new PagedResultDto<RoleDto>
            {
                Items = rolesAfterDeleteValid as List<RoleDto>,
                Total = 5,
            };
            _roleServiceMock.Setup(service => service.DeleteAsync(invalidId)).Returns(Task.FromResult(pagedResultInvalid));
            _roleServiceMock.Setup(service => service.GetListAsync(input2)).ReturnsAsync(pagedResultInvalid);

            //Act
            var InvalidResult = _controller.Delete(validId);
            var listCountResult2 = await _controller.GetByPageAsync(input2);

            //Assert
            Assert.That(listCountResult2.Total, Is.EqualTo(5));
            _roleServiceMock.Verify(s => s.DeleteAsync(validId), Times.AtLeastOnce);
        }
    }
}
