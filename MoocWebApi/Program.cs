using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders;
using Microsoft.IdentityModel.Tokens;
using Mooc.Application;
using Mooc.Application.Contracts;
using Mooc.Application.Contracts.Course;
using Mooc.Application.Course;
using Mooc.Core;
using Mooc.Core.MoocAttribute;
using Mooc.Model.DBContext;
using MoocWebApi.Filters;
using MoocWebApi.Init;
using MoocWebApi.Middlewares;
using NLog;
using NLog.Web;
using System.Reflection;
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

                // Configure response headers to use UTF-8 encoding(non-English)  
                builder.Services.Configure<WebEncoderOptions>(options =>
                {
                    options.TextEncoderSettings = new System.Text.Encodings.Web.TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All);
                });

                // Add services to the container.
                builder.Services.AddAppCore(builder.Configuration);

                //Add Mooc Application services
                builder.Services.AddApplication();

                builder.Services.AddAutoMapper(typeof(Program));

                builder.Services.AddTransient<ExceptionHandlingMiddleware>();

                builder.Services.AddDbContext<MoocDBContext>(option =>
                {
                    var connectString = builder.Configuration["DataBase:ConnectionString"];
                    //option.UseSqlServer(connectString);
                    option.UseSqlite(connectString);
                });

                //Add JWT Authentication
                builder
                    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.SaveToken = true;

                        options.TokenValidationParameters =
                            new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidIssuer = builder.Configuration["JwtSetting:Issuer"],
                                ValidAudience = builder.Configuration["JwtSetting:Audience"],
                                IssuerSigningKey = new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        builder.Configuration["JwtSetting:SecurityKey"]
                                    )
                                ),
                            };
                    });

                builder
                    .Services.AddControllers(options =>
                    {
                        options.Filters.Add<ValidateModelFilter>();
                        options.Filters.Add<UnifiedResultFilter>();
                        //Handle requests up to 50 MB
                        options.Filters.Add(
                            new RequestFormLimitsAttribute() { BufferBodyLengthLimit = 52428800 }
                        );
                    })
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;
                    })
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy =
                            JsonNamingPolicy.CamelCase;
                    });

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                //builder.Services.AddEndpointsApiExplorer();

                builder.Services.AddSwaggerMooc();


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

                using (var socpe = app.Services.CreateScope())
                {
                    var dbSeedDataSevices = socpe.ServiceProvider.GetRequiredService<IEnumerable<IDBSeedDataService>>();

                    SortedDictionary<int, IDBSeedDataService> sdSeedData = new SortedDictionary<int, IDBSeedDataService>();

                    foreach (var dbSeedDataSevice in dbSeedDataSevices)
                    {
                        var orderAttri = dbSeedDataSevice.GetType().GetCustomAttribute<DBSeedDataOrderAttribute>();
                        if (orderAttri!=null)
                        {
                            sdSeedData.Add(orderAttri.Order, dbSeedDataSevice);
                        }
                    }
                    foreach (var item in sdSeedData)
                    {
                        item.Value.InitAsync().GetAwaiter().GetResult();
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
