using Microsoft.OpenApi.Models;
using Mooc.Application;
using System.Reflection;

namespace MoocWebApi.Init;


public static class SwaggerExtension
{

    /// <summary>
    /// add swagger to services
    /// </summary>
    /// <param name="services"></param>
    public static void AddSwaggerMooc(this IServiceCollection services)
    {
        //add swagger
        services.AddSwaggerGen(optinos =>
        {
            typeof(SwaggerGroup).GetEnumNames().ToList().ForEach(version =>
            {
                optinos.SwaggerDoc(version, new OpenApiInfo()
                {
                    Title = "Mooc Web Api",
                    Version = "V1.0",
                    Description = "Mooc WebApi The backend server provides data",
                    Contact = new OpenApiContact { Name = "Mooc Team", Url = new Uri("http://www.google.com") }
                });

            });

            //Reflection acquisition interface and method description
            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            optinos.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName), true);

            //use jwt
            optinos.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Please enter Bearer Token in the input box below to enable JWT authentication",
                Name = "Authorization", // Default name, cannot be modified
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            //Make Swagger comply with the JWT protocol
            optinos.AddSecurityRequirement(new OpenApiSecurityRequirement
           {
             {
             new OpenApiSecurityScheme
             {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
             },
            new List<string>()
            }
           });
        });
    }


    /// <summary>
    ///  Join routing and pipeline
    /// </summary>
    /// <param name="app"></param>
    public static void UseSwaggerMooc(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            typeof(SwaggerGroup).GetEnumNames().ToList().ForEach(versoin =>
            {
                options.SwaggerEndpoint($"/swagger/{versoin}/swagger.json", $" {versoin}");
            });
        });
    }
}