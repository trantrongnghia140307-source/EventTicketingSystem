# BÁO CÁO TIẾN ĐỘ TUẦN 3

**Dự án:** Event Management & Ticketing System

## 1. Mục tiêu

Tiếp tục phát triển hệ thống từ nền tảng OOP của Tuần 1 và quy trình mua vé, quản lý vé đã phát hành của Tuần 2. Trong tuần 3, dự án tập trung hoàn thiện chức năng check-in, ngăn chặn việc sử dụng lại vé đã check-in, xây dựng báo cáo doanh thu và báo cáo tình hình tham dự sự kiện.

## 2. Công việc đã thực hiện

- **Check-in vé:** Xây dựng lớp `CheckInService` với phương thức `CheckIn()` để kiểm tra mã vé, xác định vé hợp lệ và ghi nhận thời điểm check-in.
- **Quản lý trạng thái vé:** Bổ sung cơ chế cập nhật trạng thái check-in trong lớp `Ticket`, lưu thông tin vé đã được xác nhận và thời điểm check-in.
- **Ngăn check-in trùng lặp:** Từ chối mã vé không tồn tại hoặc vé đã được check-in trước đó, tránh việc một vé được sử dụng nhiều lần.
- **Xử lý mã vé:** Chuẩn hóa mã vé khi kiểm tra để hỗ trợ mã nhập chữ thường và chữ hoa; xử lý các trường hợp mã vé rỗng hoặc không hợp lệ.
- **Báo cáo doanh thu:** Xây dựng lớp `ReportingService` để thống kê số vé đã bán và doanh thu theo từng hạng vé, đồng thời tính tổng doanh thu dựa trên giá thực tế của các vé đã phát hành.
- **Báo cáo tham dự:** Thống kê tổng số vé đã bán, số vé đã check-in, số vé chưa check-in và tỷ lệ tham dự sự kiện.
- **Kiểm soát dữ liệu nghiệp vụ:** Bổ sung kiểm tra để hạn chế các trường hợp mua vé với hạng vé không thuộc sự kiện hoặc sử dụng dữ liệu vé không hợp lệ.
- **Kịch bản kiểm tra console:** Mở rộng chương trình demo để kiểm tra các trường hợp mua vé, hết vé, check-in thành công, check-in trùng lặp, mã vé không hợp lệ, báo cáo và thao tác đồng thời.

## 3. Áp dụng OOP

- **Đóng gói:** Quản lý trạng thái vé, thời điểm check-in và số lượng vé thông qua thuộc tính, phương thức của các lớp; hạn chế thay đổi dữ liệu trực tiếp từ bên ngoài.
- **Kế thừa và đa hình:** Tiếp tục sử dụng lớp cơ sở `TicketTier` cùng các lớp `GeneralTier` và `VIPTier`. Mỗi hạng vé có cách tính giá và quyền lợi riêng thông qua các phương thức được định nghĩa trong lớp cơ sở.
- **Trừu tượng:** Sử dụng `TicketTier` để mô tả những đặc điểm chung của các hạng vé, đồng thời xác định các hành vi mà lớp dẫn xuất cần triển khai.
- **Phân chia trách nhiệm:** Lớp `TicketingService` xử lý nghiệp vụ mua vé; `CheckInService` phụ trách xác thực và check-in; `ReportingService` thực hiện thống kê doanh thu và tình hình tham dự.
- **Quản lý dữ liệu:** Lớp `Event` quản lý các hạng vé và vé đã phát hành thông qua các tập hợp dữ liệu, trong đó `Dictionary<string, Ticket>` hỗ trợ tra cứu vé bằng mã vé.
- **Đồng bộ thao tác:** Sử dụng cơ chế khóa khi thực hiện các thao tác quan trọng nhằm hạn chế xung đột khi có nhiều yêu cầu mua vé hoặc check-in đồng thời.

## 4. Kế hoạch tiếp theo


### Kế hoạch Tuần 4

- Phát triển và hoàn thiện giao diện web để người dùng tương tác với hệ thống thuận tiện hơn.
- Tích hợp các chức năng nghiệp vụ với giao diện, bảo đảm quy trình mua vé và check-in hoạt động nhất quán.
- Bổ sung và hoàn thiện các chức năng mở rộng theo yêu cầu dự án, bao gồm quản lý mã giảm giá nếu phù hợp với phiên bản cuối.
- Rà soát các trường hợp dữ liệu không hợp lệ, kiểm tra giới hạn sức chứa và tính nhất quán của dữ liệu.
- Kiểm tra tổng thể hệ thống, hoàn thiện tài liệu và chuẩn bị sản phẩm cuối kỳ.

## 5. Sử dụng AI

ChatGPT được sử dụng để hỗ trợ tìm hiểu các nguyên lý OOP, rà soát cấu trúc chương trình và hỗ trợ soạn thảo báo cáo tiến độ. Nội dung báo cáo được đối chiếu với mã nguồn của phiên bản Tuần 3; các giới hạn về kiểm thử và những chức năng chưa triển khai được ghi rõ. Nhóm cần trực tiếp kiểm tra, chạy chương trình và xác nhận kết quả trước khi nộp báo cáo.

## 6. Liên kết mã nguồn và minh chứng

- **Repository:** https://github.com/trantrongnghia140307-source/EventTicketingSystem
- **Mã nguồn Week 3:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/tree/main/Week3
- **Event.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Domain/Event.cs
- **Ticket.cs:** [https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Domain/Ticket.cs
- **TicketTier.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Domain/TicketTier.cs
- **TicketingService.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Services/TicketingService.cs
- **CheckInService.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Services/CheckInService.cs
- **ReportingService.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Services/ReportingService.cs
- **Program.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Demo/Program.cs
- **Ảnh kết quả chạy chương trình:**
-  https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/Week3_Demo_Result.png.jpg
-  https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/Week3_Demo_Result%20-%201.png.jpg
- **Commit minh chứng Tuần 3:** 

## 7. Thành viên và phân công công việc

### Thành viên 1: Trần Trọng Nghĩa – 25110046

- Tiếp tục hoàn thiện mô hình miền OOP, tập trung vào các lớp `Event` và `Ticket`, quản lý vé đã phát hành và trạng thái check-in.
- Phối hợp hoàn thiện cơ chế kiểm soát dữ liệu vé, bảo đảm mã vé được tra cứu và xác thực phù hợp với sự kiện.
- Xây dựng và rà soát chức năng báo cáo doanh thu, báo cáo tham dự; kiểm tra tính chính xác của các số liệu thống kê.
- Phối hợp kiểm tra các trường hợp check-in không hợp lệ, check-in trùng lặp và thao tác đồng thời.

### Thành viên 2: Trần Việt Tiến – 25110064

- Tiếp tục hoàn thiện nghiệp vụ mua vé trong lớp `TicketingService`, bảo đảm kiểm soát sức chứa và xử lý các trường hợp dữ liệu không hợp lệ.
- Triển khai chức năng check-in trong lớp `CheckInService`, bao gồm xác thực mã vé, ghi nhận thời điểm check-in và ngăn sử dụng lại vé đã check-in.
- Xây dựng, mở rộng chương trình console để minh họa và kiểm tra các chức năng mua vé, check-in và báo cáo.
- Phối hợp rà soát kết quả kiểm tra, phát hiện lỗi và hoàn thiện các chức năng trước khi chuyển sang giai đoạn phát triển Tuần 4.
