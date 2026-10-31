# Event Management & Ticketing System - Week 4 (Final)

## Mục tiêu tuần 4
Tích hợp toàn bộ hệ thống thành phiên bản cuối: Domain Model + Ticketing Service + Check-in + Reporting + Web API/UI + Voucher.

## Thành phần
- `EventTicketing.Domain`: Event, TicketTier, GeneralTier, VIPTier, Ticket.
- `EventTicketing.Services`: TicketingService, CheckInService, ReportingService.
- `EventTicketing.Web`: ASP.NET Core Web API + giao diện web đơn giản.
- `Voucher`: mã giảm giá được đóng gói trong class `Voucher`.

## API chính
- `GET /api/events`: xem sự kiện, ticket tiers, giá, sức chứa, số đã bán và số còn lại.
- `POST /api/buy-ticket`: mua vé; hỗ trợ voucher hợp lệ và từ chối voucher sai.
- `POST /api/checkin`: check-in bằng ticket code và chống double check-in.

## Chạy project
Yêu cầu .NET 10 SDK.

```bash
dotnet run --project EventTicketingSystem/EventTicketing.Web/EventTicketing.Web.csproj
```

Sau đó mở `http://localhost:5139`.

## OOP cần nhấn mạnh khi vấn đáp
- Encapsulation: `TicketTier.TryReserveSeat()` tự bảo vệ inventory; `Ticket` bảo vệ trạng thái check-in bằng private setters.
- Composition: `Event` chứa nhiều `TicketTier`; mỗi `Ticket` tham chiếu `TicketTier`.
- Single Responsibility: mua vé, check-in và reporting nằm ở các service riêng.
- Inheritance/Polymorphism: `GeneralTier` và `VIPTier` override `CalculatePrice()` và `GetPerks()`.
