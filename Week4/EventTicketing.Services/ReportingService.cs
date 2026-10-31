using EventTicketing.Domain;

namespace EventTicketing.Services;

// Báo cáo giờ TRẢ VỀ DỮ LIỆU (record) thay vì chỉ in ra console -> web/test/console đều dùng lại được.
public record TierSalesLine(string TierName, int Sold, int Capacity, decimal Revenue);
public record SalesReport(string EventName, IReadOnlyList<TierSalesLine> Lines, int TotalSold, decimal TotalRevenue);
public record AttendanceReport(int TotalSold, int CheckedIn, int NoShow, double AttendanceRate);

public class ReportingService
{
    // Doanh thu = tổng GIÁ THỰC TRẢ của từng vé (đã tính voucher), không phải SoldQuantity x giá gốc
    public SalesReport BuildSalesReport(Event evt)
    {
        lock (evt.SyncRoot)
        {
            var lines = new List<TierSalesLine>();
            foreach (var tier in evt.Tiers)
            {
                var sold = evt.IssuedTickets.Values.Where(t => ReferenceEquals(t.Tier, tier)).ToList();
                lines.Add(new TierSalesLine(tier.TierName, sold.Count, tier.Capacity, sold.Sum(t => t.PricePaid)));
            }
            return new SalesReport(evt.EventName, lines, lines.Sum(l => l.Sold), lines.Sum(l => l.Revenue));
        }
    }

    public AttendanceReport BuildAttendanceReport(Event evt)
    {
        lock (evt.SyncRoot)
        {
            int total = evt.IssuedTickets.Count;
            int checkedIn = evt.IssuedTickets.Values.Count(t => t.IsCheckedIn);
            double rate = total == 0 ? 0 : Math.Round(checkedIn * 100.0 / total, 1);
            return new AttendanceReport(total, checkedIn, total - checkedIn, rate);
        }
    }

    // Bản in ra console (dùng cho Demo): chỉ việc định dạng lại dữ liệu ở trên
    public void PrintSalesReport(Event evt)
    {
        var r = BuildSalesReport(evt);
        Console.WriteLine($"=== BÁO CÁO DOANH THU SỰ KIỆN: {r.EventName} ===");
        foreach (var l in r.Lines)
            Console.WriteLine($"- Hạng vé: {l.TierName} | Đã bán: {l.Sold}/{l.Capacity} | Doanh thu: {l.Revenue:N0} VNĐ");
        Console.WriteLine($"-> TỔNG: {r.TotalSold} vé | DOANH THU: {r.TotalRevenue:N0} VNĐ\n");
    }

    public void PrintAttendanceReport(Event evt)
    {
        var r = BuildAttendanceReport(evt);
        Console.WriteLine("=== BÁO CÁO ĐIỂM DANH (ATTENDANCE) ===");
        Console.WriteLine($"- Tổng vé bán ra: {r.TotalSold}");
        Console.WriteLine($"- Đã Check-in: {r.CheckedIn}");
        Console.WriteLine($"- Vắng mặt (No-show): {r.NoShow}  (tỷ lệ tham dự {r.AttendanceRate}%)\n");
    }
}
