using EventTicketing.Domain;

namespace EventTicketing.Services;

public class TicketingService
{
    public Ticket BuyTicket(Event evt, TicketTier tier)
    {
        ArgumentNullException.ThrowIfNull(evt);
        ArgumentNullException.ThrowIfNull(tier);

        if (!evt.Tiers.Contains(tier))
            throw new ArgumentException("Hạng vé không thuộc sự kiện này!");

        decimal price = tier.CalculatePrice();

        // Kiểm tra tồn kho và phát hành vé trong cùng khóa để tránh bán vượt sức chứa.
        lock (evt.SyncRoot)
        {
            if (!tier.TryReserveSeat())
                throw new InvalidOperationException($"Hạng vé '{tier.TierName}' đã hết vé!");

            // Nếu mã trùng hiếm gặp, thử sinh mã khác.
            for (int attempt = 0; attempt < 5; attempt++)
            {
                string ticketCode = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
                var ticket = new Ticket(ticketCode, tier, price);

                if (evt.TryAddTicket(ticket))
                    return ticket;
            }

            tier.ReleaseSeat();
            throw new InvalidOperationException("Không thể sinh mã vé duy nhất, vui lòng thử lại!");
        }
    }
}
