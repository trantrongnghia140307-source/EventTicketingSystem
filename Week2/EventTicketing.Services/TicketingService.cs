using System;
using EventTicketing.Domain;

namespace EventTicketing.Services;

public class TicketingService
{
    // Logic mua vé
    public Ticket BuyTicket(Event evt, TicketTier tier)
    {
        // Gọi Encapsulation từ TicketTier để kiểm tra và giữ chỗ
        if (!tier.TryReserveSeat())
        {
            throw new InvalidOperationException($"Hạng vé '{tier.TierName}' đã hết vé (Oversell prevented)!");
        }

        // Tạo vé mới (sinh mã UUID)
        var ticket = new Ticket(tier);

        // Lưu vé vào Event (phục vụ tra cứu O(1))
        evt.IssuedTickets.Add(ticket.TicketCode, ticket);

        return ticket;
    }
}