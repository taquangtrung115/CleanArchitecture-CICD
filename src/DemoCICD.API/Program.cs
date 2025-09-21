using DemoCICD.API.Middleware;
using DemoCICD.Application.DependencyInjection.Extensions;
using DemoCICD.Persistence.DependencyInjection.Options;
using Serilog;
using DemoCICD.Persistence.DependencyInjection.Extensions;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using DemoCICD.API.DependencyInjection.Extensions;
using DemoCICD.Infrastructure.Dapper.DependencyInjection.Extensions;
using DemoCICD.Presentation.APIs.Products;
using Carter;
using DemoCICD.Infrastructure.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using DemoCICD.Persistence;
using DemoCICD.Persistence.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Thêm cấu hình

Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration).WriteTo.Console()
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();

builder.Services.AddConfigureMediatR();

builder.Services.AddInfrastructure();
builder.Services.AddRedisCache(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

//builder
//    .Services
//    .AddControllers()
//    .AddApplicationPart(DemoCICD.Presentation.AssemblyReference.Assembly);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// Cấu hình Tùy chọn và SQL
builder.Services.ConfigureSqlServerRetryOptions(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSqlConfiguration();
builder.Services.AddRepositoryBaseConfiguration();
builder.Services.AddConfigureAutoMapper();

builder.Services.AddCarter();

// Cấu hình SignalR cho real-time chat
builder.Services.AddSignalR();

// Cấu hình Dapper
builder.Services.AddInfrastructureDapper();
//test webhook CICD 2
builder.Services
        .AddSwaggerGenNewtonsoftSupport()
        .AddFluentValidationRulesToSwagger()
        .AddEndpointsApiExplorer()
        .AddSwagger();

builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Enable CORS before authentication and authorization
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthorization();

app.MapCarter();
//app.MapControllers();

// Map SignalR ChatHub
app.MapHub<DemoCICD.API.Hubs.ChatHub>("/chathub");

//if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
    app.ConfigureSwagger();

// Seed sample data for development/demo
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        // Ensure database exists and apply any pending migrations
        await context.Database.MigrateAsync();
        
        // Seed identity data (actions, functions, roles, permissions, etc.)
        await IdentitySeeder.SeedIdentityDataAsync(context);
        
        // Seed additional positions
        await PositionSeeder.SeedAdditionalPositionsAsync(context);
        
        // Seed products
        await ProductSeeder.SeedProductsAsync(context);
        
        // Seed MotoGP data (seasons, teams, riders, bikes, races, videos, etc.)
        await MotoGPSeeder.SeedMotoGPDataAsync(context);
        
        // Seed news data
        await NewsSeeder.SeedNewsAsync(context);
        
        // Seed chat data
        await ChatSeeder.SeedChatDataAsync(context);
        
        // Seed chat room members
        await ChatRoomMemberSeeder.SeedChatRoomMembersAsync(context);
        
        Log.Information("Database seeding completed successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while seeding the database");
    }
}

try
{
    await app.RunAsync();
    Log.Information("Dừng lại một cách gọn gàng");
    Console.WriteLine("Dừng lại một cách gọn gàng");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Đã xảy ra ngoại lệ không được xử lý trong quá trình khởi động");
    Console.WriteLine("Đã xảy ra ngoại lệ không được xử lý trong quá trình khởi động: \n" + ex);
    await app.StopAsync();
}
finally
{
    Log.Information("Dừng lại một cách gọn gàng 2");
    Console.WriteLine("Dừng lại một cách gọn gàng 2");
    await app.DisposeAsync();
}


public partial class Program { }
