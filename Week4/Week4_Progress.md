# WEEK 4 - FINAL PROGRESS REPORT

## 1. Mục tiêu
Tích hợp toàn bộ phần đã xây dựng thành hệ thống cuối kỳ có thể chạy qua giao diện Web/API và hoàn thiện luồng nghiệp vụ.

## 2. Chức năng hoàn thành
- Hiển thị danh sách sự kiện và các hạng vé.
- Hiển thị giá theo polymorphism.
- Mua vé và chống overselling theo từng tier.
- Sinh ticket code duy nhất.
- Lưu ticket bằng `Dictionary<string, Ticket>` để tra cứu nhanh.
- Check-in và chặn double check-in.
- Báo cáo sales/attendance trong service layer.
- Voucher giảm giá và xử lý voucher không hợp lệ.
- Web API cho xem event, mua vé và check-in.
- Giao diện web gọi các API của hệ thống.

## 3. Yêu cầu đề bài đã được nối thành một luồng
1. Event chứa các TicketTier.
2. Người dùng chọn tier -> `TicketingService` giữ chỗ.
3. Ticket được tạo và đưa vào `IssuedTickets`.
4. Ticket code được dùng để check-in.
5. `CheckInService` kiểm tra code, chống sử dụng lại.
6. Reporting có thể tính doanh thu và attendance từ dữ liệu vé.

## 4. Validation/error scenarios
- Hạng vé hết chỗ -> từ chối mua.
- Sự kiện không tồn tại -> từ chối request.
- Hạng vé không tồn tại -> từ chối request.
- Voucher không tồn tại -> từ chối request.
- Ticket code không tồn tại -> từ chối check-in.
- Ticket đã check-in -> từ chối check-in lần hai.

## 5. Polymorphism scenario
`ApiController` chỉ gọi `tier.CalculatePrice()` trên kiểu cơ sở `TicketTier`. Đối tượng thực tế là `GeneralTier` hoặc `VIPTier`, nên kết quả giá thay đổi theo subtype mà không cần viết `if/else` theo loại vé.

## 6. Self-Explanation points
### Encapsulation
`TicketTier.SoldQuantity` có setter private và chỉ tăng thông qua `TryReserveSeat()`. Điều này đặt quy tắc chống oversell ngay tại nơi sở hữu inventory.

### Composition
`Event.Tiers` giữ danh sách ticket tiers của chính sự kiện; `Ticket.Tier` giữ tier mà ticket thuộc về.

### Single Responsibility
- `TicketingService`: mua vé.
- `CheckInService`: check-in.
- `ReportingService`: báo cáo.
- `ApiController`: nhận request HTTP và điều phối service/domain.

## 7. Final test checklist
- [ ] Build solution thành công trên máy phát triển.
- [ ] Mở trang web thành công.
- [ ] GET events hoạt động.
- [ ] Mua vé hợp lệ.
- [ ] Mua vượt capacity bị từ chối.
- [ ] Voucher hợp lệ giảm giá.
- [ ] Voucher sai bị từ chối.
- [ ] Check-in hợp lệ.
- [ ] Check-in lần 2 bị từ chối.
- [ ] Check-in code giả bị từ chối.
