# GitHub Actions Quick Reference Card

## 🎯 One-Page Cheat Sheet

### Push to DEV → Auto Build
```bash
git add .
git commit -m "Your changes"
git push origin DEV
```
**Result:** Automatic build in ~5-10 minutes

---

### Manual Workflows

**Backend Only:**
```
Actions → "DEV Backend CI" → Run workflow → Select DEV → Run
```

**Frontend Only:**
```
Actions → "DEV Frontend CI" → Run workflow → Select DEV → Run
```

---

### Download Artifacts

1. Go to **Actions** tab
2. Click latest workflow run
3. Scroll to **Artifacts** section
4. Download:
   - `backend-artifacts` → Backend app
   - `frontend-artifacts` → Frontend files
   - `migration-script` → Database SQL

---

### Deploy Database

**Using SSMS:**
1. Open `migration-script.sql`
2. Select database: `DemoCICDDatabase`
3. Execute (F5)

**Using Command Line:**
```bash
sqlcmd -S your-server -d DemoCICDDatabase -i migration-script.sql
```

---

### Deploy Backend (IIS)

```powershell
# Stop IIS
iisreset /stop

# Copy files
xcopy backend-artifacts\* C:\WWW\DemoCICD\BE\DEV\ /e /y /i /r

# Start IIS
iisreset /start
```

**Verify:** Open `http://your-server/api/swagger`

---

### Deploy Frontend

```powershell
# Copy files
xcopy frontend-artifacts\* C:\WWW\DemoCICD\FE\DEV\ /e /y /i /r
```

**Verify:** Open `http://your-server`

---

## 🔍 Quick Troubleshooting

### Workflow Didn't Start?
- ✅ Check branch name is `DEV`
- ✅ Check Actions are enabled (Settings → Actions)
- ✅ Check workflow file syntax

### Build Failed?
- ✅ Check logs in Actions tab
- ✅ Test build locally first
- ✅ Check error message carefully

### Can't Download Artifacts?
- ✅ Artifacts expire after 7 days
- ✅ Check if job succeeded (green checkmark)
- ✅ Verify repository access

### Tests Failed?
- ✅ Run tests locally: `dotnet test`
- ✅ Check test logs in Actions
- ✅ Use manual workflow with "Skip tests" option

---

## 📊 Workflow Status Icons

| Icon | Status | Action |
|------|--------|--------|
| 🟡 Yellow | Running | Wait for completion |
| ✅ Green | Success | Download artifacts |
| ❌ Red | Failed | Check logs |
| ⚪ Gray | Skipped | Not run |

---

## ⏱️ Typical Build Times

| Component | Time | Notes |
|-----------|------|-------|
| Backend | 3-5 min | Includes restore, build, test |
| Frontend | 2-3 min | Includes install, lint, build |
| Database | 1-2 min | Checks migrations, generates SQL |
| **Total** | **5-10 min** | **Jobs run in parallel** |

---

## 🔐 Using Secrets

### Add Secret:
```
Settings → Secrets and variables → Actions → New secret
```

### Use in Workflow:
```yaml
env:
  API_KEY: ${{ secrets.API_KEY }}
```

**Note:** Never commit secrets to code!

---

## 🎛️ Workflow Files

| File | Purpose | Trigger |
|------|---------|---------|
| `dev-cicd.yml` | Full pipeline | Auto (push to DEV) |
| `dev-backend.yml` | Backend only | Manual |
| `dev-frontend.yml` | Frontend only | Manual |

---

## 📚 Documentation Quick Links

- **Setup Guide:** [GITHUB_ACTIONS_SETUP.md](GITHUB_ACTIONS_SETUP.md)
- **Vietnamese Guide:** [HUONG_DAN_GITHUB_ACTIONS.md](HUONG_DAN_GITHUB_ACTIONS.md)
- **Workflows Detail:** [workflows/README.md](workflows/README.md)
- **Troubleshooting:** [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
- **Architecture:** [WORKFLOW_DIAGRAM.md](WORKFLOW_DIAGRAM.md)

---

## 🚨 Common Commands

### View Workflow Runs
```
Repository → Actions → Select workflow
```

### Re-run Failed Workflow
```
Workflow run → Re-run all jobs
```

### Cancel Running Workflow
```
Workflow run → Cancel workflow
```

### View Job Logs
```
Workflow run → Click job name → Expand steps
```

---

## 📦 Artifact Contents

### backend-artifacts
```
├── DemoCICD.API.dll
├── DemoCICD.API.exe
├── appsettings.json
├── web.config
└── ... (dependencies)
```

### frontend-artifacts
```
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
(Idempotent - safe to run multiple times)
```

---

## ⚡ Pro Tips

### Faster Builds
- Use manual workflows for single component
- Skip tests when just checking build: `Skip tests: true`
- Skip linting for quick frontend check: `Skip lint: true`

### Better Debugging
- Enable debug logs: Set `ACTIONS_STEP_DEBUG = true` in repo variables
- Check specific job logs, not just summary
- Use `continue-on-error: true` for non-critical steps

### Artifact Management
- Download artifacts before 7-day expiration
- Use manual workflows for numbered builds (tracking)
- Check artifact size if download is slow

### Security
- Always use Secrets for sensitive data
- Never log secrets (they're auto-masked anyway)
- Review workflow permissions regularly

---

## 🔄 Workflow Execution Order

### Automatic (dev-cicd.yml)
```
Push → Checkout → Setup
         ↓
    ┌────┴────┬────────┐
    ↓         ↓        ↓
Backend  Frontend  Database
    │         │        │
    └────┬────┴────────┘
         ↓
    Artifacts → Summary
```

### Manual (backend/frontend)
```
Click Run → Select Options → Build → Artifact
```

---

## 📞 Need Help?

### Quick Checks
1. ✅ Check GitHub Status: https://www.githubstatus.com/
2. ✅ Read error logs carefully
3. ✅ Test locally first
4. ✅ Search GitHub Issues
5. ✅ Check documentation

### Resources
- [GitHub Actions Docs](https://docs.github.com/en/actions)
- [.NET Actions Guide](https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-net)
- [Node.js Actions Guide](https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-nodejs)

---

## ✅ Pre-Deployment Checklist

Before deploying to server:

- [ ] Workflow completed successfully (green)
- [ ] All 3 artifacts downloaded
- [ ] Database backup created
- [ ] Files backup created
- [ ] Migration script reviewed
- [ ] Test deployment procedure
- [ ] Verify IIS/web server stopped
- [ ] Copy files complete
- [ ] Start IIS/web server
- [ ] Verify application works
- [ ] Check logs for errors

---

## 🎯 Common Scenarios

### Scenario 1: Quick Backend Fix
```
1. Fix code
2. Run "DEV Backend CI" (manual)
3. Skip tests if in hurry
4. Download artifact in ~3 min
5. Deploy backend only
```

### Scenario 2: UI Change Only
```
1. Update UI
2. Run "DEV Frontend CI" (manual)
3. Download artifact in ~2 min
4. Deploy frontend only
```

### Scenario 3: Full Release
```
1. Push to DEV branch
2. Wait for auto build (~10 min)
3. Download all 3 artifacts
4. Deploy: DB → Backend → Frontend
5. Verify everything works
```

### Scenario 4: Database Migration
```
1. Create migration locally
2. Push to DEV
3. Auto build generates SQL
4. Download migration-script
5. Review SQL carefully
6. Apply to database
7. Deploy backend (may have schema changes)
```

---

## 🎉 Success Indicators

**Build Success:**
- ✅ Green checkmark in Actions
- ✅ All jobs completed
- ✅ Artifacts available
- ✅ No error messages

**Deployment Success:**
- ✅ Backend API responds
- ✅ Swagger UI loads
- ✅ Frontend loads
- ✅ No console errors
- ✅ Database updated

---

## 📝 Notes

- **Free for public repos** (unlimited minutes)
- **Artifacts kept for 7 days** (configurable up to 90 days)
- **Max workflow runtime:** 6 hours (default: unlimited)
- **Max concurrent jobs:** 20 (free tier)
- **Max artifact size:** 2GB per file

---

**Print this page for quick reference! 📄**

*Last Updated: 2025 | Version 1.0*
