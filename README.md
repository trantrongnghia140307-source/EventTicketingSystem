# Event Management & Ticketing System - Week 1

## Phạm vi tuần 1
Tuần 1 tập trung vào phân tích yêu cầu và xây dựng **Domain Model** cốt lõi của hệ thống bán vé.

### Đã triển khai
- `Event`: mô hình sự kiện và danh sách các hạng vé.
- `TicketTier`: lớp abstract cho hạng vé, chứa thông tin giá, sức chứa và số lượng đã bán.
- `GeneralTier`, `VIPTier`: hai subtype thể hiện inheritance + polymorphism.
- `Ticket`: mô hình vé và quan hệ tham chiếu tới `TicketTier`.
- Encapsulation cho inventory qua `SoldQuantity` và `TryReserveSeat()`.
- Composition giữa `Event` và nhiều `TicketTier`.

### Chưa triển khai ở tuần 1
- Quy trình mua vé hoàn chỉnh (`TicketingService`).
- Lưu vé đã bán trong `Dictionary` và luồng chống overselling ở tầng service.
- Check-in và chống double check-in.
- Báo cáo doanh thu/attendance.
- Web API và giao diện web.
- Voucher.

## Cấu trúc
```text
EventTicketingSystem/
└── EventTicketing.Domain/
    ├── Event.cs
    ├── Ticket.cs
    ├── TicketTier.cs
    ├── GeneralTier.cs
    ├── VIPTier.cs
    └── EventTicketing.Domain.csproj
```

## OOP thể hiện trong tuần 1
### 1. Encapsulation
`TicketTier.SoldQuantity` có `private set`, bên ngoài không được sửa trực tiếp. Việc tăng số lượng bán được thực hiện qua `TryReserveSeat()`, nơi kiểm tra `SoldQuantity >= Capacity` trước khi tăng.

### 2. Composition
`Event` sở hữu `List<TicketTier> Tiers`. Một `Event` được cấu thành từ nhiều hạng vé.

### 3. Inheritance / Polymorphism
`GeneralTier` và `VIPTier` kế thừa `TicketTier` và override `CalculatePrice()` / `GetPerks()`. Nhờ đó cùng một kiểu `TicketTier` có thể xử lý khác nhau theo subtype.

### 4. Quan hệ Ticket -> TicketTier
Mỗi `Ticket` tham chiếu tới `TicketTier` tương ứng với hạng vé của ticket.

## Build
Máy đã cài .NET 10 SDK có thể chạy:

```bash
dotnet build EventTicketing.Domain/EventTicketing.Domain.csproj
```

> Trong môi trường tạo file này không có sẵn `dotnet` CLI nên tôi không thể chạy build thực tế tại đây; mã nguồn được giữ ở dạng C# project chuẩn để bạn mở bằng Visual Studio / VS Code và build.
