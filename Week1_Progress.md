# Báo cáo tiến độ - Tuần 1

## Mục tiêu
Xây dựng nền tảng OOP của hệ thống Event Management & Ticketing System trước khi triển khai các luồng nghiệp vụ.

## Kết quả
1. Hoàn thành model `Event`.
2. Thiết kế abstract class `TicketTier`.
3. Tạo hai subtype `GeneralTier` và `VIPTier`.
4. Tạo model `Ticket` và quan hệ Ticket -> TicketTier.
5. Thể hiện rõ Encapsulation, Composition và Inheritance/Polymorphism.
6. Chuẩn bị inventory logic bằng `TryReserveSeat()` để các tuần sau tái sử dụng trong quy trình mua vé.

## Các phần sẽ phát triển ở tuần tiếp theo
- Ticket purchase service.
- Unique ticket code khi phát hành vé.
- Dictionary để quản lý ticket đã phát hành.
- Chống overselling và kiểm thử happy path / error path.
