using EventTicketing.Domain;

namespace EventTicketing.Services;

public class TicketingService
{
    public Ticket BuyTicket(Event evt, TicketTier tier, Voucher? voucher = null)
    {
        // Hạng vé phải thuộc đúng sự kiện này (tránh trừ tồn kho của sự kiện khác)
        if (!evt.Tiers.Contains(tier))
            throw new ArgumentException("Hạng vé không thuộc sự kiện này!");

        decimal price = tier.CalculatePrice();

        // Tính voucher ngay trong Service (Tránh God Class ở Controller)
        if (voucher != null)
        {
            price = voucher.ApplyDiscount(price);
        }

        // Bọc lock chống Race Condition (bán lố vé khi đông người mua)
        lock (evt.SyncRoot)
        {
            if (!tier.TryReserveSeat())
                throw new InvalidOperationException($"Hạng vé '{tier.TierName}' đã hết vé (Oversell prevented)!");

            // Mã vé 8 ký tự ngẫu nhiên; nếu lỡ trùng thì thử lại thay vì để chương trình văng lỗi
            for (int attempt = 0; attempt < 5; attempt++)
            {
                string ticketCode = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
                var ticket = new Ticket(ticketCode, tier, price);
                if (evt.TryAddTicket(ticket)) return ticket;
            }

            // Không sinh được mã duy nhất -> trả lại chỗ đã giữ, không để mất vé oan
            tier.ReleaseSeat();
            throw new InvalidOperationException("Không thể sinh mã vé duy nhất, vui lòng thử lại!");
        }
    }
}
