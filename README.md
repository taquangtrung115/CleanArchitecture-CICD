# Applying Migrations in EF Core
## Create Migrations
	1. Windows command prompt: "Add-Migration MigrationName [options]"
	2. dotnet CLI: "dotnet ef migrations add MigrationName [options]"
	=>> EX-PMC: "Add-Migration InitialMigration"
	=>> EX-CLI: "dotnet ef migrations add InitialMigration --output-dir Your/Directory"

## Applying Created Migration
	1. Windows command prompt: "Update-Database [options]"
	2. dotnet CLI: "dotnet ef database update [options]"
	=>> EX-PMC: "Update-Database"

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


Dapper Ref:

https://www.learndapper.com/saving-data/insert
https://github.com/CodeMazeBlog/CodeMazeGuides/tree/main/csharp-design-patterns