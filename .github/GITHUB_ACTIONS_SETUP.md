# GitHub Actions CI/CD Setup Guide

## Tổng quan (Overview)

Repository này đã được cấu hình với GitHub Actions để tự động hóa quy trình CI/CD cho branch DEV, bao gồm:
- **Database (DB)**: Kiểm tra và tạo migration scripts
- **Backend (BE)**: Build và test .NET 7.0 API
- **Frontend (FE)**: Build React application với Vite

This repository is configured with GitHub Actions to automate CI/CD for the DEV branch, including:
- **Database (DB)**: Migration checks and script generation
- **Backend (BE)**: Build and test .NET 7.0 API
- **Frontend (FE)**: Build React application with Vite

---

## Workflows Có Sẵn (Available Workflows)

### 1. `dev-cicd.yml` - Complete DEV Environment CI/CD
**Trigger:** Tự động khi push hoặc tạo Pull Request lên branch `DEV`
**Includes:** Backend build/test + Frontend build + Database migration check

**Jobs:**
- 🔨 **Backend Build & Test**: Build .NET solution và chạy tất cả unit tests
- 🎨 **Frontend Build**: Build React frontend với Vite
- 🗄️ **Database Migration Check**: Kiểm tra migrations và tạo SQL script
- 📋 **Deployment Summary**: Tổng hợp kết quả và artifacts

**Artifacts:**
- `backend-artifacts`: Published backend application
- `frontend-artifacts`: Built frontend dist files
- `migration-script`: Idempotent SQL migration script

---

### 2. `dev-backend.yml` - Backend Only CI
**Trigger:** Manual (workflow_dispatch)
**Best for:** Backend-only changes

**Features:**
- Build toàn bộ solution
- Chạy Architecture Tests, Identity Tests, và Unit Tests riêng biệt
- Option để skip tests (có thể chọn khi chạy manual)
- Upload backend artifacts với build number

**Usage:**
1. Vào tab "Actions" trong GitHub repository
2. Chọn "DEV Backend CI"
3. Click "Run workflow"
4. Chọn branch (thường là DEV)
5. Optionally check "Skip running tests" nếu muốn build nhanh
6. Click "Run workflow"

---

### 3. `dev-frontend.yml` - Frontend Only CI
**Trigger:** Manual (workflow_dispatch)
**Best for:** Frontend-only changes

**Features:**
- Build React application với Vite
- Chạy ESLint để kiểm tra code quality
- Option để skip linting (có thể chọn khi chạy manual)
- Upload frontend artifacts với build number

**Usage:**
1. Vào tab "Actions" trong GitHub repository
2. Chọn "DEV Frontend CI"
3. Click "Run workflow"
4. Chọn branch (thường là DEV)
5. Optionally check "Skip linting" nếu muốn build nhanh
6. Click "Run workflow"

---

## Cấu Hình Môi Trường (Environment Configuration)

### Backend Environment Variables
Các biến môi trường đã được cấu hình trong workflow:

```yaml
DOTNET_VERSION: '7.0.x'
BUILD_CONFIGURATION: Release
```

### Frontend Environment Variables
```yaml
NODE_VERSION: '22'
NODE_ENV: production
VITE_APP_BASE_NAME: '/'
```

### Database Configuration
- EF Core Tools được cài đặt tự động
- Migration script được tạo dạng idempotent (safe to run multiple times)

---

## Cách Triển Khai (Deployment Guide)

### Bước 1: Chạy CI/CD Pipeline
1. Push code lên branch `DEV`
2. GitHub Actions sẽ tự động chạy workflow `dev-cicd.yml`
3. Đợi workflow hoàn thành (thường mất 5-10 phút)

### Bước 2: Download Artifacts
1. Vào tab "Actions" trong GitHub repository
2. Chọn workflow run mới nhất
3. Scroll xuống phần "Artifacts"
4. Download các artifacts cần thiết:
   - `backend-artifacts` (cho Backend)
   - `frontend-artifacts` (cho Frontend)
   - `migration-script` (cho Database)

### Bước 3: Triển Khai Database
```bash
# Chạy migration script trên SQL Server
sqlcmd -S your-server -d DemoCICDDatabase -i migration-script.sql
```

Hoặc sử dụng SQL Server Management Studio (SSMS):
1. Mở file `migration-script.sql`
2. Connect tới DEV database
3. Execute script

### Bước 4: Triển Khai Backend
```bash
# Nếu dùng IIS (Windows Server):
# 1. Stop IIS site và app pool
iisreset /stop

# 2. Copy files từ backend-artifacts tới thư mục IIS
xcopy backend-artifacts\* C:\WWW\DemoCICD\BE\DEV\ /e /y /i /r

# 3. Start IIS site và app pool
iisreset /start
```

### Bước 5: Triển Khai Frontend
```bash
# Copy files từ frontend-artifacts tới web server
# Ví dụ cho IIS:
xcopy frontend-artifacts\* C:\WWW\DemoCICD\FE\DEV\ /e /y /i /r

# Ví dụ cho Linux/nginx:
scp -r frontend-artifacts/* user@server:/var/www/democid-frontend/
```

---

## Cấu Hình GitHub Secrets (Optional)

Để triển khai tự động lên server, bạn có thể thêm các secrets sau:

1. Vào **Settings** → **Secrets and variables** → **Actions**
2. Click **New repository secret**
3. Thêm các secrets sau (tùy chọn):

| Secret Name | Description | Example |
|------------|-------------|---------|
| `DEV_DB_CONNECTION_STRING` | Connection string cho DEV database | `Server=...;Database=...` |
| `SERVER_SSH_KEY` | SSH private key để deploy lên server | `-----BEGIN RSA PRIVATE KEY-----...` |
| `SERVER_HOST` | IP hoặc domain của DEV server | `192.168.1.100` |
| `SERVER_USER` | Username để SSH vào server | `deployer` |
| `IIS_SITE_NAME` | Tên IIS site (nếu dùng IIS) | `DemoCICD.Dev` |

---

## Troubleshooting

### Backend Build Fails
**Issue:** Lỗi khi build .NET solution
**Solution:**
1. Kiểm tra logs trong workflow run
2. Đảm bảo code build thành công locally: `dotnet build DemoCICD.sln`
3. Kiểm tra dependencies và package versions

### Frontend Build Fails
**Issue:** Lỗi khi build React application
**Solution:**
1. Kiểm tra logs trong workflow run
2. Đảm bảo code build thành công locally: `cd frontend && yarn build`
3. Kiểm tra Node version và dependencies

### Test Failures
**Issue:** Unit tests fail trong CI
**Solution:**
1. Chạy tests locally: `dotnet test`
2. Kiểm tra test configuration
3. Có thể skip tests tạm thời bằng manual workflow với option "Skip running tests"

### Database Migration Issues
**Issue:** Migration script không được tạo
**Solution:**
1. Kiểm tra xem có migrations trong `src/DemoCICD.Persistence/Migrations/`
2. Chạy locally: `dotnet ef migrations list`
3. Nếu không có migrations, step này sẽ skip và không tạo lỗi

---

## So Sánh với Jenkins Pipeline

### Jenkins (DEV-Environment.ps1)
```groovy
- Chạy trên Windows agent
- Sử dụng bat commands
- Deploy trực tiếp lên IIS
- Cần cấu hình Jenkins server
```

### GitHub Actions (Workflows)
```yaml
- Chạy trên GitHub-hosted runners (Ubuntu)
- Cross-platform commands
- Tạo artifacts để deploy manual hoặc tự động
- Không cần maintain server riêng
- Free cho public repositories
```

---

## Best Practices

### 1. Branching Strategy
- `DEV` branch: Development và testing
- `main/master` branch: Production (có thể tạo workflow riêng)
- Feature branches: Tạo PR vào DEV để trigger CI

### 2. Artifact Management
- Artifacts giữ trong 7 ngày (có thể thay đổi)
- Migration scripts giữ trong 30 ngày
- Download artifacts trước khi hết hạn

### 3. Testing Strategy
- Luôn chạy tests trên DEV branch
- Có thể skip tests cho quick fixes (dùng manual workflows)
- Review test results trước khi deploy

### 4. Deployment Strategy
- Manual deployment từ artifacts (recommended cho DEV)
- Automated deployment (có thể setup sau với SSH/FTP)
- Blue-green deployment (advanced setup)

---

## Liên Hệ và Hỗ Trợ

Nếu gặp vấn đề với GitHub Actions:
1. Kiểm tra workflow logs trong tab "Actions"
2. Tham khảo GitHub Actions documentation: https://docs.github.com/en/actions
3. Kiểm tra status page: https://www.githubstatus.com/

---

## Changelog

### Version 1.0 (Initial Setup)
- ✅ Created complete CI/CD workflow for DEV branch
- ✅ Separated backend and frontend workflows
- ✅ Added database migration support
- ✅ Configured artifact uploads
- ✅ Added comprehensive documentation

---

## Tài Liệu Tham Khảo (References)

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [.NET GitHub Actions](https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-net)
- [Node.js GitHub Actions](https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-nodejs)
- [EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
