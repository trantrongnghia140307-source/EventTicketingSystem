using System;
using System.Linq;
using EventTicketing.Domain;

namespace EventTicketing.Services;

public class ReportingService
{
    public void PrintSalesReport(Event evt)
    {
        Console.WriteLine($"=== BÁO CÁO DOANH THU SỰ KIỆN: {evt.EventName} ===");
        decimal totalRevenue = 0;

        foreach (var tier in evt.Tiers)
        {
            decimal tierRevenue = tier.SoldQuantity * tier.CalculatePrice(); // Polymorphism
            totalRevenue += tierRevenue;
            Console.WriteLine($"- Hạng vé: {tier.TierName} | Đã bán: {tier.SoldQuantity}/{tier.Capacity} | Doanh thu: {tierRevenue:N0} VNĐ");
        }
        Console.WriteLine($"-> TỔNG DOANH THU: {totalRevenue:N0} VNĐ\n");
    }

    public void PrintAttendanceReport(Event evt)
    {
        int totalSold = evt.IssuedTickets.Count;
        int checkedInCount = evt.IssuedTickets.Values.Count(t => t.IsCheckedIn);
        int noShowCount = totalSold - checkedInCount;

        Console.WriteLine($"=== BÁO CÁO ĐIỂM DANH (ATTENDANCE) ===");
        Console.WriteLine($"- Tổng vé bán ra: {totalSold}");
        Console.WriteLine($"- Đã Check-in: {checkedInCount}");
        Console.WriteLine($"- Vắng mặt (No-show): {noShowCount}\n");
    }
}