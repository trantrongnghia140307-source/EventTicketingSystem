# BÁO CÁO TIẾN ĐỘ TUẦN 3

**Dự án:** Event Management & Ticketing System

## 1. Mục tiêu

Trong tuần 3, nhóm em tiếp tục phát triển hệ thống từ phần mô hình OOP đã làm ở tuần 1 và chức năng mua vé ở tuần 2. Ở giai đoạn này, nhóm em tập trung vào việc kiểm tra vé tại sự kiện, tránh để một vé được check-in nhiều lần và bổ sung báo cáo doanh thu, tình hình tham dự.
Ngoài việc hoàn thiện các chức năng trên, nhóm em cũng muốn kiểm tra xem hệ thống có xử lý đúng khi người dùng nhập mã vé sai, mua vé khi đã hết chỗ hoặc có nhiều yêu cầu xảy ra cùng lúc hay không.

## 2. Công việc đã thực hiện

 **Check-in và quản lý trạng thái vé:**  Nhóm em xây dựng lớp CheckInService với phương thức CheckIn(). Khi nhận được mã vé, chương trình tìm vé trong danh sách vé đã phát hành của sự kiện. Nếu tìm thấy vé hợp lệ và vé chưa được sử dụng, hệ thống ghi nhận trạng thái check-in cùng thời điểm thực hiện. Các thông tin này được quản lý trong lớp Ticket thông qua IsCheckedIn và CheckedInAt.
 
**Ngăn sử dụng lại vé:** Nhóm em kiểm tra trường hợp một mã vé được quét lần thứ hai. Khi vé đã check-in, chương trình từ chối yêu cầu tiếp theo và không thay đổi thời điểm check-in ban đầu. Nhóm cũng kiểm tra mã vé không tồn tại, mã rỗng và trường hợp nhập mã bằng chữ thường. Mã vé được loại bỏ khoảng trắng ở hai đầu và chuyển thành chữ hoa trước khi tra cứu.

**Kiểm soát việc mua vé:** Nhóm em rà soát lại TicketingService để kiểm tra hạng vé có thuộc sự kiện đang mua hay không. Chương trình cũng xử lý trường hợp hạng vé đã hết chỗ, giá vé âm hoặc sức chứa bằng 0. Việc giữ chỗ và phát hành vé được thực hiện trong cùng một khóa đồng bộ để tránh tình trạng nhiều yêu cầu cùng mua làm số vé bán ra vượt sức chứa.

**Báo cáo doanh thu và tham dự:** Nhóm em bổ sung ReportingService để tổng hợp số vé đã bán và doanh thu theo từng hạng vé, sau đó tính tổng doanh thu của sự kiện. Phần báo cáo tham dự cho biết tổng số vé, số vé đã check-in, số vé chưa check-in và tỷ lệ tham dự. Doanh thu được tính dựa trên PricePaid của các vé đã phát hành.

**Kiểm tra chương trình:** Nhóm em mở rộng Program.cs thành bốn nhóm kiểm tra: mua vé và check-in thành công; các trường hợp lỗi; thao tác đồng thời; báo cáo doanh thu và tham dự. Mỗi trường hợp có kết quả [PASS] hoặc [FAIL], giúp nhóm nhận biết chức năng nào đáp ứng điều kiện kiểm tra.


## 3. Kết quả chạy thử và áp dụng OOP

### 3.1. Kết quả kiểm tra

Khi chạy chương trình demo tuần 3, nhóm em kiểm tra các tình huống từ mua vé thông thường đến những trường hợp có thể gây sai lệch dữ liệu. Kết quả tổng kết mà chương trình hiển thị là:

 **Tổng kết kiểm tra:** Chương trình chạy được 25 trường hợp kiểm tra và đạt kết quả **25 PASS, 0 FAIL**.
 
 **Mua vé và check-in:** Chương trình kiểm tra việc tính giá VIP, giảm số vé còn lại sau khi mua, lưu vé vào `Dictionary<string, Ticket>` và ghi nhận trạng thái check-in.
 
 **Các trường hợp lỗi:** Chương trình kiểm tra việc mua vượt sức chứa, sử dụng mã vé không tồn tại hoặc rỗng, check-in trùng, dùng hạng vé thuộc sự kiện khác, dùng vé của sự kiện khác, giá vé âm và sức chứa bằng 0. Chương trình cũng kiểm tra việc nhập mã vé bằng chữ thường và bảo đảm thời điểm check-in ban đầu không bị ghi đè.
 
 **Mua vé đồng thời:** Nhóm em tạo 100 luồng cùng thử mua vé VIP trong khi hạng vé chỉ có sức chứa 50. Kết quả mà chương trình kiểm tra là chỉ bán thành công 50 vé; số vé đã bán và số vé được lưu phải khớp nhau, đồng thời không phát sinh lỗi ngoài dự kiến.
 
 **Check-in đồng thời:** Khi 20 luồng cùng quét một mã vé, chương trình kiểm tra rằng chỉ một lần check-in thành công.
 
 **Báo cáo doanh thu:** Với 2 vé VIP có giá thực trả 1.200.000 VNĐ mỗi vé và 1 vé General giá 500.000 VNĐ, doanh thu VIP là 2.400.000 VNĐ, doanh thu General là 500.000 VNĐ và tổng doanh thu là 2.900.000 VNĐ.
 
 **Báo cáo tham dự:** Trong kịch bản kiểm tra có 3 vé đã bán, 2 vé đã check-in và 1 vé chưa check-in. Tỷ lệ tham dự được tính là 66,7%.

Các kết quả này giúp cho nhóm em kiểm tra không chỉ từng chức năng riêng lẻ mà còn cả tính nhất quán của dữ liệu khi nhiều yêu cầu được thực hiện cùng lúc.

### 3.2. Những kiến thức OOP được sử dụng

**Đóng gói:** Nhóm em tiếp tục sử dụng thuộc tính và phương thức để quản lý dữ liệu của vé. Chẳng hạn, `Ticket` lưu trạng thái và thời điểm check-in, các thay đổi được thực hiện thông qua phương thức của lớp thay vì tùy ý sửa dữ liệu từ bên ngoài.

**Kế thừa và đa hình:** `GeneralTier` và `VIPTier` cùng kế thừa từ lớp trừu tượng `TicketTier`. Khi chương trình tính giá vé thông qua `CalculatePrice()`, mỗi hạng vé có thể áp dụng cách tính riêng. Trong kịch bản demo, giá cơ bản của vé VIP là 1.000.000 VNĐ và giá sau khi áp dụng phụ thu 20% là 1.200.000 VNĐ.

**Trừu tượng:** `TicketTier` mô tả những đặc điểm chung của các hạng vé và định nghĩa những hành vi mà lớp dẫn xuất cần triển khai. Nhờ vậy, phần nghiệp vụ mua vé có thể làm việc với kiểu `TicketTier` mà không cần viết riêng toàn bộ quy trình cho từng hạng vé.

**Phân chia trách nhiệm giữa các lớp:** Trong quá trình hoàn thiện tuần 3, nhóm em tiếp tục tách các nghiệp vụ theo nhiệm vụ của từng lớp. `TicketingService` xử lý mua vé, `CheckInService` xác thực và check-in, còn `ReportingService` tổng hợp số liệu. Cách tổ chức này giúp mã nguồn dễ theo dõi và thuận tiện hơn khi kiểm tra từng chức năng.

**Quản lý và đồng bộ dữ liệu:** Lớp `Event` quản lý các hạng vé và vé đã phát hành. `Dictionary<string, Ticket>` cho phép tra cứu vé bằng mã vé. Nhóm cũng kiểm tra cơ chế khóa đồng bộ tại những thao tác quan trọng để tránh nhiều luồng cùng thay đổi dữ liệu gây ra việc bán vượt sức chứa hoặc check-in trùng.


## 4. Kế hoạch tiếp theo


### Kế hoạch Tuần 4
Trong tuần 4, nhóm em dự định tiếp tục phát triển hệ thống theo các hướng sau:
- Phát triển và hoàn thiện giao diện web để người dùng tương tác với hệ thống thuận tiện hơn.
- Tích hợp các chức năng nghiệp vụ với giao diện, bảo đảm quy trình mua vé và check-in hoạt động nhất quán.
- Bổ sung và hoàn thiện các chức năng mở rộng theo yêu cầu dự án, bao gồm quản lý mã giảm giá nếu phù hợp với phiên bản cuối.
- Rà soát các trường hợp dữ liệu không hợp lệ, kiểm tra giới hạn sức chứa và tính nhất quán của dữ liệu.
- Kiểm tra tổng thể hệ thống, hoàn thiện tài liệu và chuẩn bị sản phẩm cuối kỳ.

## 5. Sử dụng AI

Trong quá trình thực hiện tuần 3, nhóm em có sử dụng ChatGPT để tìm hiểu thêm về các kiến thức OOP, tham khảo cách tổ chức các lớp và rà soát một số nội dung trong báo cáo. Sau khi nhận được góp ý và nhận xét của thầy, nhóm em đã đọc lại mã nguồn tuần 3, đối chiếu với kết quả chạy chương trình và chỉnh sửa báo cáo bằng cách diễn đạt của nhóm em.

## 6. Liên kết mã nguồn và minh chứng

- **Repository:** https://github.com/trantrongnghia140307-source/EventTicketingSystem
- **Mã nguồn Week 3:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/tree/main/Week3
- **Event.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Domain/Event.cs
- **Ticket.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Domain/Ticket.cs
- **TicketTier.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Domain/TicketTier.cs
- **TicketingService.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Services/TicketingService.cs
- **CheckInService.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Services/CheckInService.cs
- **ReportingService.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Services/ReportingService.cs
- **Program.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/EventTicketing.Demo/Program.cs
- **Ảnh kết quả chạy chương trình:**
-  https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/Week3_Demo_Result.png.jpg
-  https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week3/Week3_Demo_Result%20-%201.png.jpg
- **Commit minh chứng Tuần 3:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/commit/a6c71d25059a61100ce4395f246aad898668a6fc

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
