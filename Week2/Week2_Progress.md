 BÁO CÁO TIẾN ĐỘ TUẦN 2
 Dự án: Event Management & Ticketing System

 1. Mục tiêu tuần 2

Tiếp tục phát triển hệ thống quản lý sự kiện và bán vé bằng C#, tập trung vào việc xây dựng quy trình mua vé, quản lý vé đã phát hành và kiểm soát số lượng vé còn lại. Đồng thời, vận dụng các nguyên lý lập trình hướng đối tượng (OOP) để tổ chức mã nguồn rõ ràng và dễ mở rộng.

 2. Công việc đã thực hiện

- Xây dựng lớp TicketingService trong dự án EventTicketing.Services để xử lý quy trình mua vé.
- Sử dụng Dictionary<string, Ticket> thông qua thuộc tính Event.IssuedTickets để quản lý các vé đã phát hành theo mã vé.
- Hoàn thiện phương thức TryReserveSeat() trong lớp TicketTier nhằm kiểm tra số lượng vé còn lại và ngăn việc bán vượt quá sức chứa của hạng vé.
- Sử dụng `Guid` để tạo mã vé, sau đó lấy một phần mã và chuyển thành chữ in hoa.
- Xây dựng chương trình minh họa trong Week2.Demo để thể hiện quy trình mua vé thành công và trường hợp từ chối mua khi hạng vé đã hết chỗ.

 3. Vận dụng các nguyên lý lập trình hướng đối tượng

Tính đóng gói (Encapsulation): Lớp TicketTier kiểm soát số lượng vé đã bán và việc đặt chỗ thông qua phương thức TryReserveSeat(), giúp bảo vệ quy tắc về sức chứa.

Tính kế thừa (Inheritance): Các lớp GeneralTier và VIPTier kế thừa từ lớp trừu tượng TicketTier, cho phép tổ chức các hạng vé dựa trên những đặc điểm chung.

Tính đa hình (Polymorphism): Các hạng vé có thể triển khai cách tính giá và cung cấp quyền lợi riêng thông qua việc ghi đè các phương thức tương ứng.

Mối quan hệ giữa các lớp: Lớp Event quản lý các hạng vé và danh sách vé đã phát hành; mỗi đối tượng Ticket tham chiếu đến hạng vé tương ứng.


 4. Kết quả đạt được

Đã xây dựng các thành phần chính phục vụ quy trình mua vé, quản lý vé đã phát hành và kiểm soát sức chứa. Mã nguồn được tổ chức thành các dự án EventTicketing.Domain, EventTicketing.Services và Week2.Demo để phân chia trách nhiệm giữa mô hình nghiệp vụ, dịch vụ xử lý và chương trình minh họa.

 5. Hạn chế và kế hoạch tuần tiếp theo

Trong giai đoạn tiếp theo, dự án sẽ tập trung vào:

- Xây dựng và kiểm tra chức năng quét vé, xác thực mã vé.
- Ngăn việc sử dụng một vé để check-in nhiều lần.
- Phát triển báo cáo doanh thu theo hạng vé và báo cáo tình trạng tham dự.
- Bổ sung các trường hợp kiểm thử cho mua vé thành công, hết vé, hạng vé không tồn tại và các quy tắc nghiệp vụ khác.
- Kiểm tra khả năng biên dịch và chạy toàn bộ dự án sau khi tích hợp các thành phần.

 6. Sử dụng công cụ AI

ChatGPT được sử dụng để hỗ trợ giải thích các nguyên lý OOP, gợi ý cấu trúc báo cáo và hướng dẫn thao tác đưa mã nguồn lên GitHub. Nội dung báo cáo cần được đối chiếu với mã nguồn thực tế; các kết quả chạy và kiểm thử chỉ được ghi nhận sau khi tự thực hiện và xác minh.
