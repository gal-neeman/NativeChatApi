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

        builder.Services.AddOptionsServices(builder);
        builder.Services.AddJwt(builder);
        builder.Services.AddControllers();
        builder.Services.AddDbServices();
        builder.Services.AddUtilityServices();
        builder.Services.AddCorsPolicies();

        builder.Services.AddValidatorsFromAssemblyContaining<CredentialsValidator>();
        builder.Services.AddFluentValidationAutoValidation();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthorization();
        app.UseSerilogRequestLogging();

        app.MapControllers();
        app.Run();
    }
}
