using Microsoft.AspNetCore.Mvc;
using EventTicketing.Domain;
using EventTicketing.Services;

namespace EventTicketing.Web.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    private static readonly Dictionary<string, Event> Events = new();
    private static readonly Dictionary<string, string> EventImages = new();

    // Dùng lớp trừu tượng Voucher để chứa được cả PercentVoucher và FixedAmountVoucher
    private static readonly Dictionary<string, Voucher> Vouchers = new();

    private static readonly TicketingService TicketingSvc = new();
    private static readonly CheckInService CheckInSvc = new();
    private static readonly ReportingService ReportingSvc = new();

    static ApiController()
    {
        // Khởi tạo 3 sự kiện
        var evt1 = new Event("EVT001", "TINH HÀ SAY HI - CONCERT 2026");
        evt1.AddTier(new VIPTier(basePrice: 1500000m, capacity: 5));
        evt1.AddTier(new GeneralTier(basePrice: 600000m, capacity: 10));
        Events.Add(evt1.EventId, evt1);
        EventImages.Add(evt1.EventId, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQOxGIvQmYtThoSeWgQhQ78IvtS66jW3K7oaEaFFlTC3u6fW6x5MyTbBhY&s=10");

        var evt2 = new Event("EVT002", "MONSTERCAT MUSIC FESTIVAL 2026");
        evt2.AddTier(new VIPTier(basePrice: 2000000m, capacity: 3));
        evt2.AddTier(new GeneralTier(basePrice: 800000m, capacity: 15));
        Events.Add(evt2.EventId, evt2);
        EventImages.Add(evt2.EventId, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQA_6yfCk4kj_SCbxc8a2v-y60LyUbiXeET9utaMqLTRQ&s=10");

        var evt3 = new Event("EVT003", "YOUTH FESTIVAL HCMUTE 2026");
        evt3.AddTier(new VIPTier(basePrice: 500000m, capacity: 10));
        evt3.AddTier(new GeneralTier(basePrice: 150000m, capacity: 30));
        Events.Add(evt3.EventId, evt3);
        EventImages.Add(evt3.EventId, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSM9wKYiXaj9MGq-L9OhZOanWMJufN8hT6ZjborTQ0qFB-bMefafn2RReMc&s=10");

        Vouchers.Add("HCMUTE10", new PercentVoucher("HCMUTE10", 0.10m));
        Vouchers.Add("TUTA20", new PercentVoucher("TUTA20", 0.20m));
        Vouchers.Add("TUTA50K", new FixedAmountVoucher("TUTA50K", 50000m));
    }

    [HttpGet("events")]
    public IActionResult GetEvents()
    {
        var result = Events.Values.Select(e => new
        {
            e.EventId,
            e.EventName,
            ImageUrl = EventImages.TryGetValue(e.EventId, out var url) ? url : "",
            Tiers = e.Tiers.Select(t => new
            {
                t.TierName,
                Price = t.CalculatePrice(),
                Perks = t.GetPerks(),
                t.Capacity,
                t.SoldQuantity,
                Available = t.Remaining
            })
        });

        return Ok(result);
    }

    [HttpPost("buy-ticket")]
    public IActionResult BuyTicket([FromBody] BuyRequest req)
    {
        if (!Events.TryGetValue(req.EventId, out var evt))
            return BadRequest(new { success = false, message = "Sự kiện không tồn tại!" });

        var tier = evt.FindTier(req.TierName);
        if (tier == null)
            return BadRequest(new { success = false, message = "Hạng vé không tồn tại!" });

        // Xử lý tìm Voucher (nếu có)
        Voucher? appliedVoucher = null;
        if (!string.IsNullOrWhiteSpace(req.VoucherCode))
        {
            string codeUpper = req.VoucherCode.Trim().ToUpperInvariant();
            if (!Vouchers.TryGetValue(codeUpper, out appliedVoucher))
                return BadRequest(new { success = false, message = $"Mã voucher '{req.VoucherCode.Trim()}' không hợp lệ!" });
        }

        try
        {
            Ticket ticket = TicketingSvc.BuyTicket(evt, tier, appliedVoucher);
            return Ok(new { success = true, ticketCode = ticket.TicketCode, price = ticket.PricePaid });
        }
        catch (InvalidOperationException ex) { return Conflict(new { success = false, message = ex.Message }); }   // hết vé
        catch (ArgumentException ex) { return BadRequest(new { success = false, message = ex.Message }); }
    }

    [HttpPost("checkin")]
    public IActionResult CheckIn([FromBody] CheckInRequest req)
    {
        // Chuẩn hóa mã ở server luôn (không tin hoàn toàn vào việc giao diện đã viết hoa)
        string code = (req.TicketCode ?? "").Trim().ToUpperInvariant();
        if (code.Length == 0)
            return BadRequest(new { success = false, message = "Vui lòng nhập mã vé!" });

        try
        {
            foreach (var evt in Events.Values)
            {
                if (evt.HasTicket(code))     // đọc Dictionary trong khóa
                {
                    CheckInSvc.CheckIn(evt, code);
                    return Ok(new { success = true, message = $"Vé '{code}' quét thành công tại sự kiện {evt.EventName}!" });
                }
            }
            return BadRequest(new { success = false, message = "Mã vé không tồn tại trên hệ thống!" });
        }
        catch (InvalidOperationException ex) { return Conflict(new { success = false, message = ex.Message }); }   // vé đã dùng
        catch (ArgumentException ex) { return BadRequest(new { success = false, message = ex.Message }); }
    }

    // Báo cáo bán hàng theo hạng + báo cáo điểm danh (yêu cầu số 5 của đề)
    [HttpGet("events/{eventId}/report")]
    public IActionResult GetReport(string eventId)
    {
        if (!Events.TryGetValue(eventId, out var evt))
            return NotFound(new { success = false, message = "Sự kiện không tồn tại!" });

        return Ok(new
        {
            success = true,
            sales = ReportingSvc.BuildSalesReport(evt),
            attendance = ReportingSvc.BuildAttendanceReport(evt)
        });
    }

    public record BuyRequest(string EventId, string TierName, string? VoucherCode);
    public record CheckInRequest(string TicketCode);
}
