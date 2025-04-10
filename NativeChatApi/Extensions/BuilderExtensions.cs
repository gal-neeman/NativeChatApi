using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace NativeChat;

public static class BuilderExtensions
{
    public static void AddJwt(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var authSettings = builder.Configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>()!;

        SecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Secret));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // Validate issuer
                    ValidateAudience = true, // Validate audience
                    ValidateIssuerSigningKey = true, // Validate the secret key.
                    IssuerSigningKey = symmetricSecurityKey, // The secret key to validate.
                    ValidIssuer = authSettings.Issuer, // Retrieve valid issuer from configuration file
                    ValidAudiences = authSettings.Audience // Retrieve valid audiences from configuration file
                };
            });
    }

    public static void AddOptionsServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services
            .Configure<AuthSettings>(
                builder.Configuration.GetSection(nameof(AuthSettings)))
            .Configure<DatabaseSettings>(
                builder.Configuration.GetSection(nameof(DatabaseSettings)));
    }

    public static void AddDbServices(this IServiceCollection services)
    {
        services.AddDbContext<NativeChatContext>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IValidationService, ValidationService>();

        services.AddScoped<IUserDao, UserDao>();
    }

    public static void AddUtilityServices(this IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
        services.AddAutoMapper(typeof(MappingProfile));
    }
}
