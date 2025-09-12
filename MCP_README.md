# MCP Configuration for Clean Architecture Project

This repository includes a comprehensive MCP (Model Context Protocol) configuration that enhances the development experience by providing intelligent assistance for Clean Architecture patterns, CQRS implementation, and .NET development best practices.

## What is MCP?

MCP (Model Context Protocol) enables AI assistants to connect with external tools and data sources, providing enhanced capabilities for code generation, analysis, and project management.

## MCP Servers Included

### 1. Filesystem Server
- **Purpose**: Navigate and analyze project files
- **Capabilities**: Read code, understand project structure, analyze dependencies

### 2. GitHub Server  
- **Purpose**: Repository management and Git operations
- **Capabilities**: Branch management, pull requests, issue tracking

### 3. SQLite Server
- **Purpose**: Database inspection and analysis
- **Capabilities**: Query database schema, analyze data, migration assistance

### 4. Browser Automation
- **Purpose**: Web API testing and UI automation
- **Capabilities**: API endpoint testing, integration testing support

## Available Prompts

### 🏗️ Architecture & Design

#### Clean Architecture Analysis
Analyzes code structure for Clean Architecture compliance and suggests improvements.
```
Usage: Specify a layer (Domain, Application, Infrastructure, Presentation, API) or analyze the entire codebase
```

#### Domain Modeling Assistant
Helps design domain models following Domain-Driven Design principles.
```
Usage: Provide business domain and specific scenario to get tailored domain model suggestions
```

### 🔧 Code Generation

#### CQRS Command Generator
Generates complete CQRS Command implementations following project patterns.
```
Example: Entity="Product", Operation="Create", Properties="Name,Price,Category"
Generates: Command, CommandHandler, Response DTO, Validation
```

#### CQRS Query Generator
Generates complete CQRS Query implementations following project patterns.
```
Example: Entity="Product", Operation="GetBy", Filters="Category,PriceRange"
Generates: Query, QueryHandler, Response DTO, Repository methods
```

### 🗄️ Database & Persistence

#### Entity Framework Migration Helper
Assists with EF Core migrations and database schema changes.
```
Operations: create, apply, remove, rollback
Provides step-by-step guidance and best practices
```

### 📚 Documentation & Testing

#### API Documentation Generator
Generates comprehensive API documentation for endpoints.
```
Creates: OpenAPI specs, examples, validation rules, error responses
```

#### Testing Strategy Advisor
Provides testing guidance for different layers (unit, integration, architecture, e2e).
```
Generates: Test templates, mocking strategies, best practices
```

### 🚀 DevOps & Performance

#### CI/CD Pipeline Optimization
Analyzes and suggests improvements for CI/CD pipeline.
```
Areas: build, test, deploy, monitoring
Reviews PowerShell scripts and suggests optimizations
```

#### Performance Optimization Guide
Analyzes and suggests performance improvements.
```
Areas: database, API, memory, caching
Provides specific code examples and monitoring strategies
```

## Setup Instructions

### Prerequisites
- Node.js installed for MCP servers
- Git configured for GitHub integration
- SQLite for database operations (optional)

### Installation
1. The `.mcp.json` file is already configured in the repository root
2. MCP servers will be automatically downloaded when first used
3. No additional setup required - the configuration is ready to use

### Usage with AI Assistants
When working with an AI assistant that supports MCP:

1. **Architecture Review**: Ask for clean architecture analysis of specific layers
2. **Code Generation**: Request CQRS command/query generation with specific parameters
3. **Database Help**: Get assistance with EF migrations and schema changes
4. **Testing**: Request testing strategies and template generation
5. **Performance**: Ask for performance analysis and optimization suggestions
6. **Documentation**: Generate API documentation for controllers/endpoints

### Example Interactions

```
"Analyze the Application layer for Clean Architecture compliance"
"Generate a CQRS command for creating a Product with Name, Price, and Category properties"
"Help me create an EF migration for adding a new Email field to User entity"
"Generate unit tests for the ProductCommandHandler"
"Optimize the database queries in the Product repository"
```

## Configuration Details

### Global Settings
- **Timeout**: 30 seconds for MCP operations
- **Retries**: 3 attempts for failed operations
- **Log Level**: Info level logging for debugging

### Customization
The `.mcp.json` file can be customized to:
- Add additional MCP servers
- Modify prompt templates
- Adjust timeout and retry settings
- Add project-specific prompts

## Benefits

✅ **Faster Development**: Automated code generation following project patterns
✅ **Consistency**: Ensures adherence to Clean Architecture principles
✅ **Quality**: Built-in best practices and validation
✅ **Documentation**: Comprehensive API and code documentation
✅ **Testing**: Structured testing approach across all layers
✅ **Performance**: Proactive performance optimization guidance

## Support

For issues or questions about the MCP configuration:
1. Check the prompt descriptions in `.mcp.json`
2. Review this documentation
3. Consult the Clean Architecture project documentation
4. Refer to MCP official documentation for server-specific issues

---

*This MCP configuration is specifically designed for the CleanArchitecture-CICD project and follows the established patterns and conventions used throughout the codebase.*