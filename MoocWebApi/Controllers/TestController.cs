using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mooc.Core.Caching;
using Mooc.Core.ExceptionHandling;


namespace MoocWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IMoocCache _moocCache;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UploadFolderOption _uploadFolderOption;
        public TestController(ILogger<TestController> logger, IMoocCache moocCache, IWebHostEnvironment webHostEnvironment, IOptions< UploadFolderOption> uploadFolderOption)
        {
            this._logger = logger;
            this._moocCache = moocCache;
            this._webHostEnvironment = webHostEnvironment;
            this._uploadFolderOption = uploadFolderOption.Value;
        }

        [HttpGet]
        public string GetTest1()
        {

            this._logger.LogInformation("GetTest1 LogInformation");
            this._logger.LogDebug("GetTest1 LogDebug");
            this._logger.LogError("GetTest1 LogError");
            this._logger.LogWarning("GetTest1 LogWarning");

            var password = "123456";

            var hashPassword = BCryptUtil.HashPassword(password);

            var hh = "$2a$11$ZvdoiAApml18VQS57ozyMOncODCABm165TkqXuKZ6E0cAtB8NZ32K";
            var isSuccess = BCryptUtil.Verify(password, hashPassword);
            isSuccess = BCryptUtil.Verify(password, hh);
            var user = new UserDto() { Id = 1, UserName = "abc", Phone = "1234444444", Age = 50 };
            string key = "user_1";
            this._moocCache.Set<UserDto>(key, user, 30);
            var sss = this._moocCache.Get<UserDto>(key);
            Thread.Sleep(40000);
            var dd = this._moocCache.Get<UserDto>(key);

            return "test1";
        }
        [HttpGet]
        public string GetTest2()
        {
            return "test1";
        }


        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var savePath = Path.Combine(this._webHostEnvironment.ContentRootPath, _uploadFolderOption.RootFolder, _uploadFolderOption.AvatarFolder);

            var pathFile = Path.Combine(savePath, file.FileName);

            Directory.CreateDirectory(savePath);

            using (var stream = System.IO.File.Create(pathFile))
            {
                file.CopyTo(stream);
            }

            return Ok(new { file.FileName, file.ContentType, file.Length });
        }
    }
}
