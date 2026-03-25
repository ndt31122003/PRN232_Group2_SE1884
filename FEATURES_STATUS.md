# Tổng Hợp Các Chức Năng Feedback, Dispute và Support Ticket

## ✅ 1. BUYER FEEDBACK (Đánh giá của Buyer)

### Backend
- **Controller**: `BuyerFeedbackController.cs`
- **Endpoint**: `/api/buyer-feedback/*`
- **Commands**:
  - `CreateBuyerFeedbackCommand` - Tạo feedback mới
  - Các query để lấy feedback
- **Status**: ✅ Hoạt động

### Frontend
- **Page**: `FeedbackPage.jsx` (`/feedback`)
- **Features**:
  - Xem feedback nhận được (as buyer/seller)
  - Filter theo thời gian (1m, 6m, 12m, all)
  - Sort (recent/oldest)
  - Reply to feedback
  - Request revision
  - Statistics (positive/neutral/negative)
- **Status**: ✅ Hoạt động

### Routes
- `/feedback` - Trang chính xem feedback
- `/orders/bulk-feedback` - Bulk feedback cho nhiều orders

---

## ✅ 2. DISPUTES (Tranh chấp)

### Backend
- **Controllers**: 
  - `DisputesController.cs` - Main disputes API
  - `SellerDisputesController.cs` - Seller-specific disputes
- **Endpoints**: 
  - `/api/disputes/*` - General disputes
  - `/api/seller/disputes/*` - Seller disputes
- **Commands**:
  - `CreateDisputeCommand` - Tạo dispute mới
  - `RespondToDisputeCommand` - Trả lời dispute
  - `BuyerRequestRefundCommand` - Buyer yêu cầu hoàn tiền
  - `AcceptRefundCommand` - Seller chấp nhận hoàn tiền
  - `BuyerAcceptEvidenceCommand` - Buyer chấp nhận bằng chứng
  - `EscalateSellerDisputeCommand` - Escalate dispute
  - `UploadEvidenceCommand` - Upload bằng chứng
  - `CloseDisputeCommand` - Đóng dispute
- **Realtime**: ✅ SignalR notifications cho status changes và responses
- **Status**: ✅ Hoạt động

### Frontend
- **Page**: `MyDisputes.jsx` (`/disputes`)
- **Features**:
  - Xem danh sách disputes (buyer/seller view)
  - Filter theo status (Open, WaitingSeller, WaitingBuyer, Escalated, Resolved, Closed)
  - Conversation UI với response history
  - Actions theo role và status:
    - **Buyer (Open)**: Request Refund, Respond
    - **Seller (WaitingSeller)**: Accept Refund, Upload Evidence, Escalate, Respond
    - **Buyer (WaitingBuyer)**: Accept Evidence, Escalate, Respond
  - Realtime updates qua SignalR
  - File upload cho evidence
- **Status**: ✅ Hoạt động

### Dispute Flow
```
Open → WaitingSeller → WaitingBuyer → Resolved/Escalated → Closed
```

### Routes
- `/disputes` - Danh sách disputes
- `/disputes/create` - Tạo dispute mới
- `/disputes/detail/:disputeId` - Chi tiết dispute
- `/disputes/action/:disputeId/:mode` - Actions trên dispute

---

## ✅ 3. SUPPORT TICKETS (Hỗ trợ kỹ thuật)

### Backend
- **Controller**: `SellerSupportTicketsController.cs`
- **Endpoint**: `/api/seller/tickets/*`
- **Commands**:
  - `CreateSupportTicketCommand` - Tạo ticket mới
  - `GetSellerSupportTicketsQuery` - Lấy danh sách tickets
  - `GetSupportTicketByIdQuery` - Lấy chi tiết ticket
  - `UpdateSupportTicketStatusCommand` - Cập nhật status
  - `DeleteSupportTicketCommand` - Xóa ticket
- **Entity**: `SupportTicket` với các trường:
  - SellerId, Category, Subject, Message, Status
  - CreatedAt, UpdatedAt, IsDeleted
- **Status**: ✅ Hoạt động (Đã fix lỗi column mapping)

### Frontend
- **Pages**: 
  - `MySupportTickets.jsx` (`/support-tickets`)
  - `CreateSupportTicket.jsx` (`/support-tickets/create`)
- **Features**:
  - Xem danh sách support tickets
  - Filter theo status (Open, Pending, Resolved, Closed)
  - Filter theo category
  - Search theo subject/message
  - Tạo ticket mới với file upload
  - View ticket details
  - Status badges với màu sắc
- **Status**: ✅ Hoạt động

### Routes
- `/support-tickets` - Danh sách tickets
- `/support-tickets/create` - Tạo ticket mới

### Access Point
- Có nút "Support Tickets" trong `FeedbackPage` để truy cập nhanh

---

## 🔧 Recent Fixes

### Support Tickets Column Mapping Fix
- **Problem**: Backend lỗi "column s.created_by does not exist"
- **Root Cause**: Entity `SupportTicket` kế thừa từ `AggregateRoot<Guid>` có properties `CreatedBy` và `UpdatedBy`, nhưng database chỉ có `created_at` và `updated_at`
- **Solution**: Thêm `builder.Ignore(x => x.CreatedBy)` và `builder.Ignore(x => x.UpdatedBy)` vào `SupportTicketConfiguration.cs`
- **Status**: ✅ Fixed và tested

---

## 📊 Database Tables

1. **buyer_feedback** - Lưu feedback từ buyers
2. **disputes** - Lưu thông tin tranh chấp
3. **dispute_responses** - Lưu conversation trong disputes
4. **support_tickets** - Lưu support tickets
5. **reviews** - Lưu seller reviews

---

## 🔗 Integration Points

### SignalR Realtime
- **Hub**: `/hub`
- **Events**:
  - `DisputeStatusChanged` - Khi status dispute thay đổi
  - `DisputeNewResponse` - Khi có response mới
- **Connection**: `signalRConnection.js` utility
- **Authentication**: JWT token từ `STORAGE.TOKEN`

### File Upload
- **Service**: `LocalFileManager.cs`
- **Supported**: Evidence files cho disputes, attachments cho support tickets

---

## ✅ Tất Cả Chức Năng Đang Hoạt Động

Các chức năng feedback, dispute và support ticket đều đã được implement đầy đủ và đang hoạt động tốt:

1. ✅ Buyer Feedback - Hoàn chỉnh
2. ✅ Disputes với Realtime - Hoàn chỉnh
3. ✅ Support Tickets - Hoàn chỉnh (vừa fix xong)

Backend đang chạy trên port 5149, Frontend đang chạy trên port 3000.
