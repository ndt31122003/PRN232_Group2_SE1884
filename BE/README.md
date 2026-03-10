# Chạy Backend bằng Docker

## Yêu cầu
- Docker
- Docker Compose

## Cách 1: Sử dụng Docker Compose (Khuyến nghị)

```bash
cd BE
docker-compose up -d
```

Đợi container khởi động xong, sau đó chạy migration:

```bash
docker-compose exec api dotnet ef database update --project /app/PRN232_EbayClone.Infrastructure/PRN232_EbayClone.Infrastructure.csproj --startup-project /app/PRN232_EbayClone.Api/PRN232_EbayClone.Api.csproj --no-build
```

## Cách 2: Chạy thủ công

### 1. Chạy Redis và PostgreSQL
```bash
docker run -d --name redis -p 6379:6379 redis:alpine
docker run -d --name postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=ebayclone -p 5432:5432 postgres:15-alpine
```

### 2. Build và chạy API
```bash
cd BE
docker build -t ebayclone-api -f PRN232_EbayClone/PRN232_EbayClone.Api/Dockerfile .
docker run -d -p 8080:8080 -p 8081:8081 --name ebayclone-api --link postgres:postgres --link redis:redis ebayclone-api
```

### 3. Chạy Migration
```bash
docker exec ebayclone-api dotnet ef database update --project /app/PRN232_EbayClone.Infrastructure/PRN232_EbayClone.Infrastructure.csproj --startup-project /app/PRN232_EbayClone.Api/PRN232_EbayClone.Api.csproj
```

## API

- Swagger: http://localhost:8080/swagger
- API: http://localhost:8080

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
docker rm -f ebayclone-api
```
