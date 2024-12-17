using Autofac.Core;
using Mooc.Application.Admin;
using Mooc.Application.Contracts.Admin;
using Mooc.Application.Contracts.Dto;
using Mooc.Shared;
using MoocWebApi.Controllers.Admin;
using Moq;
using MoocWebApi.Controllers.Admin;


namespace Mooc.UnitTest
{
    public class Tests
    {

        private readonly UserController _controller;
        private readonly Mock<IUserService> _userServiceMock;

        public Tests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);

        }
        [SetUp]
        public void SetUp()
        {
        }
        [Test]
        public async Task GetByPageAsync_WhenPageUserexit_ShouldReturnPageUsers()
        {
            // Arrange
            var input = new FilterPagedResultRequestDto();
            var users = new List<UserDto>
            {
                new UserDto { Id = 1, UserName = "A1", Password="123", Age= 1, Email="abc@uow.edu.au", Phone="0401499796",
                Address = "Jane Street", Gender=Gender.Male, Avatar="123" },
                new UserDto { Id = 2, UserName = "A2", Password="123", Age= 1, Email="abc@uow.edu.au", Phone="0401499796",
                Address = "Jane Street", Gender=Gender.Male, Avatar="123" },
                new UserDto { Id = 3, UserName = "A3", Password="123", Age= 1, Email="abc@uow.edu.au", Phone="0401499796",
                Address = "Jane Street", Gender=Gender.Male, Avatar="123" },
                new UserDto { Id = 4, UserName = "A4", Password="123", Age= 1, Email="abc@uow.edu.au", Phone="0401499796",
                Address = "Jane Street", Gender=Gender.Male, Avatar="123" },
                new UserDto { Id = 5, UserName = "A5", Password="123", Age= 1, Email="abc@uow.edu.au", Phone="0401499796",
                Address = "Jane Street", Gender=Gender.Male, Avatar="123" },
            };
            var pagedResult = new PagedResultDto<UserDto>
            {
                Items = users,
                Total = 5,
            };
            _userServiceMock.Setup(service => service.GetListAsync(input)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetByPageAsync(input);

            // Assert
            Assert.That(result, Is.EqualTo(pagedResult));
            Assert.That(result.Total, Is.EqualTo(pagedResult.Total));
            _userServiceMock.Verify(s => s.GetListAsync(input), Times.Once);
        }

        [Test]
        public async Task Add_WhenUserIsAdded_ShouldReturnTrue()
        {
            // Arrange
            var input = new CreateUserDto()
            {   Id = 1,
                UserName = "A5",
                Password = "123",
                Age = 1,
                Email = "abc@uow.edu.au",
                Phone = "0401499796",
                Address = "Jane Street",
                Gender = Gender.Male,
                Avatar = "123",
                RoleIds = new List<long>() { 1, 2, 3 }
            };
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
            _userServiceMock.Setup(s => s.CreateAsync(input)).Returns(Task.FromResult(useDto));

            // Act
            var userDtoadded = await _userServiceMock.Object.CreateAsync(input);
            var result = await _controller.Add(input);


            // Assert
           
            Assert.That(useDto.Age, Is.EqualTo(userDtoadded.Age));
            Assert.That(useDto.UserName, Is.EqualTo(userDtoadded.UserName));
            Assert.That(useDto.Avatar, Is.EqualTo(userDtoadded.Avatar));
            Assert.That(useDto.Email, Is.EqualTo(userDtoadded.Email));
            Assert.That(useDto.Phone, Is.EqualTo(userDtoadded.Phone));
            Assert.That(useDto.Address, Is.EqualTo(userDtoadded.Address));
            Assert.That(useDto.Gender, Is.EqualTo(userDtoadded.Gender));
            Assert.IsTrue(result);
            _userServiceMock.Verify(s => s.CreateAsync(input), Times.Exactly(2));

        }
        [Test]
        public async Task Add_WhenUserIsAdded_ShouldReturnFalse()
        {
            //bad case
            //Arrange
            var input = new CreateUserDto()
            {
                UserName = "A5",
                Password = "123",
                Age = 1,
                Email = "abc@uow.edu.au",
                Phone = "0401499796",
                Address = "Jane Street",
                Avatar = "123",
                RoleIds = new List<long>() { 1, 2, 3 }
            };
            var inputIncomplete = new UserDto()
            {

                UserName = "A5",
                Password = "123",
                Age = 1,
                Email = "abc@uow.edu.au",
                Phone = "0401499796",
                Address = "Jane Street",
                Avatar = "123",
            };
            _userServiceMock.Setup(s => s.CreateAsync(input)).Returns(Task.FromResult(inputIncomplete));
            //Act
            _controller.ModelState.AddModelError("Gender", "Gender is a requried field");
            var badResponse = await _userServiceMock.Object.CreateAsync(input);
            var result = await _controller.Add(input);

            //Assert
            Assert.IsFalse(result);
            _userServiceMock.Verify(s => s.CreateAsync(input), Times.AtLeastOnce);
        }

        [Test]
        public async Task Update_WhenUserIsUpdated_ShouldReturnTrue()
        {
            //arrange
            var input = new UpdateUserDto()
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
                RoleIds = new List<long>() { 1, 2, 3 }
            };
            var output = new UserDto()
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

            //act
            _userServiceMock.Setup(s => s.UpdateAsync(input.Id, input)).Returns(Task.FromResult(output));
            var updatedUser = await _userServiceMock.Object.UpdateAsync(input.Id, input);
            var result = await _controller.Update(input);

            //Assert
            Assert.That(output.Age, Is.EqualTo(updatedUser.Age));
            Assert.That(output.UserName, Is.EqualTo(updatedUser.UserName));
            Assert.That(output.Avatar, Is.EqualTo(updatedUser.Avatar)); 
            Assert.That(output.Email, Is.EqualTo(updatedUser.Email));
            Assert.That(output.Phone, Is.EqualTo(updatedUser.Phone));
            Assert.That(output.Gender, Is.EqualTo(updatedUser.Gender));
            Assert.That(output.Address, Is.EqualTo(updatedUser.Address));
            Assert.IsTrue(result);
            _userServiceMock.Verify(s => s.UpdateAsync(input.Id, input), Times.Exactly(2));
        }

        [Test]
        public async Task Update_WhenUserIsUpdated_ShouldReturnFalse()
        {
            //arrange
            var input = new UpdateUserDto()
            {
                Id = 1,
                UserName = "A5",
                Password = "",
                Age = 1,
                Email = "abc@uow.edu.au",
                Phone = "0401499796",
                Address = "Jane Street",
                Gender = Gender.Male,
                Avatar = "123",
                RoleIds = new List<long>() { 1, 2, 3 }
            };
            var output = new UserDto()
            {
                Id = 1,
                UserName = "A5",
                Password = "",
                Age = 1,
                Email = "abc@uow.edu.au",
                Phone = "0401499796",
                Address = "Jane Street",
                Gender = Gender.Male,
                Avatar = "123",
            };
            _userServiceMock.Setup(s => s.UpdateAsync(input.Id, input)).Returns(Task.FromResult(output));
            _controller.ModelState.AddModelError("Password", "Password is null ");

            //act
            var updatedUser = await _userServiceMock.Object.UpdateAsync(input.Id, input);

            //Assert
            Assert.IsTrue(updatedUser.Password == "");
            _userServiceMock.Verify(s => s.UpdateAsync(input.Id, input), Times.Once);
        }

        [Test]
        [TestCase(3,7)]
        public async Task Delete_WhenUserIsDeleted_ShouldReturnTrue(int id, int id2)
        {
            //valid Id case
            //Arrange
            var validId = id;
            var input = new FilterPagedResultRequestDto();
            var usersBeforeDelete = new List<UserDto>
            {
                new UserDto { Id = 1, UserName = "A1", Password="123", Age= 1, Email="abc@uow.edu.au", Phone="0401499796",
                Address = "Jane Street", Gender=Gender.Male, Avatar="123" },
                new UserDto { Id = 2, UserName = "A2", Password="123", Age= 1, Email="abc@uow.edu.au", Phone="0401499796",
                Address = "Jane Street", Gender=Gender.Male, Avatar="123" },
                new UserDto { Id = 3, UserName = "A3", Password="123", Age= 1, Email="abc@uow.edu.au", Phone="0401499796",
                Address = "Jane Street", Gender=Gender.Male, Avatar="123" },
                new UserDto { Id = 4, UserName = "A4", Password="123", Age= 1, Email="abc@uow.edu.au", Phone="0401499796",
                Address = "Jane Street", Gender=Gender.Male, Avatar="123" },
                new UserDto { Id = 5, UserName = "A5", Password="123", Age= 1, Email="abc@uow.edu.au", Phone="0401499796",
                Address = "Jane Street", Gender=Gender.Male, Avatar="123" },
            };
            var usersAfterDelete = from listItem in usersBeforeDelete where id != validId select listItem;

            var pagedResult = new PagedResultDto<UserDto>
            {
                Items = usersAfterDelete as List<UserDto>,
                Total = 4,
            };
            _userServiceMock.Setup(service => service.DeleteAsync(validId)).Returns(Task.FromResult(pagedResult));
            _userServiceMock.Setup(service => service.GetListAsync(input)).ReturnsAsync(pagedResult);//Act

            var ValidResult = _controller.Delete(validId);
            var listCountResult = await _controller.GetByPageAsync(input);
            //Assert

            Assert.That(listCountResult.Total, Is.EqualTo(4));
            Assert.IsTrue(ValidResult.Result);
            _userServiceMock.Verify(s => s.DeleteAsync(validId), Times.Once);


            //invalid Id case
            //arrange
            var invalidId = id2;
            var input2 = new FilterPagedResultRequestDto();
            var usersAfterDeleteInvalid  = from listItem in usersBeforeDelete where id != invalidId select listItem;
            var pagedResultInvalid = new PagedResultDto<UserDto>
            {
                Items = usersAfterDelete as List<UserDto>,
                Total = 5,
            };
            _userServiceMock.Setup(service => service.DeleteAsync(invalidId)).Returns(Task.FromResult(pagedResultInvalid));
            _userServiceMock.Setup(service => service.GetListAsync(input2)).ReturnsAsync(pagedResultInvalid);
            //Act
            var InvalidResult = _controller.Delete(validId);
            var listCountResult2 = await _controller.GetByPageAsync(input2);
            //Assert
            Assert.That(listCountResult2.Total, Is.EqualTo(5));
            _userServiceMock.Verify(s => s.DeleteAsync(validId), Times.AtLeastOnce);
        }
     
    }
}