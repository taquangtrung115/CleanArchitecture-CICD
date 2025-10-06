# CleanArchitecture-CICD Setup Guide

## Prerequisites

### Required Software
- **.NET 8.0 SDK** or later ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- **Node.js 18+** and npm ([Download](https://nodejs.org/))
- **SQL Server** (LocalDB, Express, or full version)
- **Redis** (optional, for caching features)

### Verify Installation
```bash
# Check .NET version (should be 8.0 or higher)
dotnet --version

# Check Node version
node --version

# Check npm version
npm --version
```

## Backend Setup

### 1. Clone Repository
```bash
git clone https://github.com/taquangtrung115/CleanArchitecture-CICD.git
cd CleanArchitecture-CICD
```

### 2. Configure Database Connection
Update connection string in `src/DemoCICD.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DemoCICDDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Configure OpenAI (Optional)
For AI features, set your OpenAI API key:
```json
{
  "OpenAI": {
    "ApiKey": "your-api-key-here"
  }
}
```

Or use demo mode by setting:
```json
{
  "OpenAI": {
    "ApiKey": "demo-key"
  }
}
```

### 4. Restore Dependencies
```bash
dotnet restore
```

### 5. Apply Database Migrations
```bash
dotnet ef database update --project src/DemoCICD.Persistence --startup-project src/DemoCICD.API
```

### 6. Build Backend
```bash
dotnet build
```

### 7. Run Tests (Optional)
```bash
dotnet test
```

### 8. Run API
```bash
cd src/DemoCICD.API
dotnet run
```

API will be available at:
- HTTP: `http://localhost:5258`
- HTTPS: `https://localhost:7258`
- Swagger: `http://localhost:5258/swagger`

## Frontend Setup

### 1. Navigate to Frontend Directory
```bash
cd frontend
```

### 2. Install Dependencies
```bash
npm install --legacy-peer-deps
```

### 3. Configure API URL
Update `.env` file if needed:
```env
VITE_API_BASE_URL="http://localhost:5258"
```

### 4. Run Development Server
```bash
npm start
```

Frontend will be available at `http://localhost:3000`

### 5. Build for Production (Optional)
```bash
npm run build
```

## Features

### Identity Management
- User Management
- Role Management
- Permission Management
- Position Management
- Action Management
- JWT Authentication

### MotoGP Features
- Rider Management
- Team Management
- Race Management
- News & Video Management

### AI Features
- AI Chat Agent for Role/Permission Management
- Natural language processing (Vietnamese & English)
- OpenAI/Ollama Integration

### Communication
- User-to-User Chat (SignalR)
- Real-time notifications

## Testing

### Run All Tests
```bash
dotnet test
```

### Run Specific Test Project
```bash
# Unit Tests
dotnet test test/DemoCICD.UnitTests

# Architecture Tests
dotnet test test/DemoCICD.Architecture.Tests

# Identity Tests
dotnet test test/DemoCICD.Identity.Tests
```

### Test Coverage
- **Unit Tests:** 171 tests
- **Architecture Tests:** 138 tests
- **Identity Tests:** 6 tests
- **Total:** 315 tests

## Troubleshooting

### Issue: .NET 7.0 Required
**Solution:** This project now uses .NET 8.0. Install .NET 8.0 SDK.

### Issue: Database Connection Failed
**Solutions:**
1. Verify SQL Server is running
2. Check connection string
3. Ensure database exists or run migrations

### Issue: npm Install Fails
**Solution:** Use `npm install --legacy-peer-deps`

### Issue: Redis Connection Failed
**Solution:** Redis is optional. You can disable it in configuration or install Redis locally.

### Issue: OpenAI API Errors
**Solutions:**
1. Verify API key is valid
2. Use "demo-key" for testing without OpenAI
3. Check network connectivity

## Architecture

This project follows **Clean Architecture** principles:

```
┌─────────────────────────────────────┐
│      Presentation Layer             │  ← APIs (Carter)
├─────────────────────────────────────┤
│      Application Layer              │  ← Use Cases (MediatR)
├─────────────────────────────────────┤
│      Domain Layer                   │  ← Entities, Services
├─────────────────────────────────────┤
│      Infrastructure Layer           │  ← External Services
├─────────────────────────────────────┤
│      Persistence Layer              │  ← EF Core, Repositories
└─────────────────────────────────────┘
```

## Default Users

After running migrations, you can use these default credentials:
- **Admin:** admin@example.com / Admin@123
- **User:** user@example.com / User@123

*(Note: These are examples - check seeding configuration for actual credentials)*

## Documentation

- [AI Agent Guide (Vietnamese)](AI_AGENT_VIETNAMESE_GUIDE.md)
- [AI Agent Documentation](AI_AGENT_DOCUMENTATION.md)
- [Error Handling Guide](ERROR_HANDLING_GUIDE.md)
- [Identity Management API](IDENTITY_MANAGEMENT_API.md)
- [MotoGP API Documentation](MOTOGP_API_DOCUMENTATION.md)
- [Repository Review Summary](REPOSITORY_REVIEW_SUMMARY.md)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run tests
5. Submit a pull request

## License

See LICENSE file for details.

## Support

For issues and questions:
- Create an issue on GitHub
- Contact: [Your Contact Info]
- Facebook Group: https://www.facebook.com/groups/342670156801353
