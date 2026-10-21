namespace EventTicketing.Domain;

public class Ticket
{
    public string TicketCode { get; private set; }
    public TicketTier Tier { get; private set; }

    // Giá thực tế khách trả; ở mốc tuần 3 chưa áp dụng voucher.
    public decimal PricePaid { get; private set; }

    public bool IsCheckedIn { get; private set; }
    public DateTime? CheckedInAt { get; private set; }

    public Ticket(string ticketCode, TicketTier tier, decimal pricePaid)
    {
        if (string.IsNullOrWhiteSpace(ticketCode))
            throw new ArgumentException("Mã vé không được để trống!");
        ArgumentNullException.ThrowIfNull(tier);
        if (pricePaid < 0)
            throw new ArgumentException("Giá vé đã thanh toán không được âm!");

        TicketCode = ticketCode.Trim().ToUpperInvariant();
        Tier = tier;
        PricePaid = pricePaid;
        IsCheckedIn = false;
    }

    public void MarkAsCheckedIn()
    {
        if (IsCheckedIn)
            throw new InvalidOperationException("Vé đã được check-in!");

        IsCheckedIn = true;
        CheckedInAt = DateTime.Now;
    }
}
