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

var builder = WebApplication.CreateBuilder(args);

// Add configuration

Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();

builder.Services.AddConfigureMediatR();

builder.Services.AddInfrastructure();
builder.Services.AddJwtAuthentication(builder.Configuration);

//builder
//    .Services
//    .AddControllers()
//    .AddApplicationPart(DemoCICD.Presentation.AssemblyReference.Assembly);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// Configure Options and SQL
builder.Services.ConfigureSqlServerRetryOptions(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlConfiguration();
builder.Services.AddRepositoryBaseConfiguration();
builder.Services.AddConfigureAutoMapper();

builder.Services.AddCarter();

// Configure Dapper
builder.Services.AddInfrastructureDapper();

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

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Add API Endpoint
//app.NewVersionedApi("products-minimal-show-on-swagger").MapProductApiV1().MapProductApiV2();

// Add API Endpoint with carter module

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();
//app.MapControllers();

//if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
    app.ConfigureSwagger();

try
{
    await app.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}

public partial class Program { }
