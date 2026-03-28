# PRN232 – eBay Clone (Group 2 – SE1884)

Một nền tảng thương mại điện tử mô phỏng eBay, hỗ trợ đấu giá (Auction) và mua ngay (Fixed Price), quản lý kho hàng, thông báo thời gian thực qua SignalR, và dashboard dành cho người bán.

---

## 📋 Yêu cầu Hệ thống

| Công cụ | Phiên bản tối thiểu |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download) | 8.0 |
| [Node.js](https://nodejs.org/) | 18.x trở lên |
| [npm](https://www.npmjs.com/) | 9.x trở lên |
| Git | Bất kỳ |

> **Lưu ý:** Dự án sử dụng **PostgreSQL trên Supabase** (cloud) và **Redis trên Redis Labs** (cloud) – bạn **không cần cài đặt database cục bộ**, chỉ cần kết nối Internet.

---

## 🚀 Hướng dẫn Cài đặt & Chạy

### 1. Clone dự án

> ⚠️ **Quan trọng:** Nhánh chính của dự án là `coremain`, **không phải** `main`.

```bash
git clone https://github.com/ndt31122003/PRN232_Group2_SE1884.git
cd PRN232_Group2_SE1884
git checkout coremain
```

---

### 2. Chạy Backend (.NET)

```bash
# Từ thư mục gốc của dự án
dotnet run --project BE\PRN232_EbayClone\PRN232_EbayClone.Api\PRN232_EbayClone.Api.csproj
```

Backend sẽ khởi động tại:
- **HTTP:** `http://localhost:5149`
- **HTTPS:** `https://localhost:7046`
- **Swagger UI:** `http://localhost:5149/swagger`

> ⚠️ Lần đầu chạy sẽ mất khoảng **60-90 giây** để build và tải các packages.

---

### 3. Chạy Frontend (React)

Mở terminal **mới** (song song với Backend):

```bash
cd FE
npm install       # Chỉ cần chạy lần đầu
npm start
```

Frontend sẽ chạy tại: `http://localhost:3000`

---

## 🏗️ Cấu trúc Dự án

```
PRN232_Group2_SE1884/
├── BE/                                    # Mã nguồn Backend (.NET 8)
│   └── PRN232_EbayClone/
│       ├── PRN232_EbayClone.Api/          # REST API + SignalR Hub
│       ├── PRN232_EbayClone.Application/  # Business Logic (CQRS)
│       ├── PRN232_EbayClone.Domain/       # Domain Entities
│       └── PRN232_EbayClone.Infrastructure/ # DB, Repositories, Realtime
├── FE/                                    # Mã nguồn Frontend (React SPA)
│   └── src/
│       ├── pages/                         # Các trang chính
│       ├── components/                    # Các component tái sử dụng
│       ├── hooks/                         # Custom hooks (useSignalR, ...)
│       └── services/                      # Axios API calls
├── checkdb/                               # Script kiểm tra/tạo bảng DB
└── dump-postgres-*.sql                    # File dump database dự phòng
```

---

## 🔑 Tài khoản Test

Dự án cung cấp sẵn một số tài khoản Demo để kiểm tra tính năng Real-time (Auction/Bid):

| Loại tài khoản | Email | Mật khẩu | Ghi chú |
|---|---|---|---|
| **Seller 1** | `demo.seller1@example.com` | `Thinh2003@` | Tài khoản chính để test |
| **Seller 2** | `demo.seller2@example.com` | `123abc@A` | Dùng để bid chéo qua tab 2 |
| **Seller 3** | `demo.seller3@example.com` | `123abc@A` | Tài khoản phụ |

---

## 🧪 Hướng dẫn Test Toàn Bộ Tính năng

### 🔐 1. Xác thực & Tài khoản
- **Đăng nhập/Đăng xuất**: Kiểm tra lưu token JWT và chuyển hướng.
- **Hồ sơ cá nhân**: Xem địa chỉ và trạng thái xác minh.

### 📦 2. Quản lý Listings (Sản phẩm)
- **Dashboard**: Xem danh sách Active, Scheduled, Drafts, Ended.
- **Tạo Listing**: Support cả **Auction** và **Fixed Price**. Tối ưu load Category nhanh.
- **Thao tác**: Chỉnh sửa, Kết thúc sớm, Bulk Delete, Copy Draft, Relist.
- **Data**: Hỗ trợ Export/Import danh sách sản phẩm qua file CSV.

### 🔨 3. Đấu giá (Auction) – Real-time
- **Cập nhật tức thì**: Mở 2 tab trình duyệt cùng lúc. Một tab đặt giá thầu (Bid), tab kia sẽ tự động cập nhật giá mới và nháy xanh mà không cần F5.
- **Countdown**: Đồng hồ đếm ngược chạy thời gian thực cho phiên đấu giá sắp kết thúc.

### 📢 4. Marketing & Khuyến mãi
- **Sale Events**: Giảm giá % cho các sản phẩm được chọn.
- **Offers to Buyers**: Gửi lời mời giảm giá cho những người quan tâm.
- **Order/Shipping Discounts**: Giảm giá theo đơn hàng hoặc phí vận chuyển.
- **Volume Pricing**: Giảm giá khi mua số lượng lớn (Tiered Pricing).

### 🛍️ 5. Orders & Payments
- **Quản lý đơn hàng**: Theo dõi trạng thái (Pending, Paid, Shipped, Delivered).
- **Thanh toán**: Xem lịch sử dòng tiền và doanh thu.

### ⚖️ 6. Disputes & Support Tickets
- **Disputes**: Quy trình giải quyết tranh chấp giữa Buyer & Seller (Open -> Evidence -> Escalated -> Closed).
- **Support Tickets**: Gửi yêu cầu hỗ trợ kỹ thuật đến admin.

---

## 🔌 API & Swagger
Xem toàn bộ tài liệu API tại: `http://localhost:5149/swagger`

---

## 🛠️ Khắc phục sự cố (Troubleshooting)

| Vấn đề | Giải pháp |
|---|---|
| **Lỗi Redis connection** | Bỏ qua. Redis chỉ dùng để Rate Limiting, ứng dụng vẫn hoạt động bình thường qua DB. |
| **Lỗi MSB3027 (File Lock)** | Do ứng dụng đang chạy chiếm quyền file. Hãy tắt server Backend (`Ctrl+C`) rồi mới build/chạy lại. |
| **SignalR không kết nối được** | Hệ thống đã có hỗ trợ SSE fallback. Đảm bảo port 5149 đang mở và không bị chặn bởi tường lửa. |
| **Dữ liệu trắng/Thiếu bảng** | Xem hướng dẫn **Khôi phục Database** bên dưới. |

---

## 💾 Khôi phục Database (Dòng cuối)

Nếu môi trường cloud gặp sự cố hoặc bạn muốn setup môi trường riêng, hãy sử dụng file dump SQL trong thư mục gốc:
1. Tạo database PostgreSQL mới.
2. Chạy lệnh: `psql -U username -d dbname -f dump-postgres-202603260041.sql`

---

## 🛠️ Tech Stack
- **Backend**: .NET 8 (Clean Arch), EF Core 8, Dapper, MediatR (CQRS).
- **Frontend**: React 18, Axios, SignalR Client, SCSS.
- **Infrastructure**: PostgreSQL (Supabase), Redis Cloud, Cloudinary (Images), Quartz.NET.

---

