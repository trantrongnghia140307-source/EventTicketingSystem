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
Chưa có project unit test tự động riêng; chưa kiểm thử trường hợp hạng vé không thuộc sự kiện. Quy trình check-in và các báo cáo doanh thu, tham dự cũng chưa hoàn thiện.
Kế hoạch tiếp theo là bổ sung kiểm thử, hoàn thiện check-in, ngăn sử dụng vé nhiều lần và phát triển các chức năng báo cáo.
5. Sử dụng AI
ChatGPT được sử dụng để hỗ trợ tìm hiểu OOP và rà soát báo cáo. Nội dung được đối chiếu với mã nguồn và kết quả demo; các chức năng chưa hoàn thành được ghi rõ là hạn chế.

