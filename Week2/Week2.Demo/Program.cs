using EventTicketing.Domain;
using EventTicketing.Services;

var evt = new Event("EVT001", "OOP Concert 2026");
var general = new GeneralTier(600000m, 2);
var vip = new VIPTier(1500000m, 1);
evt.AddTier(general);
evt.AddTier(vip);

var service = new TicketingService();

Console.WriteLine("=== WEEK 2 DEMO: TICKET PURCHASE ===");

// Happy path: buy one General ticket.
var t1 = service.BuyTicket(evt, general);
Console.WriteLine($"Mua vé thành công: {t1.TicketCode} | {t1.Tier.TierName}");
Console.WriteLine($"General inventory: {general.SoldQuantity}/{general.Capacity}");

// Buy the second General ticket.
var t2 = service.BuyTicket(evt, general);
Console.WriteLine($"Mua vé thành công: {t2.TicketCode} | {t2.Tier.TierName}");
Console.WriteLine($"General inventory: {general.SoldQuantity}/{general.Capacity}");

// Validation: a third purchase must be rejected because the tier is sold out.
try
{
    service.BuyTicket(evt, general);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Overselling blocked: {ex.Message}");
}

// Ticket codes are generated for each ticket; these two should be different.
Console.WriteLine($"Ticket codes unique: {t1.TicketCode != t2.TicketCode}");
