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
            if(HttpStatusCode.OK == resp.StatusCode)
            {
                Assert.That(HttpStatusCode.OK == resp.StatusCode, "The status code is incorrect");
                var stringResult = await resp.Content.ReadAsStringAsync();
                Assert.IsNotNull(stringResult);

                var jsonResult = Deserialize<ApiResponseResult<MenuDto>>(stringResult);

                Assert.IsNotNull(jsonResult);
                Assert.IsTrue(jsonResult.IsSuccess);
                Assert.AreEqual(id, jsonResult.Data.Id);
            }
            else
            {
                Assert.That(HttpStatusCode.NotFound == resp.StatusCode, "The status code is incorrect");
            } 
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
            if(HttpStatusCode.OK == resp.StatusCode)
            {
                Assert.That(HttpStatusCode.OK == resp.StatusCode, "The status code is incorrect");

                var stringResult = await resp.Content.ReadAsStringAsync();
                Assert.IsNotNull(stringResult);

                var jsonResult = Deserialize<ApiResponseResult<bool>>(stringResult);

                Assert.IsNotNull(jsonResult);
                Assert.IsTrue(jsonResult.IsSuccess);
            }
            else
            {
                Assert.That(HttpStatusCode.NotFound == resp.StatusCode, "The status code is incorrect");
            }
            
        }

        [Test]
        public async Task TestDeleteAsync()
        {
            var ids = await GetMenuIds();
            foreach (var id in ids)
            {
                var getByIdResp = await Client.GetAsync($"/api/menu/GetById/{id}");
                Assert.IsNotNull(getByIdResp);
                Assert.That(HttpStatusCode.OK == getByIdResp.StatusCode);

                var menu = Deserialize<ApiResponseResult<MenuDto>>(await getByIdResp.Content.ReadAsStringAsync());
                Assert.IsNotNull(menu);
                Assert.IsTrue(menu.IsSuccess);

                if (menu.Data.Children != null && menu.Data.Children.Any())
                {
                    var deleteResp = await Client.DeleteAsync($"/api/menu/Delete/{id}");
                    Assert.IsNotNull(deleteResp);
                    Assert.That(HttpStatusCode.BadRequest == deleteResp.StatusCode);
                }
                else
                {
                    var deleteResp = await Client.DeleteAsync($"/api/menu/Delete/{id}");
                    Assert.IsNotNull(deleteResp);
                    Assert.That(HttpStatusCode.OK == deleteResp.StatusCode);
                }
            }
        }

        private async Task<List<long>> GetMenuIds()
        {
            var resp = await this.Client.GetAsync($"/api/menu/GetByPage?PageIndex=1&PageSize=100");
            Assert.IsNotNull(resp);
            Assert.That(HttpStatusCode.OK == resp.StatusCode, $"Expected status code 200, but got {resp.StatusCode}");

            var stringResult = await resp.Content.ReadAsStringAsync();
            var jsonResult = Deserialize<ApiResponseResult<PagedResultDto<MenuDto>>>(stringResult);

            return jsonResult.IsSuccess && jsonResult.Data != null
                ? jsonResult.Data.Items.Select(x => x.Id).ToList()
                : new List<long>();
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

        [Test]
        public async Task TestGetMenuTreeAsync()
        {
            var response = await Client.GetAsync("/api/menu/GetMenuTree");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(stringResult);

            var result = Deserialize<ApiResponseResult<List<MenuDto>>>(stringResult);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count > 0);
        }
    }
}
