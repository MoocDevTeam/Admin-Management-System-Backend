using Mooc.Application.Contracts.Admin;
using Mooc.Application.Contracts.Dto;
using Mooc.Core.WrapperResult;
using Mooc.Shared;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mooc.UnitTest
{
    public class UserEndPointsTest : BaseTest
    {
        [Order(3)]
        [Test]
        public async Task TestGetByPageAsync([Values("test01", "test02")] string userName)
        {

            var resp = await this.Client.GetAsync("/api/user/GetByPage?PageIndex=1&PageSize=12&Filter=" + userName);
            Assert.IsNotNull(resp);
            Assert.That(HttpStatusCode.OK == resp.StatusCode, "The statu code is incorrect");

            var stringResult = await resp.Content.ReadAsStringAsync();
            Assert.IsNotNull(stringResult);

            var serializeOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

            };
            var jsonResult = JsonSerializer.Deserialize<ApiResponseResult<PagedResultDto<UserDto>>>(stringResult, serializeOptions);

            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.IsSuccess);

        }

        [Order(2)]
        [Test, Sequential]
        public async Task TestDeleteAsync(
            [Values("test01", "test02")] string userName
            )
        {
            List<long> ids = await GetUserIdbyUserName(userName);

            foreach (var id in ids)
            {
                var resp = await this.Client.DeleteAsync("/api/user/Delete/" + id);
                Assert.IsNotNull(resp);
                Assert.That(HttpStatusCode.OK == resp.StatusCode, "The statu code is incorrect");

                var stringResult = await resp.Content.ReadAsStringAsync();
                Assert.IsNotNull(stringResult);

                var jsonResult = Deserialize<ApiResponseResult<bool>>(stringResult);

                Assert.IsNotNull(jsonResult);
                Assert.IsTrue(jsonResult.IsSuccess);
            }
        }

        private async Task<List<long>> GetUserIdbyUserName(string userName)
        {
            var resp = await this.Client.GetAsync("/api/user/GetByPage?PageIndex=1&PageSize=12&Filter=" + userName);
            var stringResult = await resp.Content.ReadAsStringAsync();


            var serializeOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

            };
            var jsonResult = JsonSerializer.Deserialize<ApiResponseResult<PagedResultDto<UserDto>>>(stringResult, serializeOptions);
            if (jsonResult.IsSuccess && jsonResult.Data != null)
            {
                return jsonResult.Data.Items.Select(x => x.Id).ToList();
            }

            return new List<long>();
        }

        [Order(1)]
        [Test, Sequential]
        public async Task TestAddAsync(
        [Values("test01")] string userName,
        [Values("123456")] string password,
        [Values(30)] int Age,
        [Values("abc@gmail.com")] string email,
        [Values("0421658272")] string phone,
        [Values("brisbane")] string address,
        [Values(Gender.Female)] Gender gender,
        [Values("test01")] string Avatar
        )
        {

            string uniqueUserName = $"{userName}_{Guid.NewGuid()}";
            string uniqueEmail = $"{Guid.NewGuid()}_{email}";
            CreateUserDto user = new CreateUserDto
            {
                UserName = uniqueUserName,
                Password = password,
                Age = Age,
                Email = uniqueEmail,
                        
                Gender = gender,
                Avatar = Avatar,
                // Include RoleIds if required by the API
                RoleIds = new List<long> { 1 } // Example role ID; ensure it exists in your test DB
            };

            var jsonContent = JsonContent.Create(user);

                var resp = await this.Client.PostAsync("/api/user/Add", jsonContent);
                Assert.IsNotNull(resp);
                Assert.That(HttpStatusCode.OK == resp.StatusCode, "The statu code is incorrect");

                var stringResult = await resp.Content.ReadAsStringAsync();
                Assert.IsNotNull(stringResult);
                var jsonResult = Deserialize<ApiResponseResult<bool>>(stringResult);

                Assert.IsNotNull(jsonResult);
                Assert.IsTrue(jsonResult.IsSuccess);
        }

    }

}
