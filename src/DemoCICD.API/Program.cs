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

// Thêm cấu hình

Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
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

// Cấu hình Dapper
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

// Thêm Điểm cuối API
//app.NewVersionedApi("products-minimal-show-on-swagger").MapProductApiV1().MapProductApiV2();

// Thêm Điểm cuối API với mô-đun carter

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthorization();

app.MapCarter();
//app.MapControllers();

//if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
    app.ConfigureSwagger();

try
{
    await app.RunAsync();
    Log.Information("Dừng lại một cách gọn gàng");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Đã xảy ra ngoại lệ không được xử lý trong quá trình khởi động");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}

public partial class Program { }
