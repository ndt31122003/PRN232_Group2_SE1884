# Chạy Backend bằng Docker

## Yêu cầu
- Docker
- Docker Compose
- .NET 8 SDK (để chạy migration locally)

## Cách 1: Sử dụng Docker Compose (Khuyến nghị)

### Build và chạy container
```bash
cd BE
docker compose build
docker compose up -d
```

### Chạy Migration (lần đầu hoặc khi có thay đổi database)
```bash
cd BE/PRN232_EbayClone/PRN232_EbayClone.Infrastructure
dotnet ef database update --connection "Host=aws-1-ap-northeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.cdvfbycudyhkqowylnhk;Password=EmThinhShizuka"
```

Hoặc nếu chạy migration từ trong container:
```bash
docker compose exec api dotnet ef database update --project /app/PRN232_EbayClone.Infrastructure/PRN232_EbayClone.Infrastructure.csproj --startup-project /app/PRN232_EbayClone.Api/PRN232_EbayClone.Api.csproj --no-build
```

## EF Core Migration Commands

### Tạo Migration mới
```bash
cd BE/PRN232_EbayClone/PRN232_EbayClone.Infrastructure
dotnet ef migrations add <MigrationName>
```

### Xem danh sách Migration
```bash
dotnet ef migrations list
```

### Revert Migration (xóa migration cuối cùng)
```bash
dotnet ef migrations remove
```

### Tạo lại toàn bộ Migration (xóa hết và tạo lại)
```bash
# Xóa các file migration trong folder Migrations (giữ lại ApplicationDbContextModelSnapshot.cs nếu cần)
dotnet ef migrations add InitialCreate
```

### Update database lên migration mới nhất
```bash
dotnet ef database update
```

### Update database đến một migration cụ thể
```bash
dotnet ef database update <MigrationName>
```

## API Endpoints

- Swagger: http://localhost:8080/swagger
- API: http://localhost:8080
- Health Check: http://localhost:8080/

## Đăng ký tài khoản

```bash
curl -X POST http://localhost:8080/api/identity/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "fullName": "Test User",
    "password": "Password123!"
  }'
```

Password cần: 8+ ký tự, chữ hoa, chữ thường, số, ký tự đặc biệt

## Đăng nhập

```bash
curl -X POST http://localhost:8080/api/identity/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "test@example.com",
    "password": "Password123!"
  }'
```

## Xóa container

```bash
docker compose down
```
