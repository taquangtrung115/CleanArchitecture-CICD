# GitHub Actions Troubleshooting Guide

## Common Issues and Solutions

### 1. Workflow Not Triggering

#### Issue: Pushed to DEV but workflow didn't start

**Possible Causes:**
- Branch name mismatch
- Workflow file has syntax errors
- Actions disabled for repository

**Solutions:**

1. **Check Branch Name**
   ```bash
   git branch
   # Make sure you're on DEV branch
   
   git push origin DEV
   # Push explicitly to DEV
   ```

2. **Verify Workflow Syntax**
   ```bash
   # Use online YAML validator
   # https://www.yamllint.com/
   
   # Or use Python
   python3 -c "import yaml; yaml.safe_load(open('.github/workflows/dev-cicd.yml'))"
   ```

3. **Check Actions Permissions**
   - Go to Settings → Actions → General
   - Ensure "Actions permissions" is set to "Allow all actions and reusable workflows"
   - Check if specific workflows are allowed

4. **Check GitHub Status**
   - Visit https://www.githubstatus.com/
   - Verify Actions service is operational

---

### 2. Backend Build Failures

#### Issue: "dotnet restore" fails

**Error Message:**
```
Unable to load the service index for source https://api.nuget.org/v3/index.json
```

**Solutions:**

1. **Check Internet Connection**
   - GitHub runners should have internet access
   - Check for firewall or network issues

2. **Add Retry Logic** (if intermittent)
   ```yaml
   - name: Restore with retry
     run: |
       for i in {1..3}; do
         dotnet restore && break
         echo "Retry $i..."
         sleep 5
       done
   ```

3. **Use Private NuGet Feed**
   ```yaml
   - name: Add NuGet Source
     run: dotnet nuget add source ${{ secrets.NUGET_SOURCE }} -n PrivateFeed -u user -p ${{ secrets.NUGET_API_KEY }}
   ```

#### Issue: "dotnet build" fails with compilation errors

**Error Message:**
```
error CS1061: 'Type' does not contain a definition for 'Method'
```

**Solutions:**

1. **Test Locally First**
   ```bash
   dotnet clean
   dotnet restore
   dotnet build --configuration Release
   ```

2. **Check .NET Version**
   - Ensure workflow uses same version as local: `DOTNET_VERSION: '7.0.x'`
   - Update if needed

3. **Check Dependencies**
   - Verify all project references exist
   - Check package versions compatibility

4. **Clear NuGet Cache** (if corrupted)
   ```yaml
   - name: Clear Cache
     run: dotnet nuget locals all --clear
   ```

#### Issue: Tests fail in CI but pass locally

**Error Message:**
```
Test method failed: Expected True but was False
```

**Solutions:**

1. **Check Environment Differences**
   - CI uses Ubuntu Linux, local might be Windows
   - File path separators differ (/ vs \)
   - Line endings differ (LF vs CRLF)

2. **Check Connection Strings**
   - Tests might try to connect to database
   - Use in-memory database for testing
   - Mock external dependencies

3. **Check Time Zone Issues**
   ```csharp
   // Use UTC explicitly
   DateTime.UtcNow
   ```

4. **Add Debug Output**
   ```yaml
   - name: Run Tests with Debug
     run: dotnet test --logger "console;verbosity=detailed"
   ```

---

### 3. Frontend Build Failures

#### Issue: "yarn install" fails

**Error Message:**
```
error Package "package-name" not found
```

**Solutions:**

1. **Check yarn.lock**
   ```bash
   # Regenerate if corrupted
   rm yarn.lock
   yarn install
   git add yarn.lock
   git commit -m "Regenerate yarn.lock"
   ```

2. **Update Yarn Version**
   ```yaml
   - name: Set Yarn Version
     run: yarn set version stable
   ```

3. **Clear Cache**
   ```yaml
   - name: Install with clean cache
     run: yarn install --immutable --check-cache
   ```

#### Issue: "yarn build" fails

**Error Message:**
```
RollupError: Could not resolve './component' from 'src/App.jsx'
```

**Solutions:**

1. **Check File Names (Case Sensitivity)**
   - Linux is case-sensitive
   - `Component.jsx` ≠ `component.jsx`
   - Fix imports to match exact file names

2. **Check Import Paths**
   ```javascript
   // Bad (might work on Windows)
   import Component from './Component'
   
   // Good (works everywhere)
   import Component from './Component.jsx'
   ```

3. **Check Environment Variables**
   ```yaml
   - name: Build with env vars
     run: yarn build
     env:
       NODE_ENV: production
       VITE_API_URL: ${{ secrets.API_URL }}
   ```

4. **Increase Memory** (if needed)
   ```yaml
   - name: Build with more memory
     run: NODE_OPTIONS="--max-old-space-size=4096" yarn build
   ```

#### Issue: Linting errors

**Error Message:**
```
error  'variable' is assigned a value but never used  no-unused-vars
```

**Solutions:**

1. **Fix Linting Issues**
   ```bash
   # Auto-fix what can be fixed
   yarn lint:fix
   ```

2. **Skip Linting Temporarily**
   - Use manual workflow with "Skip linting" option
   - Or add to workflow:
   ```yaml
   - name: Lint
     run: yarn lint
     continue-on-error: true  # Don't fail build
   ```

3. **Disable Specific Rules** (if necessary)
   ```javascript
   // eslint.config.mjs
   export default {
     rules: {
       'no-unused-vars': 'warn', // Change from error to warn
     }
   }
   ```

---

### 4. Database Migration Issues

#### Issue: No migrations found

**Error Message:**
```
No migrations were found
```

**Solutions:**

This is normal if you haven't created migrations yet. The workflow handles this gracefully with `continue-on-error: true`.

To create migrations:
```bash
dotnet ef migrations add InitialMigration --project src/DemoCICD.Persistence --startup-project src/DemoCICD.API
git add .
git commit -m "Add database migration"
git push origin DEV
```

#### Issue: Migration script generation fails

**Error Message:**
```
Build failed for project 'DemoCICD.Persistence.csproj'
```

**Solutions:**

1. **Check Project References**
   - Ensure Persistence project compiles
   - Check all dependencies exist

2. **Check Connection String** (if required)
   ```yaml
   - name: Generate Script
     run: dotnet ef migrations script --idempotent
     env:
       ConnectionStrings__Default: "Server=localhost;Database=dummy;Trusted_Connection=True;"
   ```

3. **Update EF Core Tools**
   ```yaml
   - name: Install Latest EF Tools
     run: dotnet tool update --global dotnet-ef
   ```

---

### 5. Artifact Issues

#### Issue: Can't download artifacts

**Error Message:**
```
Artifact not found
```

**Solutions:**

1. **Check Artifact Retention**
   - Artifacts expire after 7 days (default)
   - Download before expiration

2. **Check Job Status**
   - Artifact only available if job succeeded
   - Check if upload step completed

3. **Check Permissions**
   - Ensure you have read access to repository
   - Check if Actions artifacts are enabled

#### Issue: Artifact is empty or incomplete

**Solutions:**

1. **Check Upload Path**
   ```yaml
   - name: Upload Artifacts
     uses: actions/upload-artifact@v4
     with:
       name: backend-artifacts
       path: ./publish/backend  # Verify this path exists
   ```

2. **Add Debug Step**
   ```yaml
   - name: List Files Before Upload
     run: |
       echo "Files in publish directory:"
       ls -lah ./publish/backend
   ```

3. **Check Disk Space**
   ```yaml
   - name: Check Disk Space
     run: df -h
   ```

---

### 6. Permission Issues

#### Issue: Permission denied errors

**Error Message:**
```
Permission denied (publickey)
```

**Solutions:**

1. **Check Repository Access**
   - Ensure you have push access
   - Check if branch is protected

2. **Check Actions Permissions**
   - Settings → Actions → General
   - Ensure "Read and write permissions" is set

3. **Check Token Permissions**
   - Built-in `GITHUB_TOKEN` should work
   - If using custom token, check scopes

---

### 7. Timeout Issues

#### Issue: Workflow times out

**Error Message:**
```
The job was canceled because it exceeded the maximum execution time
```

**Solutions:**

1. **Increase Timeout** (default is 360 minutes)
   ```yaml
   jobs:
     backend:
       timeout-minutes: 60  # Increase if needed
   ```

2. **Optimize Build**
   - Use caching for dependencies
   - Skip unnecessary steps
   - Use manual workflows for quick iterations

3. **Check for Hanging Processes**
   - Tests might be waiting for input
   - Add timeout to test steps:
   ```yaml
   - name: Run Tests
     run: timeout 10m dotnet test
   ```

---

### 8. Secret Issues

#### Issue: Secret not available in workflow

**Error Message:**
```
Warning: Unexpected input(s) 'SECRET_NAME'
```

**Solutions:**

1. **Verify Secret Exists**
   - Settings → Secrets and variables → Actions
   - Check secret name matches exactly (case-sensitive)

2. **Check Secret Scope**
   - Repository secrets work for all workflows
   - Environment secrets need environment specified

3. **Use Secret Correctly**
   ```yaml
   # Correct
   env:
     API_KEY: ${{ secrets.API_KEY }}
   
   # Incorrect
   env:
     API_KEY: secrets.API_KEY  # Missing ${{ }}
   ```

4. **Secrets in Logs** (won't show)
   - Secrets are automatically masked in logs
   - Can't debug by printing them
   - Use dummy values for testing

---

### 9. Cache Issues

#### Issue: Cache not being used

**Solutions:**

1. **Check Cache Key**
   ```yaml
   - uses: actions/setup-node@v4
     with:
       cache: 'yarn'
       cache-dependency-path: './frontend/yarn.lock'  # Must match
   ```

2. **Clear Cache Manually**
   - Settings → Actions → Caches
   - Delete specific caches

3. **Cache Size Limits**
   - Maximum 10GB per repository
   - Older caches automatically deleted

---

### 10. Workflow Syntax Errors

#### Issue: Workflow file has errors

**Error Message:**
```
Invalid workflow file: .github/workflows/dev-cicd.yml#L10
```

**Solutions:**

1. **Validate YAML Syntax**
   ```bash
   # Use Python
   python3 -c "import yaml; yaml.safe_load(open('.github/workflows/dev-cicd.yml'))"
   
   # Use online validator
   # https://www.yamllint.com/
   ```

2. **Common Syntax Issues**
   ```yaml
   # Bad: Inconsistent indentation
   jobs:
     backend:
       runs-on: ubuntu-latest
      steps:  # Wrong indentation
   
   # Good: Consistent indentation (2 spaces)
   jobs:
     backend:
       runs-on: ubuntu-latest
       steps:
   ```

3. **Check Required Fields**
   ```yaml
   jobs:
     job-name:
       runs-on: ubuntu-latest  # Required
       steps:  # Required
         - name: Step name  # Required
           run: echo "command"  # run or uses required
   ```

---

## Debugging Checklist

When a workflow fails, check these in order:

- [ ] Is the workflow file syntax valid?
- [ ] Did the workflow actually run?
- [ ] Which job failed?
- [ ] What's the error message in the logs?
- [ ] Does it build/test locally?
- [ ] Are all secrets configured?
- [ ] Are dependencies up to date?
- [ ] Is there enough disk space/memory?
- [ ] Are there network issues?
- [ ] Is it a timing/race condition?

## Getting Help

### Check Logs
1. Go to Actions tab
2. Click on failed workflow run
3. Click on failed job
4. Expand failed step
5. Read error messages carefully

### Search for Similar Issues
1. Copy error message
2. Search GitHub Issues
3. Search Stack Overflow
4. Check GitHub Actions documentation

### Ask for Help
Include in your question:
- Workflow file (sanitized, no secrets)
- Complete error message
- What you've tried
- What works locally

### Useful Resources
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [GitHub Actions Community Forum](https://github.community/c/code-to-cloud/github-actions/)
- [GitHub Status Page](https://www.githubstatus.com/)
- [.NET GitHub Actions Guide](https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-net)
- [Node.js GitHub Actions Guide](https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-nodejs)

---

## Still Having Issues?

If you've tried everything and still stuck:

1. **Use Manual Workflows**
   - Try backend-only or frontend-only workflow
   - Helps isolate the problem

2. **Check GitHub Status**
   - Maybe it's not your fault
   - https://www.githubstatus.com/

3. **Enable Debug Logging**
   - Settings → Secrets → Variables
   - Add variable: `ACTIONS_STEP_DEBUG` = `true`
   - Re-run workflow for detailed logs

4. **Simplify Workflow**
   - Comment out steps one by one
   - Find which step causes the issue

5. **Ask in Repository Issues**
   - Create an issue describing the problem
   - Include workflow run link
   - Include error messages

---

*Last Updated: 2025*
*For more help, see [GITHUB_ACTIONS_SETUP.md](GITHUB_ACTIONS_SETUP.md)*
