using Mooc.Application.Contracts;
using Mooc.Core.MoocAttribute;
using Mooc.Core.Utils;
using Mooc.Shared.Enum;

namespace Mooc.Application.Admin
{

    [DBSeedDataOrder(1)]
    public class AdminDBSeedDataService : IDBSeedDataService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;

        public AdminDBSeedDataService(MoocDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private List<User> users = new List<User>()
        {
            new User(){Id=1, UserName="admin", Password=BCryptUtil.HashPassword("123456"), Email="admin@demo.com", Age=30, Access=Access.admin, Gender=Gender.Male, Avatar="admin-avatar.png", CreatedAt=DateTime.Now, IsActive=true },
            new User(){Id=2, UserName="teacher1", Password=BCryptUtil.HashPassword("123456"), Email="teacher1@demo.com", Age=35, Access=Access.teacher, Gender=Gender.Male, Avatar="teacher1-avatar.png", CreatedAt=DateTime.Now.AddMinutes(1), IsActive=true },
            new User(){Id=3, UserName="teacher2", Password=BCryptUtil.HashPassword("123456"), Email="teacher2@demo.com", Age=32, Access=Access.teacher, Gender=Gender.Female, Avatar="teacher2-avatar.png", CreatedAt=DateTime.Now.AddMinutes(2), IsActive=true },
            new User(){Id=4, UserName="student1", Password=BCryptUtil.HashPassword("123456"), Email="student1@demo.com", Age=20, Access=Access.student, Gender=Gender.Male, Avatar="student1-avatar.png", CreatedAt=DateTime.Now.AddMinutes(3), IsActive=true },
            new User(){Id=5, UserName="student2", Password=BCryptUtil.HashPassword("123456"), Email="student2@demo.com", Age=21, Access=Access.student, Gender=Gender.Female, Avatar="student2-avatar.png", CreatedAt=DateTime.Now.AddMinutes(4), IsActive=true },
            new User(){Id=6, UserName="student3", Password=BCryptUtil.HashPassword("123456"), Email="student3@demo.com", Age=22, Access=Access.student, Gender=Gender.Male, Avatar="student3-avatar.png", CreatedAt=DateTime.Now.AddMinutes(5), IsActive=true }

        };
        private List<Role> roles = new List<Role>()
        {
            new Role(){Id=1, RoleName="admin1",   Description="with full permission to crud"},
            new Role(){Id=2, RoleName="admin2",   Description="with full permission to crud"},
            new Role(){Id=3, RoleName="admin3",   Description="with full permission to crud"},
            new Role(){Id=4, RoleName="teacher1", Description="with limited permission to crud"},
            new Role(){Id=5, RoleName="teacher2", Description="with limited permission to crud"},
            new Role(){Id=6, RoleName="teacher3", Description="with limited permission to crud"},
        };


        private List<Carousel> carousels = new List<Carousel>()
        {
            new Carousel(){Id=1, Title="Welcome Banner", ImageUrl="/images/welcome.jpg", RedirectUrl="/home", IsActive=true, UpdatedAt=DateTime.Now, Position=1, StartDate=DateTime.Now.AddDays(-7), EndDate=DateTime.Now.AddDays(30), CreatedByUserId=1, UpdatedByUserId=1 },
            new Carousel(){Id=2, Title="Sale Banner", ImageUrl="/images/sale.jpg", RedirectUrl="/sale", IsActive=true, UpdatedAt=DateTime.Now, Position=2, StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddDays(10), CreatedByUserId=2, UpdatedByUserId=2 }
        };

        private List<Menu> menus = new List<Menu>()
        {
            new Menu(){Id=1, Title="Rights Management", Description="Rights Management", MenuType= MenuType.Dir, OrderNum=0, Permission=PermissionConsts.PermissionManagement, CreatedByUserId=1,CreatedAt= DateTime.Now},
            new Menu(){Id=2, Title="User", Description="User", ParentId=1, MenuType=MenuType.Menu, OrderNum=1, Route="/user", ComponentPath="./pages/user/index.jsx", Permission=PermissionConsts.User.Default, CreatedByUserId=1,CreatedAt= DateTime.Now},
            new Menu(){Id=3, Title="Add", Description="Add",ParentId=2, MenuType= MenuType.Btn, OrderNum=1, Permission=PermissionConsts.User.Add, CreatedByUserId=1,CreatedAt= DateTime.Now},
            new Menu(){Id=4, Title="Update", Description="Update",ParentId=2, MenuType= MenuType.Btn, OrderNum=2, Permission=PermissionConsts.User.Update, CreatedByUserId=1,CreatedAt= DateTime.Now},
            new Menu(){Id=5, Title="Delete", Description="Delete",ParentId=2, MenuType= MenuType.Btn, OrderNum=3, Permission=PermissionConsts.User.Delete, CreatedByUserId=1,CreatedAt= DateTime.Now},
            new Menu(){Id=6, Title="Role", Description="Role",ParentId=1,MenuType= MenuType.Menu, OrderNum=2,Route="/role", ComponentPath="./pages/role/index.jsx", Permission=PermissionConsts.Role.Default,CreatedByUserId=1,CreatedAt= DateTime.Now },
            new Menu(){Id=7, Title="Add",Description="Add",ParentId=6, MenuType=MenuType.Btn, OrderNum=1, Permission=PermissionConsts.Role.Add, CreatedByUserId=1,CreatedAt= DateTime.Now},
            new Menu(){Id=8, Title="Update",Description="Update",ParentId=6, MenuType=MenuType.Btn, OrderNum=2, Permission=PermissionConsts.Role.Update, CreatedByUserId=1,CreatedAt= DateTime.Now},
            new Menu(){Id=9, Title="Delete",Description="Delete",ParentId=6, MenuType=MenuType.Btn, OrderNum=3, Permission=PermissionConsts.Role.Delete, CreatedByUserId=1,CreatedAt= DateTime.Now},
        };

        public async Task<bool> InitAsync()
        {
            if (!this._dbContext.Users.Any())
            {
                await this._dbContext.Users.AddRangeAsync(users);
                await this._dbContext.SaveChangesAsync();
            }
            if (!this._dbContext.Roles.Any())
            {
                await this._dbContext.Roles.AddRangeAsync(roles);
                await this._dbContext.SaveChangesAsync();
            }
            if (!this._dbContext.Carousels.Any())
            {
                await this._dbContext.Carousels.AddRangeAsync(carousels);
                await this._dbContext.SaveChangesAsync();
            }

            if (!this._dbContext.Menus.Any())
            {
                await this._dbContext.Menus.AddRangeAsync(menus);
                await this._dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}