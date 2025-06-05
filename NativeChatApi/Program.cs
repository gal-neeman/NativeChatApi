using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;

namespace NativeChat;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Setup Serilog Logger
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration).CreateLogger();

        builder.Host.UseSerilog();

        // Add global filters
        builder.Services.AddMvc(options => {
            options.Filters.Add<GlobalErrorHandler>();
            options.Filters.Add<ExtractUserIdFilter>();
        });

        builder.Services.AddHostedService<UserCheckupService>();

        builder.Services.AddOptionsServices(builder);
        builder.Services.AddOpenAIServices(builder);
        builder.Services.AddJwt(builder);
        builder.Services.AddControllers();
        builder.Services.AddDbServices();
        builder.Services.AddUtilityServices();
        builder.Services.AddCorsPolicies();

        builder.Services.AddValidatorsFromAssemblyContaining<CredentialsValidator>();
        builder.Services.AddFluentValidationAutoValidation();

        var app = builder.Build();

        var webSocketOptions = new WebSocketOptions { };
        webSocketOptions.AllowedOrigins.Add("https://localhost.com");

        // Configure the HTTP request pipeline.
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseWebSockets();
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();

        app.MapControllers();
        app.Run();
    }
}
