using EventTicketing.Domain;
using EventTicketing.Services;

int passed = 0, failed = 0;

void Check(bool condition, string description)
{
    if (condition)
    {
        passed++;
        Console.WriteLine($"  [PASS] {description}");
    }
    else
    {
        failed++;
        Console.WriteLine($"  [FAIL] {description}");
    }
}

void Section(string title) => Console.WriteLine($"\n--- {title} ---");

Exception? Capture(Action action)
{
    try { action(); return null; }
    catch (Exception ex) { return ex; }
}

Event NewEvent(string id, int vipCapacity = 2, int generalCapacity = 10)
{
    var evt = new Event(id, "Monstercat Music Festival 2026");
    evt.AddTier(new VIPTier(1_000_000m, vipCapacity));
    evt.AddTier(new GeneralTier(500_000m, generalCapacity));
    return evt;
}

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("==================================================");
Console.WriteLine(" EVENT MANAGEMENT & TICKETING - WEEK 3 CHECKS");
Console.WriteLine("==================================================");

var ticketing = new TicketingService();
var checkIn = new CheckInService();
var reporting = new ReportingService();

Section("1. MUA VÉ VÀ CHECK-IN THÀNH CÔNG");
{
    var evt = NewEvent("E1");
    var vip = evt.FindTier("VIP Tier")!;
    var ticket = ticketing.BuyTicket(evt, vip);

    Check(ticket.PricePaid == 1_200_000m, "Giá VIP được tính đa hình (phụ thu 20%)");
    Check(vip.SoldQuantity == 1 && vip.Remaining == 1, "Tồn kho VIP giảm đúng một vé");
    Check(evt.IssuedTickets.ContainsKey(ticket.TicketCode), "Vé được lưu bằng mã trong Dictionary");
    Check(checkIn.CheckIn(evt, ticket.TicketCode), "Check-in bằng mã vé hợp lệ thành công");
    Check(ticket.IsCheckedIn && ticket.CheckedInAt != null, "Lưu trạng thái và thời điểm check-in");
}

Section("2. KIỂM TRA TRƯỜNG HỢP LỖI");
{
    var evt = NewEvent("E2");
    var vip = evt.FindTier("VIP Tier")!;
    var first = ticketing.BuyTicket(evt, vip);
    ticketing.BuyTicket(evt, vip);

    Check(Capture(() => ticketing.BuyTicket(evt, vip)) is InvalidOperationException,
        "Từ chối mua vượt sức chứa");
    Check(vip.SoldQuantity == 2 && evt.IssuedTickets.Count == 2,
        "Tồn kho và danh sách vé không đổi sau khi hết vé");
    Check(Capture(() => checkIn.CheckIn(evt, "NOTACODE")) is ArgumentException,
        "Từ chối mã vé không tồn tại");
    Check(Capture(() => checkIn.CheckIn(evt, "  ")) is ArgumentException,
        "Từ chối mã vé rỗng");

    checkIn.CheckIn(evt, first.TicketCode);
    var firstCheckInTime = first.CheckedInAt;
    Check(Capture(() => checkIn.CheckIn(evt, first.TicketCode)) is InvalidOperationException,
        "Từ chối check-in lần thứ hai");
    Check(first.CheckedInAt == firstCheckInTime, "Không ghi đè thời điểm check-in ban đầu");

    var generalTicket = ticketing.BuyTicket(evt, evt.FindTier("General Tier")!);
    Check(checkIn.CheckIn(evt, generalTicket.TicketCode.ToLowerInvariant()),
        "Chuẩn hóa mã chữ thường thành chữ hoa");

    var otherEvent = NewEvent("E2B");
    var foreignTier = otherEvent.FindTier("VIP Tier")!;
    Check(Capture(() => ticketing.BuyTicket(evt, foreignTier)) is ArgumentException,
        "Từ chối hạng vé thuộc sự kiện khác");

    var foreignTicket = ticketing.BuyTicket(otherEvent, foreignTier);
    Check(Capture(() => checkIn.CheckIn(evt, foreignTicket.TicketCode)) is ArgumentException,
        "Từ chối check-in vé thuộc sự kiện khác");

    Check(Capture(() => new VIPTier(-1m, 5)) is ArgumentException,
        "Từ chối giá vé âm");
    Check(Capture(() => new VIPTier(1000m, 0)) is ArgumentException,
        "Từ chối sức chứa bằng 0");
}

Section("3. KIỂM TRA BÁN VÉ ĐỒNG THỜI");
{
    var evt = NewEvent("E3", vipCapacity: 50, generalCapacity: 10);
    var vip = evt.FindTier("VIP Tier")!;
    int successCount = 0, unexpectedErrors = 0;
    using var start = new ManualResetEventSlim(false);

    var buyers = Enumerable.Range(0, 100).Select(_ => new Thread(() =>
    {
        start.Wait();
        for (int i = 0; i < 20; i++)
        {
            try
            {
                ticketing.BuyTicket(evt, vip);
                Interlocked.Increment(ref successCount);
            }
            catch (InvalidOperationException) { }
            catch (Exception) { Interlocked.Increment(ref unexpectedErrors); }
        }
    })).ToList();

    buyers.ForEach(thread => thread.Start());
    start.Set();
    buyers.ForEach(thread => thread.Join());

    Check(successCount == 50, "Chỉ bán đúng 50 vé khi nhiều luồng cùng mua");
    Check(vip.SoldQuantity == 50 && evt.IssuedTickets.Count == 50,
        "Số lượng đã bán khớp số vé lưu và sức chứa");
    Check(unexpectedErrors == 0, "Không phát sinh lỗi ngoài dự kiến");

    var ticket = evt.IssuedTickets.Values.First();
    int successfulCheckIns = 0;
    using var startScanners = new ManualResetEventSlim(false);

    var scanners = Enumerable.Range(0, 20).Select(_ => new Thread(() =>
    {
        startScanners.Wait();
        try
        {
            checkIn.CheckIn(evt, ticket.TicketCode);
            Interlocked.Increment(ref successfulCheckIns);
        }
        catch (ArgumentException) { }
        catch (InvalidOperationException) { }
    })).ToList();

    scanners.ForEach(thread => thread.Start());
    startScanners.Set();
    scanners.ForEach(thread => thread.Join());

    Check(successfulCheckIns == 1, "Nhiều luồng quét cùng mã nhưng chỉ một lần check-in thành công");
}

Section("4. BÁO CÁO DOANH THU VÀ THAM DỰ");
{
    var evt = NewEvent("E4", vipCapacity: 5, generalCapacity: 10);
    var vip = evt.FindTier("VIP Tier")!;
    var general = evt.FindTier("General Tier")!;

    var vip1 = ticketing.BuyTicket(evt, vip);
    ticketing.BuyTicket(evt, vip);
    var general1 = ticketing.BuyTicket(evt, general);

    checkIn.CheckIn(evt, vip1.TicketCode);
    checkIn.CheckIn(evt, general1.TicketCode);

    reporting.PrintSalesReport(evt);
    reporting.PrintAttendanceReport(evt);

    var sales = reporting.BuildSalesReport(evt);
    var attendance = reporting.BuildAttendanceReport(evt);

    Check(sales.TotalSold == 3, "Báo cáo doanh thu ghi nhận tổng cộng 3 vé");
    Check(sales.TotalRevenue == 2_900_000m, "Tổng doanh thu tính theo giá thực trả");
    Check(sales.Lines.First(line => line.TierName == "VIP Tier").Revenue == 2_400_000m,
        "Doanh thu VIP được tính riêng theo hạng");
    Check(attendance.TotalSold == 3 && attendance.CheckedIn == 2 && attendance.NoShow == 1,
        "Báo cáo tham dự đúng: 3 vé, 2 check-in, 1 chưa check-in");
    Check(attendance.AttendanceRate == 66.7, "Tỷ lệ tham dự được tính và làm tròn");
}

Console.WriteLine($"\n==== KẾT QUẢ: {passed} PASS, {failed} FAIL ====");
return failed == 0 ? 0 : 1;
