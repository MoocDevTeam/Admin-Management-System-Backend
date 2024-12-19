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

        private List<User> users = new List<User>()
        {
            new User(){Id=1, UserName="admin",Age=30,Email="admin@demo.com",Address="Canberra",Gender= Gender.Male, Password=BCryptUtil.HashPassword("123456"), Created=DateTime.Now  },
            new User(){Id=2, UserName="test",Age=30,Email="test@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"), Created=DateTime.Now.AddMinutes(1)},

            new User(){Id=3, UserName="test1",Age=31,Email="test1@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"),Created=DateTime.Now.AddMinutes(2) },
            new User(){Id=4, UserName="test2",Age=31,Email="test2@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"),Created=DateTime.Now.AddMinutes(3) },
            new User(){Id=5, UserName="test3",Age=31,Email="test3@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"),Created=DateTime.Now.AddMinutes(4) },
            new User(){Id=6, UserName="test4",Age=31,Email="test4@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"),Created=DateTime.Now.AddMinutes(5) },
            new User(){Id=7, UserName="test5",Age=31,Email="test5@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"),Created=DateTime.Now.AddMinutes(6) },
            new User(){Id=8, UserName="test6",Age=31,Email="test6@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"),Created=DateTime.Now.AddMinutes(7) },
            new User(){Id=9, UserName="test7",Age=31,Email="test7@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"),Created=DateTime.Now.AddMinutes(8) },
            new User(){Id=10, UserName="test8",Age=31,Email="test8@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"),Created=DateTime.Now.AddMinutes(9) },
            new User(){Id=11, UserName="test9",Age=31,Email="test9@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"),Created=DateTime.Now.AddMinutes(10) },
            new User(){Id=12, UserName="test10",Age=31,Email="test10@demo.com",Address="Canberra",Gender= Gender.Male,Password=BCryptUtil.HashPassword("123456"),Created=DateTime.Now.AddMinutes(11) },
        };

        public async Task<bool> InitAsync()
        {
           
            if (!this._dbContext.Users.Any())
            {

                await this._dbContext.Users.AddRangeAsync(users);
                await this._dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}
