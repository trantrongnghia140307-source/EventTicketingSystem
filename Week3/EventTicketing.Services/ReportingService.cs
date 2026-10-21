using EventTicketing.Domain;

namespace EventTicketing.Services;

public record TierSalesLine(string TierName, int Sold, int Capacity, decimal Revenue);
public record SalesReport(string EventName, IReadOnlyList<TierSalesLine> Lines, int TotalSold, decimal TotalRevenue);
public record AttendanceReport(int TotalSold, int CheckedIn, int NoShow, double AttendanceRate);

public class ReportingService
{
    public SalesReport BuildSalesReport(Event evt)
    {
        lock (evt.SyncRoot)
        {
            var lines = new List<TierSalesLine>();

            foreach (var tier in evt.Tiers)
            {
                var soldTickets = evt.IssuedTickets.Values
                    .Where(ticket => ReferenceEquals(ticket.Tier, tier))
                    .ToList();

                lines.Add(new TierSalesLine(
                    tier.TierName,
                    soldTickets.Count,
                    tier.Capacity,
                    soldTickets.Sum(ticket => ticket.PricePaid)));
            }

            return new SalesReport(
                evt.EventName,
                lines,
                lines.Sum(line => line.Sold),
                lines.Sum(line => line.Revenue));
        }
    }

    public AttendanceReport BuildAttendanceReport(Event evt)
    {
        lock (evt.SyncRoot)
        {
            int total = evt.IssuedTickets.Count;
            int checkedIn = evt.IssuedTickets.Values.Count(ticket => ticket.IsCheckedIn);
            double rate = total == 0 ? 0 : Math.Round(checkedIn * 100.0 / total, 1);

            return new AttendanceReport(total, checkedIn, total - checkedIn, rate);
        }
    }

    public void PrintSalesReport(Event evt)
    {
        var report = BuildSalesReport(evt);
        Console.WriteLine($"=== BÁO CÁO DOANH THU: {report.EventName} ===");

        foreach (var line in report.Lines)
            Console.WriteLine($"- {line.TierName}: {line.Sold}/{line.Capacity} vé | {line.Revenue:N0} VNĐ");

        Console.WriteLine($"-> Tổng: {report.TotalSold} vé | Doanh thu: {report.TotalRevenue:N0} VNĐ\n");
    }

    public void PrintAttendanceReport(Event evt)
    {
        var report = BuildAttendanceReport(evt);
        Console.WriteLine("=== BÁO CÁO THAM DỰ ===");
        Console.WriteLine($"- Tổng vé đã bán: {report.TotalSold}");
        Console.WriteLine($"- Đã check-in: {report.CheckedIn}");
        Console.WriteLine($"- Chưa check-in: {report.NoShow}");
        Console.WriteLine($"- Tỷ lệ tham dự: {report.AttendanceRate}%\n");
    }
}
