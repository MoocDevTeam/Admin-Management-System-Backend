using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Mooc.Application.Admin;
using Mooc.Application.Contracts.Admin;
using Mooc.Application.Contracts.Dto;
using Mooc.Core.Caching;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;
using Mooc.Shared.Enum;
using Moq;

namespace Mooc.UnitTest
{
    public class RoleServiceTest
    {
        private readonly IMapper _mapper;
        private readonly IMock<IMoocCache> _moocCache;
        public RoleServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Role, RoleDto>();
                cfg.CreateMap<CreateRoleDto, Role>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

                cfg.CreateMap<UpdateRoleDto, Role>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());
            });
            _mapper = config.CreateMapper();

            _moocCache = new Mock<IMoocCache>();
        }

        private DbContextOptions<MoocDBContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<MoocDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Test]
        public async Task GetListAsync_ShouldReturnPagedResultWithTotalCount()
        {
            // Arrange
            var options = GetDbContextOptions("InMemoryRoleDB_Paged");
            using (var context = new MoocDBContext(options))
            {
                context.Roles.AddRange(new List<Role>
                {
                    new Role { Id = 1, RoleName = "Admin1", Description = "full power"},
                    new Role { Id = 2, RoleName = "Admin2", Description = "full power" },
                    new Role { Id = 3, RoleName = "Teacher", Description = "limited power"}
                });
                context.SaveChanges();

                var service = new RoleService(context, _mapper, _moocCache.Object);
                var filterInput = new FilterPagedResultRequestDto
                {
                    PageIndex = 1,
                    PageSize = 10
                };

                // Act
                var result = await service.GetListAsync(filterInput);

                // Assert
                Assert.NotNull(result);
                Assert.That(result.Total, Is.EqualTo(3));
                Assert.That(result.Items.Count, Is.EqualTo(3));
                Assert.That(result.Items[0].RoleName, Is.EqualTo("Admin1"));
                Assert.That(result.Items[1].RoleName, Is.EqualTo("Admin2"));
            }
        }

        [Test]
        public async Task CreateAsync_ShouldCreateRoleSuccessfully()
        {
            // Arrange
            var options = GetDbContextOptions("InMemoryRoleDB_Create");
            var createRoleDto = new CreateRoleDto
            {
                RoleName = "Admin",
                Description = "full power",

            };

            using (var context = new MoocDBContext(options))
            {
                var service = new RoleService(context, _mapper, _moocCache.Object);

                // Act
                var result = await service.CreateAsync(createRoleDto);

                // Assert
                Assert.NotNull(result);
                Assert.That(result.RoleName, Is.EqualTo(createRoleDto.RoleName));
                Assert.That(result.Description, Is.EqualTo(createRoleDto.Description));
            }
        }

        [Test]
        public async Task GetAsync_ShouldReturnRoleWhenExists()
        {
            // Arrange
            var options = GetDbContextOptions("InMemoryRoleDB_Get");
            using (var context = new MoocDBContext(options))
            {
                var role = new Role
                {
                    Id = 1,
                    RoleName = "Admin",
                    Description = "full power",

                };
                context.Roles.Add(role);
                context.SaveChanges();

                var service = new RoleService(context, _mapper, _moocCache.Object);

                // Act
                var result = await service.GetAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.That(result.RoleName, Is.EqualTo(role.RoleName));
                Assert.That(result.Description, Is.EqualTo(role.Description));
            }
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateRoleSuccessfully()
        {
            // Arrange
            var options = GetDbContextOptions("InMemoryRoleDB_Update");
            using (var context = new MoocDBContext(options))
            {
                var role = new Role
                {
                    Id = 1,
                    RoleName = "teacher",
                    Description = "limited power",

                };
                context.Roles.Add(role);
                context.SaveChanges();

                var service = new RoleService(context, _mapper, _moocCache.Object);
                var updateDto = new UpdateRoleDto
                {
                    RoleName = "Admin",
                    Description = "full power",

                };

                // Act
                var result = await service.UpdateAsync(1, updateDto);

                // Assert
                Assert.NotNull(result);
                Assert.That(result.RoleName, Is.EqualTo(updateDto.RoleName));
                Assert.That(result.Description, Is.EqualTo(updateDto.Description));

            }
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteRoleSuccessfully()
        {
            // Arrange
            var options = GetDbContextOptions("InMemorRoleDB_Delete");
            using (var context = new MoocDBContext(options))
            {
                var role = new Role
                {
                    Id = 1L,
                    RoleName = "admin",
                    Description = "Test Description",

                };
                context.Roles.Add(role);
                context.SaveChanges();

                var service = new RoleService(context, _mapper, _moocCache.Object);

                // Act
                await service.DeleteAsync(1L);

                // Assert
                var deletedRole = await context.Roles.FindAsync(1L);
                Assert.Null(deletedRole);
            }
        }
    }
}
