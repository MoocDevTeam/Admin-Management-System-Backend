using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mooc.Application.Admin;
using Mooc.Application.Contracts.Admin;
using Mooc.Application.Contracts.Dto;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;
using Mooc.Shared.Enum;

namespace Mooc.UnitTest
{
    public class MenuServiceTest
    {
        private readonly IMapper _mapper;

        public MenuServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Menu, MenuDto>();
                cfg.CreateMap<CreateMenuDto, Menu>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

                cfg.CreateMap<UpdateMenuDto, Menu>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());
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
        public async Task GetListAsync_ShouldReturnPagedResultWithTotalCount()
        {
            // Arrange
            var options = GetDbContextOptions("InMemoryMenuDB_Paged");
            using (var context = new MoocDBContext(options))
            {
                context.Menus.AddRange(new List<Menu>
                {
                    new Menu { Id = 1, Title = "Menu1", Description = "Desc1", MenuType = MenuType.Dir, CreatedAt = DateTime.Now, CreatedByUserId = 1 },
                    new Menu { Id = 2, Title = "Menu2", Description = "Desc2", MenuType = MenuType.Menu, CreatedAt = DateTime.Now, CreatedByUserId = 1  },
                    new Menu { Id = 3, Title = "Menu3", Description = "Desc3", MenuType = MenuType.Btn, CreatedAt = DateTime.Now, CreatedByUserId = 1  }
                });
                context.SaveChanges();

                var service = new MenuService(context, _mapper);
                var filterInput = new FilterPagedResultRequestDto
                {
                    PageIndex = 1,
                    PageSize = 10 
                };

                // Act
                var result = await service.GetListAsync(filterInput);

                // Assert
                Assert.NotNull(result);
                Assert.AreEqual(3, result.Total);
                Assert.AreEqual(3, result.Items.Count); 
                Assert.AreEqual("Menu1", result.Items[0].Title); 
                Assert.AreEqual("Menu2", result.Items[1].Title); 
            }
        }

        [Test]
        public async Task CreateAsync_ShouldCreateMenuSuccessfully()
        {
            // Arrange
            var options = GetDbContextOptions("InMemoryMenuDB_Create");
            var createMenuDto = new CreateMenuDto
            {
                Title = "Test Menu",
                Permission = "View",
                MenuType = MenuType.Dir,
                Description = "Test Description",
                OrderNum = 1,
                Route = "/test-menu",
                ComponentPath = "/components/test-menu",
                ParentId = null,
            };

            using (var context = new MoocDBContext(options))
            {
                var service = new MenuService(context, _mapper);

                // Act
                var result = await service.CreateAsync(createMenuDto);

                // Assert
                Assert.NotNull(result);
                Assert.AreEqual(createMenuDto.Title, result.Title);
                Assert.AreEqual(createMenuDto.MenuType, result.MenuType);
            }
        }

        [Test]
        public async Task GetAsync_ShouldReturnMenuWhenExists()
        {
            // Arrange
            var options = GetDbContextOptions("InMemoryMenuDB_Get");
            using (var context = new MoocDBContext(options))
            {
                var menu = new Menu
                {
                    Id = 1,
                    Title = "Test Menu",
                    Description = "Test Description",
                    MenuType = MenuType.Menu,
                    CreatedAt = DateTime.Now,
                    CreatedByUserId = 1
                };
                context.Menus.Add(menu);
                context.SaveChanges();

                var service = new MenuService(context, _mapper);

                // Act
                var result = await service.GetAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.AreEqual(menu.Title, result.Title);
                Assert.AreEqual(menu.MenuType, result.MenuType);
            }
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateMenuSuccessfully()
        {
            // Arrange
            var options = GetDbContextOptions("InMemoryMenuDB_Update");
            using (var context = new MoocDBContext(options))
            {
                var menu = new Menu
                {
                    Id = 1L, 
                    Title = "Old Menu",
                    Description = "Old Description",
                    MenuType = MenuType.Btn,
                    CreatedAt = DateTime.Now,
                    CreatedByUserId = 1,
                    UpdatedAt = DateTime.Now,
                    UpdatedByUserId = 1
                };
                context.Menus.Add(menu);
                context.SaveChanges();

                var service = new MenuService(context, _mapper);
                var updateDto = new UpdateMenuDto
                {
                    Title = "Updated Menu",
                    Description = "Updated Description",
                    MenuType = MenuType.Dir,
                    OrderNum = 2
                };

                // Act
                var result = await service.UpdateAsync(1L, updateDto);

                // Assert
                Assert.NotNull(result);
                Assert.AreEqual(updateDto.Title, result.Title);
                Assert.AreEqual(updateDto.Description, result.Description);
                Assert.AreEqual(updateDto.MenuType, result.MenuType);
            }
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteMenuSuccessfully()
        {
            // Arrange
            var options = GetDbContextOptions("InMemoryMenuDB_Delete");
            using (var context = new MoocDBContext(options))
            {
                var menu = new Menu
                {
                    Id = 1L,
                    Title = "Test Menu",
                    Description = "Test Description",
                    MenuType = MenuType.Menu,
                    CreatedAt = DateTime.Now,
                    CreatedByUserId = 1
                };
                context.Menus.Add(menu);
                context.SaveChanges();

                var service = new MenuService(context, _mapper);

                // Act
                await service.DeleteAsync(1);

                // Assert
                var deletedMenu = await context.Menus.FindAsync(1L);
                Assert.Null(deletedMenu);
            }
        }

        [Test]
        public async Task GetMenuTreeAsync_ShouldReturnCorrectTreeStructure()
        {
            var options = GetDbContextOptions("InMemoryMenuDB_Tree");
            using (var context = new MoocDBContext(options))
            {
                context.Menus.AddRange(new List<Menu>
        {
            new Menu { Id = 1, Title = "Root Menu", ParentId = null, CreatedAt = DateTime.Now, CreatedByUserId = 1  },
            new Menu { Id = 2, Title = "Child Menu 1", ParentId = 1, CreatedAt = DateTime.Now, CreatedByUserId = 1  },
            new Menu { Id = 3, Title = "Child Menu 2", ParentId = 1, CreatedAt = DateTime.Now, CreatedByUserId = 1  },
            new Menu { Id = 4, Title = "Sub Child Menu", ParentId = 2, CreatedAt = DateTime.Now, CreatedByUserId = 1  }
        });
                context.SaveChanges();

                var service = new MenuService(context, _mapper);

                var result = await service.GetMenuTreeAsync();

                Assert.NotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("Root Menu", result[0].Title);

                var children = result[0].Children.ToList();
                Assert.AreEqual(2, children.Count);
                Assert.AreEqual("Child Menu 1", children[0].Title);
                Assert.AreEqual("Child Menu 2", children[1].Title);

                var subChildren = children[0].Children.ToList();
                Assert.AreEqual(1, subChildren.Count);
                Assert.AreEqual("Sub Child Menu", subChildren[0].Title);
            }
        }
    }
}
