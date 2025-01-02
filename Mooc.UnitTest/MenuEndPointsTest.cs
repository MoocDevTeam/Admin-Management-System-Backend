using Mooc.Application.Contracts.Admin;
using Mooc.Application.Contracts.Dto;
using Mooc.Core.WrapperResult;

using Mooc.Shared.Enum;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mooc.UnitTest.EndPoints.Admin
{
    public class MenuEndPointsTest : BaseTest
    {
        [Test]
        public async Task TestGetByPageAsync([Values("Menu1", "Menu2")] string filter)
        {
            var resp = await Client.GetAsync("/api/menu/GetByPage?PageIndex=1&PageSize=12&Filter=" + filter);
            Assert.IsNotNull(resp);
            Assert.That(HttpStatusCode.OK == resp.StatusCode, "The status code is incorrect");

            var stringResult = await resp.Content.ReadAsStringAsync();
            Assert.IsNotNull(stringResult);

            var serializeOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonResult = JsonSerializer.Deserialize<ApiResponseResult<PagedResultDto<MenuDto>>>(stringResult, serializeOptions);

            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.IsSuccess);
        }

        [Test]
        public async Task TestGetByIdAsync([Values(1, 2)] long id)
        {
            var resp = await Client.GetAsync("/api/menu/GetById/" + id);
            Assert.IsNotNull(resp);
            Assert.That(HttpStatusCode.OK == resp.StatusCode, "The status code is incorrect");

            var stringResult = await resp.Content.ReadAsStringAsync();
            Assert.IsNotNull(stringResult);

            var jsonResult = Deserialize<ApiResponseResult<MenuDto>>(stringResult);

            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.IsSuccess);
            Assert.AreEqual(id, jsonResult.Data.Id);
        }

        [Test, Sequential]
        public async Task TestAddAsync(
            [Values("Menu1", "Menu2")] string title,
            [Values("Description1", "Description2")] string description,
            [Values(MenuType.Dir, MenuType.Menu)] MenuType menuType)
        {
            CreateMenuDto menu = new CreateMenuDto
            {
                Title = title,
                Description = description,
                MenuType = menuType,
                ParentId = null
            };

            var jsonContent = JsonContent.Create(menu);

            var resp = await Client.PostAsync("/api/menu/Add", jsonContent);
            Assert.IsNotNull(resp);
            Assert.That(HttpStatusCode.OK == resp.StatusCode, "The status code is incorrect");

            var stringResult = await resp.Content.ReadAsStringAsync();
            Assert.IsNotNull(stringResult);

            var jsonResult = Deserialize<ApiResponseResult<bool>>(stringResult);

            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.IsSuccess);
        }

        [Test, Sequential]
        public async Task TestUpdateAsync(
            [Values(1, 2)] long id,
            [Values("Updated Menu1", "Updated Menu2")] string title,
            [Values("Updated Description1", "Updated Description2")] string description,
            [Values(MenuType.Menu, MenuType.Btn)] MenuType menuType)
        {
            UpdateMenuDto menu = new UpdateMenuDto
            {
                Id = id,
                Title = title,
                Description = description,
                MenuType = menuType
            };

            var jsonContent = JsonContent.Create(menu);

            var resp = await Client.PostAsync("/api/menu/Update", jsonContent);
            Assert.IsNotNull(resp);
            Assert.That(HttpStatusCode.OK == resp.StatusCode, "The status code is incorrect");

            var stringResult = await resp.Content.ReadAsStringAsync();
            Assert.IsNotNull(stringResult);

            var jsonResult = Deserialize<ApiResponseResult<bool>>(stringResult);

            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.IsSuccess);
        }

        [Test]
        public async Task TestDeleteAsync()
        {
            var deleteResp = await Client.DeleteAsync("/api/menu/Delete/4");

            Assert.IsNotNull(deleteResp);
            Assert.That(deleteResp.StatusCode == HttpStatusCode.OK, "The status code is incorrect");
        }

        private T Deserialize<T>(string json)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Deserialize<T>(json, serializeOptions);
        }
    }
}
