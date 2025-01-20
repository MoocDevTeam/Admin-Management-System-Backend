using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mooc.Application.Contracts.Admin;
using Mooc.Model.Entity;
using static Amazon.S3.Util.S3EventNotification;

namespace Mooc.Application.Admin
{
    public class MenuService : CrudService<Menu, MenuDto, MenuDto, long, FilterPagedResultRequestDto, CreateMenuDto, UpdateMenuDto>, IMenuService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;
        public MenuService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
        }

        // Get menu by id
        public async Task<MenuDto> GetAsync(long id)
        {
            if (!IsMenuExist(id))
            {
                throw new EntityNotFoundException($"Menu with ID {id} not found.");
            }

            var menu = await base.GetAsync(id);

            var allMenus = await GetAllMenusAsync();

            menu.Children = BuildMenuTree(allMenus, menu.Id);

            return menu;
        }

        // Get menus based on pageniation
        public async Task<PagedResultDto<MenuDto>> GetListAsync(FilterPagedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }

        // Create a new menu
        public virtual async Task<MenuDto> CreateAsync(CreateMenuDto input)
        {
            var entity = MapToEntity(input);
            GetDbSet().Add(entity);
            await this.McDBContext.SaveChangesAsync();
            return MapToGetOutputDto(entity);
        }

        // Update a existing menu
        public async Task<MenuDto> UpdateAsync(long id, UpdateMenuDto input)
        {
            if (!IsMenuExist(id))
            {
                throw new EntityNotFoundException($"Menu with ID {id} not found.");
            }

            return await base.UpdateAsync(id, input);
        }

        // Delete a menu
        public async Task DeleteAsync(long id)
        {
            if (!IsMenuExist(id))
            {
                throw new EntityNotFoundException($"Menu with ID {id} not found.");
            }

            if (IsAnyChildMenuExist(id))
            {
                throw new MoocValidationException($"Menu with ID {id} has child menus");
            }
            await base.DeleteAsync(id);
        }

        // Get menu tree
        public async Task<List<MenuDto>> GetMenuTreeAsync()
        {
            var allMenus = await GetAllMenusAsync();

            return BuildMenuTree(allMenus, null);
        }

        // Validate menu existince
        private bool IsMenuExist(long id)
        {
            return _dbContext.Menus.Any(menu => menu.Id == id);
        }

        // Validate child menu existince
        private bool IsAnyChildMenuExist(long id)
        {
            return _dbContext.Menus.Any(menu => menu.ParentId == id);
        }

        // Get all menus
        private async Task<List<MenuDto>> GetAllMenusAsync()
        {
           
            var input = new FilterPagedResultRequestDto
            {
                PageSize = int.MaxValue, 
                PageIndex = 1
            };

            var result = await GetListAsync(input);
            return result.Items.ToList();
        }

        // Build menu tree
        private List<MenuDto> BuildMenuTree(List<MenuDto> menus, long? parentId)
        {
            return menus
                .Where(menu => menu.ParentId == parentId)
                .Select(menu => new MenuDto
                {
                    Id = menu.Id,
                    Title = menu.Title,
                    ParentId = menu.ParentId,
                    MenuType = menu.MenuType,
                    Description = menu.Description,
                    OrderNum = menu.OrderNum,
                    Route = menu.Route,
                    ComponentPath = menu.ComponentPath,
                    Permission = menu.Permission,
                    Children = BuildMenuTree(menus, menu.Id)
                })
                .OrderBy(menu => menu.OrderNum)
            .ToList();
        }

        // Override MapToEntity method
        protected override Menu MapToEntity(CreateMenuDto createInput)
        {
            var entity = this.Mapper.Map<CreateMenuDto, Menu>(createInput);
            SetIdForLong(entity);
            SetCreatedAudit(entity);
            return entity;
        }

        protected override void SetCreatedAudit(Menu entity)
        {
            if (entity is BaseEntityWithAudit baseEntityWithAudit && !baseEntityWithAudit.CreatedByUserId.HasValue)
            {
                baseEntityWithAudit.CreatedByUserId = 1;
                baseEntityWithAudit.CreatedAt = DateTime.Now;
            }
        }
    }
}
