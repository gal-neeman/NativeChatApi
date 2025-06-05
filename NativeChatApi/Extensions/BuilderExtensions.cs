using System.Text;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenAI.Chat;

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
                builder.Configuration.GetSection(nameof(DatabaseSettings)))
            .Configure<OpenAISettings>(
                builder.Configuration.GetSection(nameof(OpenAISettings)))
            .Configure<UserCheckupSettings>(
                builder.Configuration.GetSection(nameof(UserCheckupSettings)));
    }

    public static void AddOpenAIServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // Load openai settings
        var settings = builder.Configuration.GetSection(nameof(OpenAISettings));
        string chatModel = settings.GetValue<string>("Model")!;
        string contextPrompt = settings.GetValue<string>("ContextPrompt")!;

        // Load api key
        var envVariables = DotEnv.Read(options: new DotEnvOptions(envFilePaths: new[] { @"C:\Projects\NativeChatApi\NativeChatApi\.env" }));
        var key = envVariables["apikey"];

        // Set up client
        ChatClient client = new(model: chatModel, apiKey: key);

        // Register singleton
        services.AddSingleton<ChatClient>(client);

        // Register custom services
        services.AddScoped<IChatService, ChatService>();
    }

    public static void AddDbServices(this IServiceCollection services)
    {
        services.AddDbContext<NativeChatContext>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<IBotService, BotService>();
        services.AddScoped<IMessagesService, MessageService>();

        services.AddScoped<IUserDao, UserDao>();
        services.AddScoped<IBotDao, BotDao>();
        services.AddScoped<IMessageDao, MessageDao>();
    }

    public static void AddUtilityServices(this IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
        services.AddAutoMapper(typeof(MappingProfile));
    }

    public static void AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }
}
