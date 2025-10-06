# GitHub Actions Workflows

## Available Workflows

### 1. `dev-cicd.yml` - Complete DEV CI/CD Pipeline
**Auto-triggered on:** Push or Pull Request to `DEV` branch

Complete CI/CD pipeline that:
- ✅ Builds and tests Backend (.NET 7.0)
- ✅ Builds Frontend (React + Vite)
- ✅ Checks database migrations
- ✅ Generates artifacts for deployment

**Use when:** Making changes to DEV branch (automatic)

---

### 2. `dev-backend.yml` - Backend Only Build
**Manual trigger:** Workflow dispatch

Isolated backend workflow that:
- ✅ Builds .NET solution
- ✅ Runs all test suites (Architecture, Identity, Unit tests)
- ✅ Publishes backend artifacts
- ⚙️ Option to skip tests for quick builds

**Use when:** 
- Testing backend changes only
- Quick backend iteration
- Debugging backend build issues

**How to run:**
```
GitHub → Actions → "DEV Backend CI" → Run workflow
```

---

### 3. `dev-frontend.yml` - Frontend Only Build
**Manual trigger:** Workflow dispatch

Isolated frontend workflow that:
- ✅ Builds React application
- ✅ Runs linting checks
- ✅ Publishes frontend artifacts
- ⚙️ Option to skip linting for quick builds

**Use when:**
- Testing frontend changes only
- Quick UI iteration
- Debugging frontend build issues

**How to run:**
```
GitHub → Actions → "DEV Frontend CI" → Run workflow
```

---

## Quick Start

### For Automatic Deployment (Recommended)
1. Push your code to `DEV` branch
2. Wait for `dev-cicd.yml` to complete (~5-10 minutes)
3. Download artifacts from Actions tab
4. Deploy to your server

### For Quick Testing
1. Use manual workflows (`dev-backend.yml` or `dev-frontend.yml`)
2. Run only what you need
3. Skip tests/linting if needed for faster iteration

---

## Workflow Architecture

```
┌─────────────────────────────────────────────┐
│         Push/PR to DEV Branch               │
└──────────────┬──────────────────────────────┘
               │
               ▼
    ┌──────────────────────┐
    │   dev-cicd.yml       │ (Main Pipeline)
    └──────────┬───────────┘
               │
       ┌───────┴────────┐
       │                │
       ▼                ▼
   ┌────────┐      ┌─────────┐
   │Backend │      │Frontend │
   │  Job   │      │   Job   │
   └───┬────┘      └────┬────┘
       │                │
       └────────┬───────┘
                │
                ▼
         ┌──────────┐
         │ Database │
         │   Job    │
         └────┬─────┘
              │
              ▼
       ┌─────────────┐
       │  Artifacts  │
       └─────────────┘
```

---

## Manual Workflows

```
Developer wants quick testing
           │
           ├─── Backend changes?
           │    └─► Run dev-backend.yml
           │
           └─── Frontend changes?
                └─► Run dev-frontend.yml
```

---

## Environment Variables

All workflows use these common variables:

| Variable | Value | Description |
|----------|-------|-------------|
| `DOTNET_VERSION` | 7.0.x | .NET SDK version |
| `NODE_VERSION` | 22 | Node.js version |
| `BUILD_CONFIGURATION` | Release | Build configuration |

---

## Artifacts

Each workflow produces artifacts that are stored for 7 days:

| Artifact Name | Content | Workflow |
|--------------|---------|----------|
| `backend-artifacts` | Published .NET application | dev-cicd.yml |
| `frontend-artifacts` | Built Vite dist files | dev-cicd.yml |
| `migration-script` | SQL migration script | dev-cicd.yml |
| `backend-dev-build-{number}` | Backend build with run number | dev-backend.yml |
| `frontend-dev-build-{number}` | Frontend build with run number | dev-frontend.yml |

---

## Adding New Workflows

To add a new workflow:

1. Create a new `.yml` file in this directory
2. Use this template:

```yaml
name: Your Workflow Name

on:
  push:
    branches:
      - YOUR_BRANCH
  # or
  workflow_dispatch:

jobs:
  your-job:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      # Add your steps here
```

3. Commit and push
4. Workflow will appear in Actions tab

---

## Troubleshooting

### Workflow not running?
- Check if branch name matches trigger configuration
- Verify workflow file syntax (use YAML validator)
- Check repository Actions permissions: Settings → Actions → General

### Build failing?
- Check workflow logs in Actions tab
- Test build locally first
- Verify dependencies and versions

### Can't find artifacts?
- Artifacts expire after 7 days (configurable)
- Check if job completed successfully
- Look in workflow run details, scroll to bottom

---

## Security Notes

⚠️ **Never commit secrets to workflow files!**

Instead:
1. Go to repository Settings
2. Navigate to Secrets and variables → Actions
3. Add secrets there
4. Reference them in workflows: `${{ secrets.YOUR_SECRET }}`

---

## Learn More

- 📚 [Main Setup Guide](../GITHUB_ACTIONS_SETUP.md)
- 📖 [GitHub Actions Documentation](https://docs.github.com/en/actions)
- 🔧 [Workflow Syntax](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions)
