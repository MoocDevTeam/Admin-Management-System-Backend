using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders;
using Mooc.Application;
using Mooc.Core;
using Mooc.Model.DBContext;
using MoocWebApi.Filters;
using MoocWebApi.Init;
using MoocWebApi.Middlewares;
using NLog;
using NLog.Web;
using System.Text.Json;

namespace MoocWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Early init of NLog to allow startup and exception logging, before host is built
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");
            var defaultPolicy = "AllowAllOrigins";
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.WebHost.UseKestrel(options =>
                {
                    // Handle requests up to 50 MB
                    options.Limits.MaxRequestBodySize = 52428800;
                });


                //autofac
                builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
                builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
                {
                    containerBuilder.RegisterModule<AutofacModule>();
                });

                // 配置响应头以使用 UTF-8 编码  
                builder.Services.Configure<WebEncoderOptions>(options =>
                {
                    options.TextEncoderSettings = new System.Text.Encodings.Web.TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All);
                });

                // Add services to the container.
                builder.Services.AddAppCore(builder.Configuration);

                //Add Mooc Application services
                builder.Services.AddApplication();
                
                builder.Services.AddTransient<ExceptionHandlingMiddleware>();

                builder.Services.AddDbContext<MoocDBContext>(option =>
                {
                    var connectString = builder.Configuration["DataBase:ConnectionString"];
                    //option.UseSqlServer(connectString);
                    option.UseSqlite(connectString);
                });

                builder.Services.AddControllers(options =>
                {
                    options.Filters.Add<ValidateModelFilter>();
                    options.Filters.Add<UnifiedResultFilter>();
                    //Handle requests up to 50 MB
                    options.Filters.Add(new RequestFormLimitsAttribute() { BufferBodyLengthLimit = 52428800 });
                }).ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                }).AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                //builder.Services.AddEndpointsApiExplorer();

                builder.Services.AddSwaggerMooc();

                //Configure JwtSettingOption
                builder.Services.Configure<JwtSettingOption>(builder.Configuration.GetSection(JwtSettingOption.Section));
                var jwtSettingOption = builder.Configuration.GetSection(JwtSettingOption.Section).Get<JwtSettingOption>();
                builder.Services.AddJwtMooc(jwtSettingOption);

                //Configure UploadFolderOption
                builder.Services.Configure<UploadFolderOption>(builder.Configuration.GetSection(UploadFolderOption.Section));
                var uploadFolderOption = builder.Configuration.GetSection(UploadFolderOption.Section).Get<UploadFolderOption>();


                //CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(defaultPolicy,
                        builder =>
                        {
                            builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        });
                });

                var app = builder.Build();

                app.UseCors(defaultPolicy);
                app.UseMiddleware<ExceptionHandlingMiddleware>();
                // Configure the HTTP request pipeline.
                app.UseSwaggerMooc();
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwaggerMooc();
                }
                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

                var uploadFolder = Path.Combine(builder.Environment.ContentRootPath, uploadFolderOption.RootFolder);
                var staticFile = new StaticFileOptions();
                staticFile.FileProvider = new PhysicalFileProvider(uploadFolder);
                app.UseStaticFiles(staticFile);

                using (var socpe = app.Services.CreateScope())
                {
                    var dbSeedDataSevices = socpe.ServiceProvider.GetRequiredService<IEnumerable<IDBSeedDataService>>();
                    foreach (var dbSeedDataSevice in dbSeedDataSevices)
                    {
                        dbSeedDataSevice.InitAsync().GetAwaiter().GetResult();
                    }
                }

                app.Run();
            }
            catch (Exception exception)
            {
                // NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
    }
}
