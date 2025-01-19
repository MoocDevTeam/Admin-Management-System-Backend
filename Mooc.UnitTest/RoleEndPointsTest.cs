using Mooc.Application.Contracts.Admin;
using Mooc.Application.Contracts.Dto;
using Mooc.Core.WrapperResult;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Mooc.UnitTest
{
    public class RoleEndPointsTest : BaseTest
    {
        [Order(1)]
        [Test, Sequential]
        public async Task TestGetByPageAsync([Values("role01", "role02")] string userName)
        {

            var resp = await this.Client.GetAsync("/api/role/GetByPage?PageIndex=1&PageSize=12&Filter=" + userName);
            Assert.IsNotNull(resp);
            Assert.That(HttpStatusCode.OK == resp.StatusCode, "The statu code is incorrect");

            var stringResult = await resp.Content.ReadAsStringAsync();
            Assert.IsNotNull(stringResult);

            //var serializeOptions = new JsonSerializerOptions
            //{
            //    ReferenceHandler = ReferenceHandler.IgnoreCycles,
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

            //};
            //var jsonResult = JsonSerializer.Deserialize<ApiResponseResult<PagedResultDto<RoleDto>>>(stringResult, serializeOptions);

            var jsonResult = Deserialize<ApiResponseResult<PagedResultDto<RoleDto>>>(stringResult);

            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.IsSuccess);

        }
        private async Task<List<long>> GetRoleIdbyRoleName(string roleName)
        {
            var resp = await this.Client.GetAsync("/api/role/GetByPage?PageIndex=1&PageSize=12&Filter=" + roleName);
            var stringResult = await resp.Content.ReadAsStringAsync();

            var serializeOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

            };
            var jsonResult = JsonSerializer.Deserialize<ApiResponseResult<PagedResultDto<RoleDto>>>(stringResult, serializeOptions);
            if (jsonResult.IsSuccess && jsonResult.Data != null)
            {
                return jsonResult.Data.Items.Select(x => x.Id).ToList();
            }

            return new List<long>();
        }

        [Order(2)]
        [Test, Sequential]
        public async Task TestAddAsync(
        [Values("role01", "role02")] string roleName,
        [Values("full power", "limited power")] string description)
        {
            CreateRoleDto role = new CreateRoleDto();
            role.RoleName = roleName;
            role.Description = description;
            var jsonContent = JsonContent.Create(role);

            var resp = await this.Client.PostAsync("/api/role/Add", jsonContent);
            Assert.IsNotNull(resp);
            Assert.That(HttpStatusCode.OK == resp.StatusCode, "The statu code is incorrect");

            var stringResult = await resp.Content.ReadAsStringAsync();
            Assert.IsNotNull(stringResult);
            var jsonResult = Deserialize<ApiResponseResult<bool>>(stringResult);

            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.IsSuccess);
        }

        [Order(3)]
        [Test, Sequential]
        public async Task TestUpdateAsync(
        [Values(2, 1)] long id,
        [Values("UpdatedRole1", "UpdatedRole2")] string roleName,
        [Values("full-power", "limited-power")] string description)
        {
            UpdateRoleDto role = new UpdateRoleDto()
            {
                Id = id,
                RoleName = roleName,
                Description = description,
            };

            var jsonContent = JsonContent.Create(role);
           

            var resp = await Client.PostAsync("/api/role/Update", jsonContent);
           
            Assert.IsNotNull(resp);

            Console.WriteLine($"Status Code: {resp?.StatusCode}");
            var stringResult = await resp.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {stringResult}");
            Assert.IsNotNull(stringResult);

            Assert.That(HttpStatusCode.OK == resp.StatusCode, "The status code is incorrect");

            var jsonResult = Deserialize<ApiResponseResult<bool>>(stringResult);

            Assert.IsNotNull(jsonResult, "Response deserialization failed");
            Assert.IsTrue(jsonResult.IsSuccess, "API did not succeed");
        }

        [Order(4)]
        [Test, Sequential]  
        public async Task TestDeleteAsync([Values("role1", "role02")] string roleName)
        {
            List<long> ids = await GetRoleIdbyRoleName(roleName);


            foreach (var id in ids)
            {
                var resp = await this.Client.DeleteAsync("/api/role/Delete/" + id);
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

}
