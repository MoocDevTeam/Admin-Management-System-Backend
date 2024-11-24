using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Mooc.Model.Option;
using System.Text;

namespace MoocWebApi.Init;

public static class JwtExtension
{
    /// <summary>
    /// 初始化JWT
    /// </summary>
    /// <param name="services"></param>
    /// <param name="jwtSettingOption"></param>
    public static void AddJwtMooc(this IServiceCollection services, JwtSettingOption jwtSettingOption)
    {

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettingOption.Issuer,
                    ValidAudience = jwtSettingOption.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettingOption.SecurityKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }


}
