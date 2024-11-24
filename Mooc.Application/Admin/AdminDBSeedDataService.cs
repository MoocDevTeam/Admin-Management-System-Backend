using Mooc.Application.Contracts;
using Mooc.Core.Utils;


namespace Mooc.Application.Admin
{
    public class AdminDBSeedDataService : IDBSeedDataService, ITransientDependency
    {

        private readonly MoocDBContext _dbContext;
        public AdminDBSeedDataService(MoocDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private List<Menu> menus = new List<Menu>()
        {
            new Menu(){Id=1, Title="Rights Management", MenuType= MenuType.Dir, OrderNum=0, Permission=PermissionConsts.PermissionManagement},
            new Menu(){Id=2, Title="User", ParentId=1, MenuType=MenuType.Menu, OrderNum=1, Route="/user", ComponentPath="./pages/user/index.jsx", Permission=PermissionConsts.User.Default},
            new Menu(){Id=3, Title="Add", ParentId=2, MenuType= MenuType.Btn, OrderNum=1, Permission=PermissionConsts.User.Add},
            new Menu(){Id=4, Title="Update", ParentId=2, MenuType= MenuType.Btn, OrderNum=2, Permission=PermissionConsts.User.Update},
            new Menu(){Id=5, Title="Delete", ParentId=2, MenuType= MenuType.Btn, OrderNum=3, Permission=PermissionConsts.User.Delete},
            new Menu(){Id=6, Title="Role", ParentId=1,MenuType= MenuType.Menu, OrderNum=2,Route="/role", ComponentPath="./pages/role/index.jsx", Permission=PermissionConsts.Role.Default },
            new Menu(){Id=7, Title="Add",ParentId=6, MenuType=MenuType.Btn, OrderNum=1, Permission=PermissionConsts.Role.Add},
            new Menu(){Id=8, Title="Update",ParentId=6, MenuType=MenuType.Btn, OrderNum=2, Permission=PermissionConsts.Role.Update},
            new Menu(){Id=9, Title="Delete",ParentId=6, MenuType=MenuType.Btn, OrderNum=3, Permission=PermissionConsts.Role.Delete},
        };


        private List<Role> roles = new List<Role>()
        {
            new Role(){Id=1, RoleName="Admin"},
            new Role(){Id=2, RoleName="Test"},
            new Role(){Id=3, RoleName="Test3"},
            new Role(){Id=4, RoleName="Test4"},
            new Role(){Id=5, RoleName="Test5"},
            new Role(){Id=6, RoleName="Test6"},
            new Role(){Id=7, RoleName="Test7"},
            new Role(){Id=8, RoleName="Test8"},
            new Role(){Id=9, RoleName="Test9"},
            new Role(){Id=10, RoleName="Test10"},
            new Role(){Id=11, RoleName="Test11"},
            new Role(){Id=12, RoleName="Test12"},
            new Role(){Id=13, RoleName="Test13"},
        };


        private List<User> users = new List<User>()
        {
            new User(){Id=1, UserName="admin",Age=30,Email="admin@demo.com",Address="Canberra",Gender= Gender.Male, Password=BCryptUtil.HashPassword("123456")  },
            new User(){Id=2, UserName="test",Age=30,Email="test@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456") },

            new User(){Id=3, UserName="test1",Age=31,Email="test1@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456") },
            new User(){Id=4, UserName="test2",Age=31,Email="test2@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456") },
            new User(){Id=5, UserName="test3",Age=31,Email="test3@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456") },
            new User(){Id=6, UserName="test4",Age=31,Email="test4@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456") },
            new User(){Id=7, UserName="test5",Age=31,Email="test5@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456") },
            new User(){Id=8, UserName="test6",Age=31,Email="test6@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456") },
            new User(){Id=9, UserName="test7",Age=31,Email="test7@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456") },
            new User(){Id=10, UserName="test8",Age=31,Email="test8@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456") },
            new User(){Id=11, UserName="test9",Age=31,Email="test9@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456") },
            new User(){Id=12, UserName="test10",Age=31,Email="test10@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456") },
        };

        private List<UserRole> userRoles = new List<UserRole>()
        {
            new UserRole(){Id=1, UserId=1, RoleId=1  },
            new UserRole(){Id=2, UserId=2, RoleId=2  }
        };


        private List<RoleMenu> roleMenus = new List<RoleMenu>()
        {
            new RoleMenu(){ Id=1,RoleId=1, MenuId=1 },
            new RoleMenu(){ Id=2,RoleId=1, MenuId=2 },
            new RoleMenu(){ Id=3,RoleId=1, MenuId=3 },
            new RoleMenu(){ Id=4,RoleId=1, MenuId=4 },
            new RoleMenu(){ Id=5,RoleId=1, MenuId=5 },
            new RoleMenu(){ Id=6,RoleId=1, MenuId=6 },
            new RoleMenu(){ Id=7,RoleId=1, MenuId=7 },
            new RoleMenu(){ Id=8,RoleId=1, MenuId=8 },
            new RoleMenu(){ Id=9,RoleId=1, MenuId=9 },
            new RoleMenu(){ Id=10,RoleId=1, MenuId=10 },
            new RoleMenu(){ Id=11,RoleId=1, MenuId=11 },
            new RoleMenu(){ Id=12,RoleId=1, MenuId=12 },
            new RoleMenu(){ Id=13,RoleId=1, MenuId=13 },
            new RoleMenu(){ Id=14,RoleId=1, MenuId=14 },
            new RoleMenu(){ Id=15,RoleId=1, MenuId=15 },
            new RoleMenu(){ Id=16,RoleId=1, MenuId=16 },
            new RoleMenu(){ Id=17,RoleId=1, MenuId=17 },
            new RoleMenu(){ Id=18,RoleId=1, MenuId=18 },

            new RoleMenu(){ Id=19,RoleId=2, MenuId=1 },
            new RoleMenu(){ Id=20,RoleId=2, MenuId=2 },
            new RoleMenu(){ Id=21,RoleId=2, MenuId=3 },
            new RoleMenu(){ Id=22,RoleId=2, MenuId=4 },
            new RoleMenu(){ Id=23,RoleId=2, MenuId=6 },
            new RoleMenu(){ Id=24,RoleId=2, MenuId=7 },

        };

        public async Task<bool> InitAsync()
        {
            if (!this._dbContext.Menus.Any())
            {
                await this._dbContext.Menus.AddRangeAsync(menus);
                await this._dbContext.SaveChangesAsync();
            }

            if (!this._dbContext.Roles.Any())
            {
                await this._dbContext.Roles.AddRangeAsync(roles);
                await this._dbContext.SaveChangesAsync();
            }

            if (!this._dbContext.RoleMenus.Any())
            {
                await this._dbContext.RoleMenus.AddRangeAsync(roleMenus);
                await this._dbContext.SaveChangesAsync();
            }


            if (!this._dbContext.Users.Any())
            {

                await this._dbContext.Users.AddRangeAsync(users);
                await this._dbContext.SaveChangesAsync();
            }

            if (!this._dbContext.UserRoles.Any())
            {

                await this._dbContext.UserRoles.AddRangeAsync(userRoles);
                await this._dbContext.SaveChangesAsync();
            }


            return true;
        }
    }
}
