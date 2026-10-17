BÁO CÁO TIẾN ĐỘ TUẦN 2
Dự án: Event Management & Ticketing System
1. Mục tiêu
Tiếp tục phát triển hệ thống từ nền tảng OOP của Tuần 1, tập trung vào quy trình mua vé, quản lý vé đã phát hành và ngăn chặn bán vượt sức chứa.
2. Công việc đã thực hiện
+	TicketingService: Triển khai phương thức BuyTicket() để xử lý quy trình mua vé.
+	Quản lý vé: Bổ sung Dictionary<string, Ticket> trong lớp Event để lưu vé theo mã vé.
+	Kiểm soát sức chứa: Sử dụng TryReserveSeat() trong TicketTier để từ chối mua vé khi hạng vé đã hết.
+	Tạo mã vé: Sử dụng Guid để tạo mã vé gồm 8 ký tự chữ và số in hoa.
+	Demo: Xây dựng Week2.Demo để minh họa mua vé thành công và xử lý trường hợp hết vé.
3. Áp dụng OOP
+	Đóng gói: Quản lý trạng thái và số lượng vé thông qua thuộc tính, phương thức của lớp.
+	Kế thừa và đa hình: Tiếp tục sử dụng TicketTier, GeneralTier và VIPTier từ Tuần 1.
+	Phân chia trách nhiệm: Event quản lý thông tin sự kiện và vé đã phát hành; TicketTier quản lý sức chứa; TicketingService xử lý nghiệp vụ mua vé.
4. Hạn chế và kế hoạch tiếp theo
Hạn chế:
- Chưa có project unit test tự động riêng để kiểm thử các chức năng và trường hợp biên.
- Chưa kiểm thử trường hợp hạng vé không thuộc sự kiện đang mua vé.
- Quy trình check-in và các báo cáo doanh thu, tham dự chưa hoàn thiện trong phạm vi Tuần 2.

Kế hoạch Tuần 3:
- Xây dựng và chạy unit test tự động cho quy trình mua vé, kiểm soát sức chứa và các trường hợp lỗi.
- Hoàn thiện chức năng check-in và ngăn sử dụng lại vé đã check-in.
- Phát triển báo cáo doanh thu theo từng hạng vé và tổng doanh thu.
- Phát triển báo cáo tham dự, bao gồm số vé đã bán, số vé đã check-in và số vé chưa được sử dụng để check-in.
- Kiểm thử các chức năng mới và ghi nhận kết quả thực tế.
5. Sử dụng AI
ChatGPT được sử dụng để hỗ trợ tìm hiểu OOP và rà soát báo cáo. Nội dung được đối chiếu với mã nguồn và kết quả demo; các chức năng chưa hoàn thành được ghi rõ là hạn chế.

6. Liên kết mã nguồn và minh chứng
- Repository: https://github.com/trantrongnghia140307-source/EventTicketingSystem
  
- Mã nguồn Week 2: https://github.com/trantrongnghia140307-source/EventTicketingSystem/tree/main/Week2
  
- TicketingService.cs: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week2/EventTicketing.Services/TicketingService.cs
  
- Event.cs: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week2/EventTicketing.Domain/Event.cs
  
- TicketTier.cs: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week2/EventTicketing.Domain/TicketTier.cs

- Program.cs: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week2/Week2.Demo/Program.cs

- Commit minh chứng: https://github.com/trantrongnghia140307-source/EventTicketingSystem/commit/1a0838f09cd314ebd269d757d3b535b36420ca72

- Ảnh kết quả chạy demo: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week2/Week2_Demo_Result.png.jpg
7. Thành viên và phân công công việc

 Thành viên 1: Trần Trọng Nghĩa – 25110046
  - Tiếp tục hoàn thiện mô hình miền OOP từ Tuần 1, tập trung vào lớp Event và Ticket, quản lý danh sách vé đã phát hành bằng Dictionary<string, Ticket>.
  - Hoàn thiện cơ chế quản lý sức chứa trong TicketTier, sử dụng TryReserveSeat() để kiểm soát số lượng vé có thể bán.
  - Phối hợp rà soát tính nhất quán giữa mô hình miền và quy trình mua vé.
 Thành viên 2: Trần Việt Tiến – 25110064
  - Triển khai nghiệp vụ mua vé trong lớp TicketingService thông qua phương thức BuyTicket().
  - Xây dựng chức năng tạo mã vé bằng Guid, định dạng mã gồm 8 ký tự chữ và số in hoa.
  - Xây dựng và chạy chương trình Week2.Demo để minh họa mua vé thành công, ngăn bán vượt sức chứa và kiểm tra tính duy nhất của mã vé.
