# Zero-Downtime Deployment

## Vấn đề với cách deploy thông thường

```bash
# ❌ Cách cũ — gây downtime = toàn bộ thời gian build (~2-5 phút)
docker compose -f docker-compose.prod.yml up -d --build api
```

Lệnh này dừng container cũ → build image mới → khởi container mới.
Trong suốt thời gian build, server **không phục vụ được request**.

---

## Cơ chế Zero-Downtime

Ý tưởng: **tách bước build ra khỏi bước restart**.

```
[Container cũ đang chạy] ──serving──▶ users
         │
         ▼
    build image mới  ← không ảnh hưởng container đang chạy
         │
         ▼
[Container mới healthy] ──▶ Nginx nhận traffic
         │
         ▼
    dừng container cũ
```

### Cách 1: Downtime ~2-3s (đơn giản)

```bash
git pull

# Bước 1: Build image mới (container cũ vẫn serve)
docker compose -f docker-compose.prod.yml build api

# Bước 2: Swap container (chỉ mất ~2-3s)
docker compose -f docker-compose.prod.yml up -d --no-deps api
```

Downtime chỉ là khoảng thời gian dừng container cũ → cái mới nhận traffic.

### Cách 2: 0s downtime (dùng scaling)

Nginx đã cấu hình `upstream api_servers` với `least_conn` load balancing.
Khi scale lên 2 replica, Nginx tự route traffic sang cả 2 — container cũ vẫn serve trong lúc container mới khởi động.

```bash
git pull
docker compose -f docker-compose.prod.yml build api

# Thêm replica mới (Nginx tự load balance)
docker compose -f docker-compose.prod.yml up -d --no-deps --scale api=2 api

# Đợi container mới healthy
sleep 25

# Xóa replica cũ
docker compose -f docker-compose.prod.yml up -d --no-deps --scale api=1 api
```

### Cách 3: Dùng script tự động

```bash
./deploy.sh api        # Deploy chỉ backend
./deploy.sh frontend   # Deploy chỉ frontend
./deploy.sh all        # Deploy cả hai
```

Script `deploy.sh` ở root project thực hiện toàn bộ flow (Cách 2) tự động.

---

## Tại sao frontend không cần scale?

Frontend là **static file** (HTML/JS/CSS) do Nginx serve.
Không có state → rebuild nhanh (~30s) → swap container không gây lỗi user.

> **Lưu ý:** `deploy.update_config.order: start-first` trong `docker-compose.prod.yml`
> chỉ có hiệu lực khi dùng **Docker Swarm** (`docker swarm init`).
> Với Docker Compose thường, dùng script `deploy.sh` hoặc Cách 1/2 ở trên.
