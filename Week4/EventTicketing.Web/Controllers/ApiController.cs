using Microsoft.AspNetCore.Mvc;
using EventTicketing.Domain;
using EventTicketing.Services;

namespace EventTicketing.Web.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    private static readonly Dictionary<string, Event> Events = new();
    private static readonly Dictionary<string, Voucher> Vouchers = new();

    private static readonly TicketingService TicketingSvc = new();
    private static readonly CheckInService CheckInSvc = new();

    static ApiController()
    {
        var evt1 = new Event("EVT001", "ANH TRAI SAY HI - CONCERT 2026");
        evt1.AddTier(new VIPTier(basePrice: 1500000m, capacity: 5));
        evt1.AddTier(new GeneralTier(basePrice: 600000m, capacity: 10));
        Events.Add(evt1.EventId, evt1);

        var evt2 = new Event("EVT002", "MONSTERCAT MUSIC FESTIVAL 2026");
        evt2.AddTier(new VIPTier(basePrice: 2000000m, capacity: 3));
        evt2.AddTier(new GeneralTier(basePrice: 800000m, capacity: 15));
        Events.Add(evt2.EventId, evt2);

        // Voucher bảo mật đóng gói
        Vouchers.Add("HCMUTE10", new Voucher("HCMUTE10", 0.10m));
        Vouchers.Add("TUTA20", new Voucher("TUTA20", 0.20m));
    }

    [HttpGet("events")]
    public IActionResult GetEvents()
    {
        var result = Events.Values.Select(e => new
        {
            e.EventId,
            e.EventName,
            Tiers = e.Tiers.Select(t => new
            {
                t.TierName,
                Price = t.CalculatePrice(),
                Perks = t.GetPerks(),
                t.Capacity,
                t.SoldQuantity,
                Available = t.Capacity - t.SoldQuantity
            })
        });

        return Ok(result);
    }

    [HttpPost("buy-ticket")]
public IActionResult BuyTicket([FromBody] BuyRequest req)
{
    try
    {
        if (!Events.TryGetValue(req.EventId, out var evt))
            return BadRequest(new { success = false, message = "Sự kiện không tồn tại!" });

        var tier = evt.Tiers.FirstOrDefault(t => t.TierName == req.TierName);
        if (tier == null) 
            return BadRequest(new { success = false, message = "Hạng vé không tồn tại!" });

        decimal finalPrice = tier.CalculatePrice();

        // KIỂM TRA MÃ VOUCHER
        if (!string.IsNullOrWhiteSpace(req.VoucherCode))
        {
            string codeUpper = req.VoucherCode.Trim().ToUpper();
            if (Vouchers.TryGetValue(codeUpper, out var voucher))
            {
                finalPrice = voucher.ApplyDiscount(finalPrice);
            }
            else
            {
                // Nếu người dùng nhập voucher nhưng mã KHÔNG TỒN TẠI -> Báo lỗi ngay!
                return BadRequest(new { success = false, message = $"Mã voucher '{req.VoucherCode}' không hợp lệ hoặc đã hết hạn!" });
            }
        }

        // Voucher hợp lệ (hoặc không nhập) thì mới tiến hành xuất vé
        Ticket ticket = TicketingSvc.BuyTicket(evt, tier);

        return Ok(new 
        { 
            success = true, 
            ticketCode = ticket.TicketCode, 
            price = finalPrice 
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}

    [HttpPost("checkin")]
    public IActionResult CheckIn([FromBody] CheckInRequest req)
    {
        try
        {
            foreach (var evt in Events.Values)
            {
                if (evt.IssuedTickets.ContainsKey(req.TicketCode))
                {
                    CheckInSvc.CheckIn(evt, req.TicketCode);
                    return Ok(new { success = true, message = $"Vé '{req.TicketCode}' quét thành công!" });
                }
            }

            return BadRequest(new { success = false, message = "Mã vé không tồn tại!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    public record BuyRequest(string EventId, string TierName, string? VoucherCode);
    public record CheckInRequest(string TicketCode);
}