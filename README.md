# Course1 - Structure code - Setup CI/CD of Mr. Tran Dong
# Study and Practice, customy by Ta Quang Trung
# Reference: https://www.facebook.com/groups/342670156801353

# Applying Migrations in EF Core
## Create Migrations
	1. Windows command prompt: "Add-Migration MigrationName [options]"
	2. dotnet CLI: "dotnet ef migrations add MigrationName [options]"
	=>> EX-PMC: "Add-Migration InitialMigration"
	=>> EX-CLI: "dotnet ef migrations add InitialMigration --output-dir Your/Directory"
	=>> EX-CLI: "dotnet ef migrations add InitialMigration --project src/DemoCICD.Persistence --startup-project src/DemoCICD.API"

## Applying Created Migration
	1. Windows command prompt: "Update-Database [options]"
	2. dotnet CLI: "dotnet ef database update [options]"
	=>> EX-PMC: "Update-Database"
	=>> EX-CLI: "dotnet ef database update --project src/DemoCICD.Persistence --startup-project src/DemoCICD.API"

## Removing a Migration
	1. Windows command prompt: "Remove-Migration [options]"
	2. dotnet CLI: "dotnet ef migrations remove [options]"
	=>> EX-PMC: "Remove-Migration"

### Want to use RawQuery with EF to handle sort on multi columns with flexible sort strategy
=> Break up Clean Architecture Rule (Application have to reference to Persistence layer)

# Strategy for Execution Strategy

## SQL-SERVER-STRATEGY-1
 - Use ApplicationDbContext for entire source code => Handler global Transaction
	- Advantage:
		+ Can use pair with retry execution strategy
	- Downside:
		+ Break up Clean Architecture Rule (Application have to reference to Persistence layer)

## SQL-SERVER-STRATEGY-2
 - Use UnitOfWork for entire source code => Handler global Transaction
	- Advantage:
		+ Keep up Clean Architecture Rule (Application does not reference to Persistence layer)
	- Downside:
		+ Can not use pair with retry execution strategy
		+ 


Avoid error check for format 'using( .. ) { ... }'
=> Comment '# csharp_prefer_simple_using_statement = true:error' on .editorconfig file

# Noted: 
Trong class 'TransactionPipelineBehavior' Mình chỉ handler Transaction cho Command thôi 
vì Query chỉ Get data nên các command phải theo rule có name ending 'Command'
=>> Mình đã set rule này trong ArchitectureTest rồi

# Command-Query-Event

ICommand(V+N) : IRequest => CommandBus (ISender) => ICommandHandler : IRequestHandler
IQuery(V+N) : IRequest   => QueryBus (ISender) => IQueryHandler : IRequestHandler

IDomainEvent: Inotification => EventBus (IPublisher) => IDomainEventHandler : INotificationHandler

Inmemory (RAM)

DomainEvent : Notification : MediatR => Memory
IntegrationEvent : Masstransit | Rebus => MessageBus (RabbitMQ,Kafka)

# Clone code

## GIT: 
git clone https://oauth-key-goes-here@github.com/username/repo.git
or
git clone https://username:token@github.com/username/repo.git
## Azure DevOps:
https://<OrganizationName>@dev.azure.com/<OrganizationName>/MyTestProject/_git/TestSample

=>> Then we need to replace the first OrganizationName with PAT. So, it will be:

https://<PAT>@dev.azure.com/<OrganizationName>/MyTestProject/_git/TestSample

## Active Senbox
Active Senbox: https://learn.microsoft.com/en-us/training/modules/create-azure-storage-account/5-exercise-create-a-storage-account
## Redis
# **Cài đặt Redis**
## **Download ở link**
- https://drive.google.com/drive/folders/1CdmvxKhCfZax4rLWI6DwzpCta0JudYeB?usp=sharing
 - **Redis-x64-3.0.504.zip** giải nén mở CMD quyền admin r làm theo các bước dưới
 - **redis-desktop-manager-0.9.3.817.zip** giao diện quản lý cache
- **redisBGNTest**: dùng cho link test http://192.168.10.59:7004/
- - **redisBGNTest**: dùng cho link test http://192.168.10.59:7004/
 - **Cài đặt**: redis-server --service-install --service-name redisBGNTest --port 1201
Dapper Ref:

https://www.learndapper.com/saving-data/insert
https://github.com/CodeMazeBlog/CodeMazeGuides/tree/main/csharp-design-patterns

---

# GitHub Actions CI/CD

## 🚀 Automated Deployment for DEV Branch

This repository is configured with GitHub Actions for automated CI/CD pipeline.

### Quick Start

1. **Push to DEV branch** → Automatic build and test
2. **Check Actions tab** → Monitor workflow progress
3. **Download artifacts** → Get built files for deployment

### Available Workflows

| Workflow | Trigger | Purpose |
|----------|---------|---------|
| **DEV CI/CD** | Auto (push to DEV) | Full pipeline: BE + FE + DB |
| **Backend CI** | Manual | Backend only build & test |
| **Frontend CI** | Manual | Frontend only build |

### What Gets Built?

- ✅ **Backend**: .NET 7.0 API with all tests
- ✅ **Frontend**: React application with Vite
- ✅ **Database**: Migration scripts generated

### Getting Your Build Artifacts

1. Go to **Actions** tab in GitHub
2. Click on the latest workflow run
3. Scroll down to **Artifacts** section
4. Download:
   - `backend-artifacts` - Backend application
   - `frontend-artifacts` - Frontend dist files
   - `migration-script` - Database migration SQL

### Manual Deployment

After downloading artifacts:

**Database:**
```bash
# Run migration script on your SQL Server
sqlcmd -S your-server -d DemoCICDDatabase -i migration-script.sql
```

**Backend (IIS):**
```bash
# Stop IIS
iisreset /stop

# Copy files
xcopy backend-artifacts\* C:\WWW\DemoCICD\BE\DEV\ /e /y /i /r

# Start IIS
iisreset /start
```

**Frontend:**
```bash
# Copy to web server
xcopy frontend-artifacts\* C:\WWW\DemoCICD\FE\DEV\ /e /y /i /r
```

### 📚 Detailed Documentation

For complete setup guide, see [GitHub Actions Setup Guide](.github/GITHUB_ACTIONS_SETUP.md)

For workflow details, see [Workflows README](.github/workflows/README.md)

---

