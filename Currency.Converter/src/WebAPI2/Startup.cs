using Currency.Converter.Application.Common.Interfaces;
using Currency.Converter.Infrastructure.Services;
using MediatR;
using Serilog;
namespace WebAPI2;

public static class Startup
{
    public static WebApplication InitializeApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog((contex, config) => {
            config.WriteTo.Console();
        });
        
        ConfigureServices(builder);
        var app = builder.Build();
        Configure(app);
        return app;
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var assembly = AppDomain.CurrentDomain.Load("Currency.Converter.Application");
        builder.Services.AddMediatR(assembly);
        
        builder.Services.AddSingleton<ICurrencyConverter, CurrencyConverterService>();
    }

    private static void Configure(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        
    }
}
