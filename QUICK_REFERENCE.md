# Hướng Dẫn Nhanh - Quick Reference Guide
## Clean Architecture MotoGP System

### 🚀 Khởi Động Nhanh (Quick Start)

#### 1. Clone và Build
```bash
git clone https://github.com/taquangtrung115/CleanArchitecture-CICD.git
cd CleanArchitecture-CICD
dotnet restore
dotnet build
```

#### 2. Chạy Ứng Dụng
```bash
# API Project
cd src/DemoCICD.API
dotnet run

# Hoặc với profile cụ thể
dotnet run --environment Development
```

#### 3. Database Migration
```bash
# Tạo migration mới
dotnet ef migrations add MigrationName --project src/DemoCICD.Persistence --startup-project src/DemoCICD.API

# Áp dụng migration
dotnet ef database update --project src/DemoCICD.Persistence --startup-project src/DemoCICD.API
```

### 📁 Cấu Trúc Dự Án (Project Structure)

```
src/
├── DemoCICD.API/               # Web API Entry Point
├── DemoCICD.Application/       # Use Cases (CQRS)
├── DemoCICD.Contract/          # Contracts & DTOs
├── DemoCICD.Domain/            # Domain Logic
├── DemoCICD.Infrastructure/    # External Services
├── DemoCICD.Infrastructure.Dapper/ # Dapper Queries
├── DemoCICD.Persistence/       # Database & Repositories
└── DemoCICD.Presentation/      # Controllers & APIs

test/
├── DemoCICD.Architecture.Tests/
├── DemoCICD.Identity.Tests/
└── DemoCICD.UnitTests/
```

### 🏗️ Clean Architecture Layers

#### Domain Layer (Core)
```csharp
// Domain/Entities/MotoGP/
public class Rider : Entity<Guid>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int RacingNumber { get; private set; }
    public string Nationality { get; private set; }
    public Team? Team { get; private set; }
    
    // Rich domain methods
    public void TransferToTeam(Team newTeam) { /* logic */ }
    public void Retire() { /* logic */ }
}
```

#### Application Layer (Use Cases)
```csharp
// Application/UserCases/V1/Commands/MotoGP/Rider/
public sealed class CreateRiderCommandHandler 
    : ICommandHandler<CreateRiderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateRiderCommand command, 
        CancellationToken cancellationToken)
    {
        // Business logic implementation
    }
}
```

#### Infrastructure Layer (External)
```csharp
// Infrastructure/Authentication/
public class JwtTokenService : IJwtTokenService
{
    public string GenerateAccessToken(AppUser user) { /* JWT logic */ }
    public string GenerateRefreshToken() { /* Refresh logic */ }
}
```

### 🔧 Configuration Files

#### appsettings.json Structure
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;",
    "Redis": "localhost:6379"
  },
  "JwtSettings": {
    "Issuer": "DemoCICD",
    "Audience": "DemoCICD-Users",
    "SecretKey": "your-secret-key"
  },
  "RedisOptions": {
    "ConnectionString": "localhost:6379",
    "InstanceName": "DemoCICD"
  }
}
```

### 📊 API Endpoints

#### Authentication
```
POST /api/v1/auth/register
POST /api/v1/auth/login
POST /api/v1/auth/refresh-token
POST /api/v1/auth/logout
```

#### MotoGP Management
```
# Riders
GET    /api/v1/riders
GET    /api/v1/riders/{id}
POST   /api/v1/riders
PUT    /api/v1/riders/{id}
DELETE /api/v1/riders/{id}

# Teams
GET    /api/v1/teams
GET    /api/v1/teams/{id}
POST   /api/v1/teams
PUT    /api/v1/teams/{id}

# Specific Actions
POST   /api/v1/riders/{id}/transfer-to-team
POST   /api/v1/riders/{id}/retire
POST   /api/v1/riders/{id}/comeback
```

### 🧪 Testing Commands

#### Unit Tests
```bash
# Chạy tất cả tests
dotnet test

# Chạy với coverage
dotnet test --collect:"XPlat Code Coverage"

# Chạy specific test project
dotnet test test/DemoCICD.UnitTests/
```

#### Architecture Tests
```bash
# Kiểm tra architecture compliance
dotnet test test/DemoCICD.Architecture.Tests/
```

### 🔍 Debugging & Logging

#### Serilog Configuration
```csharp
// Program.cs
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
```

#### Log Levels
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    }
  }
}
```

### 📦 Package Management

#### Main Dependencies
```xml
<!-- MediatR for CQRS -->
<PackageReference Include="MediatR" Version="12.1.1" />

<!-- Entity Framework -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="7.0.0" />

<!-- Authentication -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="7.0.0" />

<!-- Validation -->
<PackageReference Include="FluentValidation" Version="11.11.0" />

<!-- Mapping -->
<PackageReference Include="AutoMapper" Version="12.0.1" />
```

### 🚨 Troubleshooting

#### Common Issues

1. **Build Errors**:
   ```bash
   # Clear và rebuild
   dotnet clean
   dotnet restore
   dotnet build
   ```

2. **Database Issues**:
   ```bash
   # Reset database
   dotnet ef database drop --project src/DemoCICD.Persistence --startup-project src/DemoCICD.API
   dotnet ef database update --project src/DemoCICD.Persistence --startup-project src/DemoCICD.API
   ```

3. **Test Failures**:
   ```bash
   # Check test output
   dotnet test --logger "console;verbosity=detailed"
   ```

### 🔧 Development Tools

#### Recommended VS Code Extensions
```json
{
  "recommendations": [
    "ms-dotnettools.csharp",
    "ms-dotnettools.vscode-dotnet-runtime",
    "jchannon.csharpextensions",
    "formulahendry.dotnet-test-explorer"
  ]
}
```

#### Git Workflow
```bash
# Feature development
git checkout -b feature/new-feature
git add .
git commit -m "feat: implement new feature"
git push origin feature/new-feature

# Create PR to develop branch
```

### 📈 Performance Tips

#### Database Optimization
```csharp
// Use AsNoTracking for read-only queries
var riders = await context.Riders
    .AsNoTracking()
    .Where(r => r.IsActive)
    .ToListAsync();

// Use Include for eager loading
var riderWithTeam = await context.Riders
    .Include(r => r.Team)
    .FirstOrDefaultAsync(r => r.Id == id);
```

#### Caching Best Practices
```csharp
// Redis caching example
public async Task<RiderResponse> GetRiderAsync(Guid id)
{
    var cacheKey = $"rider:{id}";
    var cached = await _cache.GetStringAsync(cacheKey);
    
    if (cached != null)
        return JsonSerializer.Deserialize<RiderResponse>(cached);
        
    var rider = await _repository.GetByIdAsync(id);
    var response = _mapper.Map<RiderResponse>(rider);
    
    await _cache.SetStringAsync(cacheKey, 
        JsonSerializer.Serialize(response),
        TimeSpan.FromMinutes(30));
        
    return response;
}
```

### 📞 Support & Documentation

#### Links Hữu Ích
- **Clean Architecture**: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html
- **CQRS Pattern**: https://martinfowler.com/bliki/CQRS.html
- **MediatR Documentation**: https://github.com/jbogard/MediatR
- **Entity Framework**: https://docs.microsoft.com/en-us/ef/core/

#### Contact
- **Repository**: https://github.com/taquangtrung115/CleanArchitecture-CICD
- **Issues**: Create GitHub issue for bugs hoặc feature requests
- **Discussions**: Use GitHub Discussions for questions

---

**Last Updated**: Current Date  
**Maintainer**: Development Team