# BÁO CÁO TIẾN ĐỘ TUẦN 4

## DỰ ÁN: EVENT MANAGEMENT & TICKETING SYSTEM


## 1. Mục tiêu thực hiện

Trong Tuần 4, nhóm em tiếp tục phát triển dự án Event Management & Ticketing System dựa trên nền tảng đã xây dựng trong ba tuần trước.
Ở Tuần 3, nhóm em đã hoàn thiện các nghiệp vụ mua vé, kiểm soát sức chứa, check-in, ngăn chặn check-in trùng lặp và báo cáo doanh thu, tình hình tham dự. Chương trình Demo của giai đoạn đó đạt 25 PASS, 0 FAIL.
Sang Tuần 4, nhóm em tập trung mở rộng và hoàn thiện hệ thống với các mục tiêu:

+ Bổ sung chức năng giảm giá bằng voucher với nhiều hình thức khác nhau.
+ Lưu số tiền thực tế khách hàng thanh toán cho từng vé.
+ Điều chỉnh cách tính doanh thu để phản ánh chính xác số tiền thực thu sau khi áp dụng voucher.
+ Xây dựng giao diện Web để người dùng có thể xem sự kiện, mua vé, nhập voucher và thực hiện check-in.
+ Kết nối giao diện Web với các API xử lý nghiệp vụ.
+ Mở rộng các trường hợp kiểm thử, đặc biệt là kiểm tra tính đúng đắn của dữ liệu và khả năng xử lý nhiều yêu cầu đồng thời.
Mục tiêu chung của nhóm em là hoàn thiện một phiên bản hệ thống có thể sử dụng thông qua giao diện Web, đồng thời tiếp tục áp dụng các nguyên tắc lập trình hướng đối tượng đã xây dựng từ Tuần 1.

## 2. Công việc đã thực hiện

### 2.1. Hoàn thiện mô hình quản lý sự kiện và vé

Nhóm em tiếp tục phát triển các lớp nghiệp vụ đã xây dựng từ những tuần trước và bổ sung một số thành phần:

 **Event:** Quản lý mã sự kiện, tên sự kiện, danh sách hạng vé và danh sách vé đã phát hành. Các danh sách được quản lý bên trong lớp và cung cấp giao diện chỉ đọc để hạn chế việc thay đổi dữ liệu trực tiếp từ bên ngoài.
 
 **TicketTier:** Là lớp trừu tượng quản lý tên hạng vé, giá cơ bản, sức chứa và số lượng vé đã bán. Các thao tác giữ chỗ và trả chỗ được bảo vệ bằng cơ chế đồng bộ.
 
 **GeneralTier và VIPTier:** Tiếp tục kế thừa từ `TicketTier`, mỗi loại hạng vé có cách tính giá và quyền lợi riêng.
 
 **Ticket:** Quản lý mã vé, hạng vé, giá thanh toán, trạng thái check-in và thời điểm check-in. Trong Tuần 4, nhóm em sử dụng thuộc tính `PricePaid` để lưu số tiền thực tế khách hàng đã thanh toán.
 
 **Voucher:** Bổ sung lớp trừu tượng để quản lý các loại mã giảm giá và xác định hành vi áp dụng giảm giá.

Việc bổ sung `PricePaid` giúp hệ thống lưu lại giá trị giao dịch thực tế của từng vé, thay vì chỉ dựa vào giá niêm yết ban đầu của hạng vé.

### 2.2. Xây dựng chức năng voucher

Nhóm em bổ sung hai loại voucher:

 **PercentVoucher:** Giảm giá theo tỷ lệ phần trăm trên giá vé.
 
 **FixedAmountVoucher:** Giảm trực tiếp một số tiền cố định trên giá vé.

Hai lớp này cùng kế thừa từ lớp trừu tượng `Voucher` và triển khai phương thức `ApplyDiscount()` theo cách riêng.
Hệ thống kiểm tra mã voucher và các giá trị giảm giá không hợp lệ. Giá vé sau khi giảm không được thấp hơn 0.

Các mã voucher mẫu được cấu hình trong ứng dụng Web:

 HCMUTE10:Giảm 10% 
 TUTA20: Giảm 20% 
 TUTA50K: Giảm 50.000 VNĐ 

Khi người dùng mua vé, `TicketingService` nhận voucher, tính giá sau giảm và lưu giá thực tế vào thuộc tính `PricePaid` của đối tượng `Ticket`.
Ví dụ: vé General Tier có giá 600.000 VNĐ. Khi sử dụng voucher HCMUTE10, số tiền khách hàng cần thanh toán là 540.000 VNĐ.

### 2.3. Hoàn thiện nghiệp vụ mua vé – TicketingService
Nhóm em tiếp tục sử dụng `TicketingService` để xử lý quy trình mua vé và bổ sung các bước kiểm tra:

- Kiểm tra hạng vé có thuộc đúng sự kiện đang mua hay không.
- Tính giá vé thông qua phương thức đa hình `CalculatePrice()`.
- Áp dụng voucher nếu người mua cung cấp mã hợp lệ.
- Kiểm tra sức chứa và giữ chỗ trước khi phát hành vé.
- Tạo mã vé gồm 8 ký tự chữ và số in hoa bằng `Guid`.
- Kiểm tra mã vé trùng và thử tạo lại nếu cần.
- Nếu không tạo được mã vé duy nhất sau số lần thử quy định, hệ thống trả lại chỗ đã giữ để tránh làm sai lệch số lượng vé còn lại.

Các thao tác mua vé và lưu vé đã phát hành được phối hợp với cơ chế khóa đồng bộ trong `Event`, nhằm hạn chế lỗi khi có nhiều người mua cùng lúc.

### 2.4. Duy trì và hoàn thiện chức năng check-in

Nhóm em tiếp tục sử dụng `CheckInService` đã xây dựng từ Tuần 3.
Chức năng này thực hiện các bước:

1. Kiểm tra mã vé đầu vào không được để trống.
2. Loại bỏ khoảng trắng ở hai đầu và chuyển mã vé thành chữ in hoa.
3. Tìm vé trong danh sách vé đã phát hành của sự kiện.
4. Kiểm tra vé đã được check-in trước đó hay chưa.
5. Nếu hợp lệ, cập nhật trạng thái `IsCheckedIn` và thời gian `CheckedInAt`.

Việc tìm vé, kiểm tra trạng thái và đánh dấu check-in được thực hiện trong cùng một vùng khóa. Vì vậy, khi nhiều người cùng quét một mã vé, hệ thống chỉ cho phép một lượt check-in thành công.


### 2.5. Điều chỉnh chức năng báo cáo doanh thu và điểm danh
Trong Tuần 4, nhóm em tiếp tục hoàn thiện `ReportingService` để sử dụng chung cho chương trình Demo và ứng dụng Web.
Điểm điều chỉnh quan trọng nằm ở cách tính doanh thu:

- Ở phiên bản Tuần 3, doanh thu trong kịch bản Demo được tính theo giá thực trả của các vé đã phát hành. Khi đó, kịch bản kiểm thử chưa có voucher nên tổng doanh thu là 2.900.000 VNĐ.
- Trong phiên bản Tuần 4, nhóm em bổ sung voucher và sử dụng `PricePaid` để ghi nhận số tiền thực tế của từng vé. Do đó, báo cáo doanh thu được tính bằng cách cộng `PricePaid` của các vé thuộc từng hạng vé.

Nhờ điều chỉnh này, những vé được mua bằng voucher cũng được ghi nhận đúng số tiền khách hàng đã thanh toán.
Nhóm em sử dụng các đối tượng dữ liệu báo cáo:

 `TierSalesLine`: Chứa tên hạng vé, số vé đã bán, sức chứa và doanh thu.
 `SalesReport`: Chứa tên sự kiện, danh sách doanh thu từng hạng vé, tổng số vé đã bán và tổng doanh thu.
 `AttendanceReport`: Chứa tổng vé đã bán, số vé đã check-in, số vé vắng mặt và tỷ lệ tham dự.

Báo cáo điểm danh tiếp tục được tính dựa trên trạng thái check-in thực tế của từng vé.

### 2.6. Xây dựng ứng dụng Web

Trong Tuần 4, nhóm em mở rộng dự án từ chương trình xử lý nghiệp vụ và Demo trên Console sang ứng dụng Web.
Do nhóm em chưa được học chuyên sâu về phát triển Web, đặc biệt là ASP.NET Core, HTML, CSS và JavaScript, nên trong quá trình thực hiện, nhóm em đã sử dụng các công cụ AI: ChatGPT và Claude, 1 số video hướng dẫn trên youTube để hỗ trợ tìm hiểu và triển khai phần Web.
Cụ thể, nhóm em nhờ AI hỗ trợ giải thích kiến trúc ứng dụng Web, giao diện ban đầu, đề xuất và hỗ trợ viết, điều chỉnh một số đoạn mã nguồn, cũng như hướng dẫn cách kết nối giao diện với các API xử lý nghiệp vụ.
Sau khi nhận được nội dung hỗ trợ, nhóm em đã cùng đọc, tìm hiểu, điều chỉnh mã nguồn cho phù hợp với cấu trúc dự án và kiểm tra lại bằng cách chạy ứng dụng, thử các chức năng thực tế. Nhóm em không xem các nội dung do AI cung cấp là kết quả chắc chắn đúng mà sử dụng như một công cụ hỗ trợ học tập và phát triển sản phẩm.

Phần ứng dụng Web gồm hai phần chính:

**a. Phần xử lý API – ApiController.cs**

- `GET /api/events`: Lấy danh sách sự kiện, thông tin hạng vé, giá vé và số lượng còn lại.
- `POST /api/buy-ticket`: Xử lý mua vé, kiểm tra voucher và trả về mã vé cùng giá thanh toán thực tế.
- `POST /api/checkin`: Xử lý check-in bằng mã vé, từ chối mã không tồn tại hoặc vé đã được sử dụng.
- `GET /api/events/{eventId}/report`: Trả về báo cáo doanh thu và điểm danh của sự kiện.

Các sự kiện và voucher mẫu được khởi tạo trong ứng dụng. Dữ liệu hiện được lưu trong bộ nhớ của chương trình, chưa sử dụng cơ sở dữ liệu.

**b. Phần giao diện – index.html**

Giao diện hỗ trợ các thao tác:

1. Hiển thị và lựa chọn sự kiện.
2. Hiển thị các hạng vé, giá vé và số lượng còn lại.
3. Nhập mã voucher khi mua vé.
4. Hiển thị thông báo mua vé thành công cùng mã vé và giá thanh toán.
5. Nhập mã vé để thực hiện check-in.
6. Hiển thị kết quả check-in thành công hoặc thông báo từ chối.
7. Hiển thị báo cáo doanh thu và điểm danh.

Phần giao diện gửi yêu cầu đến các API trong `ApiController.cs` để xử lý nghiệp vụ. Các quy tắc mua vé, voucher, check-in và báo cáo vẫn được xử lý thông qua các lớp Domain và Services, thay vì đặt toàn bộ nghiệp vụ trực tiếp trong giao diện.

## 3. Áp dụng kiến thức lập trình hướng đối tượng

Trong Tuần 4, nhóm em tiếp tục vận dụng các đặc điểm của lập trình hướng đối tượng:

### 3.1. Tính đóng gói (Encapsulation)

- Các thuộc tính như `SoldQuantity`, `PricePaid`, `IsCheckedIn` và `CheckedInAt` được giới hạn quyền thay đổi thông qua `private set`.
- Việc giữ chỗ, trả chỗ và cập nhật trạng thái check-in được thực hiện thông qua phương thức của lớp.
- `Event` quản lý danh sách hạng vé và vé đã phát hành, không cho phép bên ngoài thay đổi trực tiếp các collection thông qua thuộc tính chỉ đọc.

### 3.2. Tính kế thừa (Inheritance)

 `GeneralTier` và `VIPTier` kế thừa từ `TicketTier`.
 `PercentVoucher` và `FixedAmountVoucher` kế thừa từ `Voucher`.
- Trong chương trình Demo, nhóm em bổ sung `StudentTier` để minh họa khả năng mở rộng thêm một loại hạng vé mới mà không cần sửa các lớp Domain và Services hiện có.

### 3.3. Tính đa hình (Polymorphism)

- Các loại hạng vé triển khai riêng phương thức `CalculatePrice()` và `GetPerks()`.
- Các loại voucher triển khai riêng phương thức `ApplyDiscount()`.
- Khi xử lý thông qua biến kiểu lớp cha `TicketTier` hoặc `Voucher`, chương trình vẫn thực hiện đúng hành vi của lớp con tương ứng.

### 3.4. Tính trừu tượng (Abstraction)

 `TicketTier` xác định cấu trúc chung của các hạng vé nhưng không tự quyết định giá và quyền lợi cụ thể.
 `Voucher` xác định cách sử dụng chung của voucher nhưng giao việc tính mức giảm giá cho từng lớp con.

### 3.5. Phân chia trách nhiệm giữa các lớp

Nhóm em tiếp tục tách các phần nghiệp vụ thành những lớp riêng:

 `Event` quản lý thông tin sự kiện và vé đã phát hành.
 `TicketTier` quản lý giá vé, sức chứa và số lượng đã bán.
 `TicketingService` xử lý nghiệp vụ mua vé.
 `CheckInService` xử lý nghiệp vụ check-in.
 `ReportingService` tổng hợp dữ liệu doanh thu và điểm danh.
 `ApiController` tiếp nhận yêu cầu từ giao diện Web và gọi các service tương ứng.

Cách tổ chức này giúp các thành phần có trách nhiệm rõ ràng và thuận tiện hơn khi kiểm tra, bảo trì và mở rộng hệ thống.

## 4. Kết quả kiểm tra và minh chứng

Sau khi hoàn thiện mã nguồn, nhóm em sử dụng chương trình Demo để kiểm tra các chức năng thông qua 5 nhóm kiểm thử:

- **Test 1 – Mua vé và check-in:** Kiểm tra mua vé VIP thành công, tính giá bằng đa hình, cập nhật số lượng vé và thực hiện check-in bằng mã vé hợp lệ.
- **Test 2 – Dữ liệu không hợp lệ:** Kiểm tra các trường hợp hết vé, mã vé sai hoặc rỗng, check-in trùng lặp, sử dụng hạng vé không thuộc sự kiện và dữ liệu giá vé, sức chứa không hợp lệ.
- **Test 3 – Kế thừa, đa hình và voucher:** Kiểm tra `StudentTier`, `GeneralTier`, `VIP Tier`, hai loại voucher giảm giá và việc lưu giá thực tế đã thanh toán vào `PricePaid`.
- **Test 4 – Xử lý đồng thời:** Mô phỏng 100 người cùng mua 50 vé VIP (mỗi người thử tối đa 20 lần) và 20 người cùng check-in một mã vé. Kết quả không vượt quá sức chứa và mỗi vé chỉ được check-in thành công một lần.
- **Test 5 – Báo cáo doanh thu và điểm danh:** Kiểm tra kết quả báo cáo với dữ liệu Demo:


VIP Tier: 2 vé - 2.280.000 VNĐ 

General Tier: 1 vé - 500.000 VNĐ 


 **Tổng cộng**  **3**  **2.780.000 VNĐ** 
 
Trong đó, hai vé VIP có giá thực trả lần lượt là 1.200.000 VNĐ và 1.080.000 VNĐ sau khi áp dụng voucher giảm 10%. Kết quả điểm danh gồm 2 vé đã check-in, 1 vé vắng mặt, tỷ lệ tham dự đạt 66,7%.

### 4.1. Tổng kết kết quả kiểm thử

Chương trình Demo hoàn thành **31 PASS, 0 FAIL**, cho thấy cả 5 nhóm kiểm thử đều đạt kết quả mong đợi của nhóm em tại thời điểm chạy. Ngoài ra, nhóm em đã chạy ứng dụng Web và kiểm tra các chức năng mua vé, sử dụng voucher, check-in và hiển thị báo cáo trên giao diện.



## 5. Sử dụng AI trong quá trình thực hiện

Trong quá trình thực hiện Tuần 4, nhóm em có sử dụng ChatGPT, Claude, Youtube, Google làm công cụ hỗ trợ học tập và phát triển dự án.

Do nhóm em chưa được học chuyên sâu về lập trình Web và ASP.NET Core, nhóm đã sử dụng AI nhiều hơn ở phần xây dựng ứng dụng Web, bao gồm:

- Hỗ trợ tìm hiểu cấu trúc một ứng dụng ASP.NET Core Web API.
- Hỗ trợ xây dựng và điều chỉnh `Program.cs`, `ApiController.cs`.
- Hỗ trợ thiết kế giao diện HTML, CSS và JavaScript trong `wwwroot/index.html`.
- Hỗ trợ kết nối giao diện với các API mua vé, check-in và báo cáo.
- Giải thích lỗi, gợi ý cách kiểm tra và điều chỉnh mã nguồn khi chạy chương trình.

Ngoài phần Web, nhóm em cũng sử dụng AI để giải thích một số kiến thức OOP
Nhóm em không xem nội dung do AI tạo ra là kết quả đã được kiểm chứng. Sau khi nhận hỗ trợ, nhóm chúng em đã đọc lại mã nguồn, đối chiếu với cấu trúc dự án, chạy chương trình Demo và kiểm tra trực tiếp trên giao diện Web.
Đối với phần nghiệp vụ, nhóm em chú trọng kiểm tra các chức năng quan trọng như kiểm soát sức chứa, mã vé duy nhất, áp dụng voucher, lưu giá thực trả, chống check-in trùng lặp và tính doanh thu.

Nhóm em xin trình bày trung thực rằng AI đã hỗ trợ đáng kể trong việc tìm hiểu và triển khai phần Web do đây là kiến thức nhóm chúng em chưa được học chuyên sâu. Tuy nhiên, nhóm em vẫn trực tiếp kiểm tra, chạy thử, đối chiếu kết quả

## 6. Liên kết mã nguồn và minh chứng

- **Repository:** https://github.com/trantrongnghia140307-source/EventTicketingSystem
- **Mã nguồn Week 4:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/tree/main/Week4
- **Event.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Domain/Event.cs
- **Ticket.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Domain/Ticket.cs
- **TicketTier.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Domain/TicketTier.cs
- **TicketingService.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Services/TicketingService.cs
- **CheckInService.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Services/CheckInService.cs
- **ReportingService.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Services/ReportingService.cs
- **Program.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Web/Program.cs
- **VIPTier.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Domain/VIPTier.cs
- **GeneralTier.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Domain/GeneralTier.cs
- **Voucher.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Domain/Voucher.cs
- **ApiController.cs:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Web/Controllers/ApiController.cs
- **index.html:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketing.Web/wwwroot/index.html
- **EventTicketingSystem.slnx:** https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/EventTicketingSystem.slnx
  
- **Ảnh kết quả chạy chương trình:**
  1. Giao diện Web: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/Screenshot%20evidence/Week4_01_Giao_Dien_Web.png.jpg
  2. Mua vé thành công: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/Screenshot%20evidence/Week4_02_Mua_Ve_Thanh_Cong.png.jpg
  3. Voucher hợp lệ: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/Screenshot%20evidence/Week4_03_Voucher_Hop_Le.png.jpg
  4. Voucher không hợp lệ: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/Screenshot%20evidence/Week4_04_Voucher_Khong_Hop_Le.png.jpg
  5. Check in thành công: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/Screenshot%20evidence/Week4_05_CheckIn_Thanh_Cong.png.jpg
  6. Check in trùng lặp: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/Screenshot%20evidence/Week4_06_CheckIn_Trung_Lap.png.jpg
  7. Báo cáo doanh thu điểm danh: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/Screenshot%20evidence/Week4_07_Bao_Cao_Doanh_Thu_Diem_Danh.png.jpg
  8. Demo kết quả kiểm thử 1: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/Screenshot%20evidence/Week4_08_Demo_Ket_Qua_Kiem_Thu%201.png.jpg
  9. Demo kết quả kiểm thử 2: https://github.com/trantrongnghia140307-source/EventTicketingSystem/blob/main/Week4/Screenshot%20evidence/Week4_08_Demo_Ket_Qua_Kiem_Thu%202%20.png.jpg
- **Commit minh chứng Tuần 4:**
  https://github.com/trantrongnghia140307-source/EventTicketingSystem/commit/bf00f10d84eb254dff72c0058660c8406c649a5a
  https://github.com/trantrongnghia140307-source/EventTicketingSystem/commit/a71dfddf9448141858a387e277bf1e666f14fc2a
  


## 7. Thành viên và phân công công việc

### Thành viên 1: Trần Trọng Nghĩa – 25110046

- Tiếp tục hoàn thiện mô hình Domain gồm `Event`, `Ticket`, `TicketTier`, `GeneralTier`, `VIPTier` và `Voucher`.
- Bổ sung thuộc tính `PricePaid` để lưu giá thực tế của vé sau khi áp dụng voucher.
- Phát triển và hoàn thiện nghiệp vụ mua vé trong `TicketingService`.
- Xử lý kiểm tra hạng vé thuộc sự kiện, áp dụng voucher, giữ chỗ và tạo mã vé duy nhất.
- Phối hợp kiểm thử các trường hợp mua vé, voucher và kiểm soát sức chứa.

### Thành viên 2: Trần Việt Tiến – 25110064

- Tiếp tục hoàn thiện nghiệp vụ check-in trong `CheckInService`, bao gồm kiểm tra mã vé, chuẩn hóa mã và ngăn check-in trùng lặp.
- Phát triển và hoàn thiện `ReportingService` để tổng hợp doanh thu theo hạng vé và thống kê điểm danh.
- Tham gia xây dựng và kiểm tra ứng dụng Web, bao gồm `ApiController.cs` và giao diện `index.html`, với sự hỗ trợ của AI.
- Tham gia xây dựng và kiểm thử các kịch bản trong `EventTicketing.Demo`, phối hợp kiểm tra kết quả mua vé, voucher, check-in, xử lý đồng thời và báo cáo.

### Tổng kết
Nhóm em xin chân thành cảm ơn thầy đã hướng dẫn và góp ý trong quá trình thực hiện dự án. Qua bốn tuần, nhóm em đã có cơ hội vận dụng kiến thức của môn lập trình hướng đối tượng (OOP) vào một sản phẩm cụ thể, hiểu rõ hơn cách tổ chức các lớp, phân chia trách nhiệm, kiểm soát dữ liệu và mở rộng ứng dụng từ Console sang Web.
