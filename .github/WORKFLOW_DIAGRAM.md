# GitHub Actions Workflow Architecture

## Overview Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                    GitHub Repository (DEV Branch)                │
└────────────────────────────┬────────────────────────────────────┘
                             │
                    ┌────────┴────────┐
                    │   Git Push/PR   │
                    └────────┬────────┘
                             │
                             ▼
┌────────────────────────────────────────────────────────────────┐
│               GitHub Actions Workflow Trigger                   │
│                   (dev-cicd.yml starts)                        │
└────────────────────────────┬───────────────────────────────────┘
                             │
            ┌────────────────┼────────────────┐
            │                │                │
            ▼                ▼                ▼
    ┌──────────────┐  ┌──────────────┐  ┌──────────────┐
    │   Backend    │  │   Frontend   │  │   Database   │
    │   Job        │  │   Job        │  │   Job        │
    │              │  │              │  │              │
    │ • Restore    │  │ • Install    │  │ • Check      │
    │ • Build      │  │   deps       │  │   migrations │
    │ • Test       │  │ • Lint       │  │ • Generate   │
    │ • Publish    │  │ • Build      │  │   SQL script │
    └──────┬───────┘  └──────┬───────┘  └──────┬───────┘
           │                 │                  │
           ▼                 ▼                  ▼
    ┌──────────────┐  ┌──────────────┐  ┌──────────────┐
    │  backend-    │  │  frontend-   │  │  migration-  │
    │  artifacts   │  │  artifacts   │  │  script      │
    └──────┬───────┘  └──────┬───────┘  └──────┬───────┘
           │                 │                  │
           └─────────────────┼──────────────────┘
                             ▼
                    ┌────────────────┐
                    │   Deployment   │
                    │    Summary     │
                    └────────────────┘
```

## Workflow Execution Timeline

### Automatic Workflow (dev-cicd.yml)

```
Time   Stage                    Action
─────  ─────────────────────   ──────────────────────────────────
0:00   Trigger                 Push/PR to DEV branch detected
0:05   Checkout                Clone repository code
0:10   Setup Environment       Install .NET SDK & Node.js
       
       ┌─── Parallel Jobs Start ───┐
       │                           │
0:30   │ Backend:                 │ Frontend:           │ Database:
       │ • Restore packages       │ • Install yarn      │ • Install EF tools
1:00   │ • Clean solution         │ • Run linting       │ • Check migrations
2:00   │ • Build solution         │ • Build with Vite   │ • Generate SQL
3:00   │ • Run all tests          │                     │
4:00   │ • Publish artifacts      │                     │
       │                           │                     │
5:00   └─── All Jobs Complete ────┘
       
5:10   Upload Artifacts         Store build outputs
5:15   Summary Report           Display results
```

## Component Details

### Backend Job Flow
```
┌─────────────────┐
│  Setup .NET 7.0 │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Restore Packages│
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Clean Solution │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Build Solution  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   Run Tests:    │
│ • Architecture  │
│ • Identity      │
│ • Unit Tests    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Publish Backend │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Upload Artifact │
└─────────────────┘
```

### Frontend Job Flow
```
┌─────────────────┐
│ Setup Node.js   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Enable Corepack │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Set Yarn 4.9.1 │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Install Deps    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   Lint Code     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Build with Vite │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Upload Artifact │
└─────────────────┘
```

### Database Job Flow
```
┌─────────────────┐
│  Setup .NET 7.0 │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│Install EF Tools │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ List Migrations │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Generate Script │
│  (Idempotent)   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Upload Script   │
└─────────────────┘
```

## Manual Workflows

### Backend Only Workflow
```
Developer Action → Click "Run Workflow"
                              ↓
                    Select Options:
                    • Branch: DEV
                    • Skip Tests: Yes/No
                              ↓
                    Build Backend Only
                              ↓
                    Artifact: backend-dev-build-{number}
```

### Frontend Only Workflow
```
Developer Action → Click "Run Workflow"
                              ↓
                    Select Options:
                    • Branch: DEV
                    • Skip Lint: Yes/No
                              ↓
                    Build Frontend Only
                              ↓
                    Artifact: frontend-dev-build-{number}
```

## Artifact Structure

### backend-artifacts
```
backend-artifacts/
├── DemoCICD.API.dll
├── DemoCICD.API.exe
├── appsettings.json
├── appsettings.Development.json
├── appsettings.Production.json
├── web.config
└── ... (all dependencies)
```

### frontend-artifacts
```
frontend-artifacts/
├── index.html
├── assets/
│   ├── index-[hash].js
│   ├── index-[hash].css
│   └── ... (images, fonts)
└── favicon.svg
```

### migration-script
```
migration-script.sql
(Idempotent SQL script for database updates)
```

## Decision Flow for Developers

```
┌──────────────────────────────────┐
│  Need to Deploy Changes?         │
└───────────────┬──────────────────┘
                │
        ┌───────┴───────┐
        │               │
        ▼               ▼
   Full Deploy    Quick Test?
        │               │
        ▼               ▼
  Push to DEV    What changed?
        │               │
        │       ┌───────┴───────┐
        │       │               │
        │       ▼               ▼
        │   Backend?       Frontend?
        │       │               │
        │       ▼               ▼
        │  Run Backend    Run Frontend
        │   Workflow       Workflow
        │       │               │
        └───────┴───────┬───────┘
                        │
                        ▼
              ┌─────────────────┐
              │ Wait for Build  │
              └────────┬────────┘
                       │
                       ▼
              ┌─────────────────┐
              │ Download        │
              │ Artifacts       │
              └────────┬────────┘
                       │
                       ▼
              ┌─────────────────┐
              │ Deploy to Server│
              └─────────────────┘
```

## Trigger Conditions

### Automatic Triggers
```yaml
on:
  push:
    branches: [DEV]
  pull_request:
    branches: [DEV]
```

**When it runs:**
- ✅ Direct push to DEV branch
- ✅ PR merged to DEV branch
- ✅ PR opened targeting DEV branch
- ❌ Push to other branches
- ❌ Push to tags

### Manual Triggers
```yaml
on:
  workflow_dispatch:
    inputs:
      skip_tests: boolean
      skip_lint: boolean
```

**When it runs:**
- ✅ Manual click in Actions tab
- ✅ Can select branch
- ✅ Can set input parameters
- ❌ Not automatic

## Environment Variables

### Global Environment
```yaml
env:
  DOTNET_VERSION: '7.0.x'
  NODE_VERSION: '22'
  BUILD_CONFIGURATION: Release
```

### Job-Specific Environment
```yaml
# Backend Job
- run: dotnet build
  env:
    BUILD_CONFIG: Release
    
# Frontend Job
- run: yarn build
  env:
    NODE_ENV: production
    VITE_APP_BASE_NAME: '/'
```

## Success Criteria

### Backend Job Success
- ✅ All packages restored
- ✅ Solution builds without errors
- ✅ All tests pass (or marked as continue-on-error)
- ✅ Artifacts uploaded successfully

### Frontend Job Success
- ✅ Dependencies installed
- ✅ Linting passes (or continue-on-error)
- ✅ Build completes
- ✅ Artifacts uploaded successfully

### Database Job Success
- ✅ EF Core tools installed
- ✅ Migrations listed (or no migrations)
- ✅ SQL script generated (if migrations exist)
- ✅ Script uploaded (or marked as optional)

## Failure Handling

### Build Failure
```
Job Failed
    ↓
View Logs
    ↓
Identify Error
    ↓
Fix Code
    ↓
Push Again
    ↓
Workflow Retries Automatically
```

### Test Failure
```
Tests Failed
    ↓
Check Test Reporter
    ↓
Review Failed Tests
    ↓
Options:
  • Fix tests
  • Fix code
  • Skip tests (manual workflow)
```

## Monitoring and Debugging

### Check Workflow Status
1. Go to Actions tab
2. Find workflow run
3. Click to see details

### View Job Logs
1. Click on job name (Backend/Frontend/Database)
2. Expand steps to see details
3. Use Ctrl+F to search for "error"

### Download Artifacts Early
1. Artifacts available even if later jobs fail
2. Can download partial results
3. Useful for debugging

## Security Considerations

### What's Safe in Workflows
- ✅ Public package sources (NuGet, npm)
- ✅ Standard GitHub Actions
- ✅ Build and test commands

### What Should Use Secrets
- ❌ Database connection strings
- ❌ API keys
- ❌ Deployment credentials
- ❌ Private package sources

### How to Add Secrets
```
Settings → Secrets and variables → Actions
    ↓
New repository secret
    ↓
Use in workflow: ${{ secrets.SECRET_NAME }}
```

## Performance Optimization

### Current Setup
- Parallel jobs where possible
- Caching for Node.js dependencies
- Artifact retention: 7 days (configurable)

### Optimization Tips
1. **Use cache**: Already enabled for yarn
2. **Skip unnecessary steps**: Use manual workflows
3. **Fail fast**: Errors stop workflow early
4. **Parallel jobs**: Backend/Frontend run simultaneously

## Comparison with Jenkins

| Feature | Jenkins (Current) | GitHub Actions (New) |
|---------|------------------|---------------------|
| **Infrastructure** | Self-hosted Windows server | GitHub-hosted runners |
| **Maintenance** | Manual updates & patches | Managed by GitHub |
| **Cost** | Server + electricity | Free for public repos |
| **Scalability** | Limited to server capacity | Unlimited concurrent runs |
| **Setup** | Complex (Java, plugins, etc.) | YAML files only |
| **Security** | Manage yourself | Handled by GitHub |
| **Platform** | Windows (bat commands) | Cross-platform |
| **Deployment** | Direct to IIS | Artifacts for manual/auto deploy |
| **Integration** | Jenkins plugins | Native GitHub integration |
| **Logs** | Jenkins UI | GitHub Actions UI |

## Next Steps

### For Users
1. ✅ Push code to DEV
2. ✅ Monitor workflow progress
3. ✅ Download artifacts
4. ✅ Deploy to servers

### For Future Enhancement
1. 🔄 Add automated deployment to servers
2. 🔄 Add performance testing
3. 🔄 Add code coverage reports
4. 🔄 Add deployment to staging environment
5. 🔄 Add approval gates for production

---

*This diagram shows the complete architecture of the GitHub Actions CI/CD pipeline.*
*For detailed usage instructions, see [GITHUB_ACTIONS_SETUP.md](GITHUB_ACTIONS_SETUP.md)*
