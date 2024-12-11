﻿using Mooc.Application.Contracts.Admin;
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
        [Test]
        public async Task TestGetByPageAsync([Values("test01", "test02")] string userName)
        {
            using (var client = this.Factory.CreateClient())
            {
                var resp = await client.GetAsync("/api/user/GetByPage?PageIndex=1&PageSize=12&Filter=" + userName);
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
        }

        [Test, Sequential]
        public async Task TestDeleteAsync(
            [Values("test01", "test02")] string userName
            )
        {
           List<long> ids= await GetUserIdbyUserName(userName);

            using (var client = this.Factory.CreateClient())
            {

                foreach (var id in ids)
                {

                    var resp = await client.DeleteAsync("/api/user/Delete/"+id);
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

        private async Task<List<long>> GetUserIdbyUserName(string userName)
        {

            using (var client = this.Factory.CreateClient())
            {
                var resp = await client.GetAsync("/api/user/GetByPage?PageIndex=1&PageSize=12&Filter=" + userName);


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
            }

            return new List<long>();
        }


        [Test, Sequential]
        public async Task TestAddAsync(
        [Values("test01", "test02")] string userName,
        [Values("123456", "123456")] string password,
        [Values(30, 40)] int Age,
        [Values("abc@gmail.com", "bcd@gmail.com")] string email,
        [Values("0421658272", "0421658273")] string phone,
        [Values("brisbane", "Goldcoast")] string address,
        [Values(Gender.Female, Gender.Male)] Gender gender
        )
        {
            using (var client = this.Factory.CreateClient())
            {

                CreateUserDto user = new CreateUserDto();
                user.UserName = userName;
                user.Password = password;
                user.Age = Age;
                user.Email = email;
                user.Phone = phone;
                user.Address = address;
                user.Gender = gender;

                var jsonContent = JsonContent.Create(user);

                var resp = await client.PostAsync("/api/user/Add", jsonContent);
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