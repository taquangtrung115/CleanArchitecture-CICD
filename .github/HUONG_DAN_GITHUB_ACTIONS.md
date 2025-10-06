# Hướng Dẫn Triển Khai với GitHub Actions (DEV Branch)

## 📋 Tổng Quan

Repo này đã được cấu hình sẵn GitHub Actions để tự động build và test khi bạn push code lên branch DEV.

## 🎯 Mục Tiêu

Giúp bạn triển khai hệ thống với:
- **Database (DB)**: Tự động kiểm tra migrations và tạo SQL script
- **Backend (BE)**: Build .NET 7.0 API và chạy tests
- **Frontend (FE)**: Build React application với Vite

## 🚀 Cách Sử Dụng

### Bước 1: Push Code Lên GitHub

```bash
git add .
git commit -m "Your changes"
git push origin DEV
```

### Bước 2: Theo Dõi Quá Trình Build

1. Vào repository trên GitHub
2. Click vào tab **Actions** (ở trên cùng)
3. Xem workflow đang chạy (màu vàng = đang chạy, xanh = thành công, đỏ = lỗi)

![Actions Tab](https://docs.github.com/assets/cb-23305/mw-1440/images/help/repository/actions-tab.webp)

### Bước 3: Download Kết Quả Build

Sau khi workflow chạy xong (màu xanh):

1. Click vào workflow run vừa hoàn thành
2. Scroll xuống cuối trang, tìm phần **Artifacts**
3. Download 3 files:
   - `backend-artifacts` - Backend đã build
   - `frontend-artifacts` - Frontend đã build
   - `migration-script` - SQL script cho database

![Artifacts](https://docs.github.com/assets/cb-22849/mw-1440/images/help/repository/artifact-drop-down-updated.webp)

### Bước 4: Triển Khai Lên Server

#### 4.1 Triển Khai Database

**Option 1: Dùng SSMS (SQL Server Management Studio)**
1. Mở SSMS và connect tới SQL Server
2. Mở file `migration-script.sql` đã download
3. Chọn database `DemoCICDDatabase`
4. Click Execute (F5)

**Option 2: Dùng Command Line**
```bash
sqlcmd -S your-server-name -d DemoCICDDatabase -i migration-script.sql
```

#### 4.2 Triển Khai Backend

**Nếu dùng IIS:**

```powershell
# 1. Stop IIS
iisreset /stop

# 2. Giải nén backend-artifacts.zip
Expand-Archive -Path backend-artifacts.zip -DestinationPath C:\Temp\backend

# 3. Copy files
xcopy C:\Temp\backend\* C:\WWW\DemoCICD\BE\DEV\ /e /y /i /r

# 4. Start IIS
iisreset /start
```

**Kiểm tra:**
- Mở browser: `http://your-server/api/swagger`
- Nếu thấy Swagger UI là thành công

#### 4.3 Triển Khai Frontend

```powershell
# 1. Giải nén frontend-artifacts.zip
Expand-Archive -Path frontend-artifacts.zip -DestinationPath C:\Temp\frontend

# 2. Copy files
xcopy C:\Temp\frontend\* C:\WWW\DemoCICD\FE\DEV\ /e /y /i /r
```

**Kiểm tra:**
- Mở browser: `http://your-server`
- Nếu thấy giao diện web là thành công

## 🔧 Workflows Có Sẵn

### 1. DEV CI/CD (Tự Động)
- **Khi nào chạy**: Tự động khi push/PR lên branch DEV
- **Làm gì**: Build toàn bộ (BE + FE + DB)
- **Thời gian**: ~5-10 phút
- **Kết quả**: 3 artifacts để download

### 2. Backend CI (Thủ Công)
- **Khi nào dùng**: Chỉ thay đổi Backend
- **Cách chạy**: 
  1. Vào Actions tab
  2. Chọn "DEV Backend CI"
  3. Click "Run workflow"
  4. Chọn branch DEV
  5. Click "Run workflow" (màu xanh)
- **Thời gian**: ~3-5 phút

### 3. Frontend CI (Thủ Công)
- **Khi nào dùng**: Chỉ thay đổi Frontend
- **Cách chạy**:
  1. Vào Actions tab
  2. Chọn "DEV Frontend CI"
  3. Click "Run workflow"
  4. Chọn branch DEV
  5. Click "Run workflow" (màu xanh)
- **Thời gian**: ~2-3 phút

## 📊 Theo Dõi Kết Quả

### Build Thành Công ✅
- Workflow hiển thị màu xanh
- Có artifacts để download
- Logs không có lỗi

### Build Lỗi ❌
- Workflow hiển thị màu đỏ
- Click vào để xem logs
- Tìm dòng có chữ "Error" để biết lỗi gì

**Lỗi thường gặp:**

| Lỗi | Nguyên Nhân | Giải Quyết |
|-----|------------|-----------|
| Build failed | Code có lỗi syntax | Fix code rồi push lại |
| Test failed | Unit test không pass | Fix test hoặc code |
| Restore failed | Package không tìm thấy | Check internet, NuGet sources |

## 💡 Tips và Tricks

### 1. Build Nhanh Hơn
Nếu chỉ test Frontend hoặc Backend:
- Dùng workflow thủ công (Backend CI hoặc Frontend CI)
- Có thể skip tests/linting để build nhanh hơn

### 2. Xem Logs Chi Tiết
- Click vào job bất kỳ (Backend, Frontend, Database)
- Click vào từng step để xem log
- Dùng Ctrl+F để tìm "error" hoặc "warning"

### 3. Re-run Workflow
Nếu build lỗi do lỗi tạm thời (network, GitHub server):
- Click vào workflow run
- Click "Re-run all jobs" (ở góc phải trên)

### 4. Cancel Workflow
Nếu muốn dừng workflow đang chạy:
- Click vào workflow run
- Click "Cancel workflow" (ở góc phải trên)

## 🔐 Bảo Mật

**Lưu ý quan trọng:**

❌ **KHÔNG** commit những thông tin sau vào code:
- Connection strings
- Passwords
- API keys
- Tokens

✅ **NÊN** dùng GitHub Secrets:
1. Settings → Secrets and variables → Actions
2. New repository secret
3. Thêm tên và giá trị
4. Dùng trong workflow: `${{ secrets.TEN_SECRET }}`

## 📞 Hỗ Trợ

### Nếu Gặp Vấn Đề

1. **Check logs**: Xem logs trong Actions tab
2. **Kiểm tra code**: Build thử trên local trước
3. **Xem documentation**: Đọc [GITHUB_ACTIONS_SETUP.md](GITHUB_ACTIONS_SETUP.md)
4. **Test manual**: Dùng manual workflows để test từng phần

### Build Thành Công Nhưng Deploy Lỗi

- Kiểm tra permissions (quyền ghi file)
- Kiểm tra IIS configuration
- Kiểm tra database connection string
- Xem logs của IIS/application

## 📚 Tài Liệu Thêm

- [Chi Tiết GitHub Actions Setup](GITHUB_ACTIONS_SETUP.md) (English)
- [Workflows README](workflows/README.md) (English)
- [GitHub Actions Documentation](https://docs.github.com/en/actions) (Official)

## ✅ Checklist Triển Khai

Trước khi triển khai, check list này:

- [ ] Code đã push lên branch DEV
- [ ] Workflow đã chạy xong (màu xanh)
- [ ] Đã download 3 artifacts (backend, frontend, migration-script)
- [ ] Đã backup database và files cũ
- [ ] Đã test database migration script trên dev database
- [ ] Đã stop IIS/web server trước khi copy files
- [ ] Đã verify files đã copy đúng
- [ ] Đã start IIS/web server
- [ ] Đã test application hoạt động đúng
- [ ] Đã check logs không có lỗi

## 🎉 Hoàn Thành!

Bây giờ bạn có thể:
- Push code lên DEV và tự động có build
- Download artifacts để deploy
- Không cần setup Jenkins hay CI/CD server riêng
- Free cho public repositories!

**Happy Deploying! 🚀**

---

*Cập nhật lần cuối: 2025*
*Version: 1.0*
