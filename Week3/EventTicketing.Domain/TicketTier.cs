namespace EventTicketing.Domain;

public abstract class TicketTier
{
    private readonly object _lock = new();

    public string TierName { get; private set; }
    public decimal BasePrice { get; private set; }
    public int Capacity { get; private set; }
    public int SoldQuantity { get; private set; }
    public int Remaining => Capacity - SoldQuantity;

    protected TicketTier(string tierName, decimal basePrice, int capacity)
    {
        if (string.IsNullOrWhiteSpace(tierName))
            throw new ArgumentException("Tên hạng vé không được để trống!");
        if (basePrice < 0)
            throw new ArgumentException("Giá vé không được âm!");
        if (capacity <= 0)
            throw new ArgumentException("Sức chứa phải lớn hơn 0!");

        TierName = tierName;
        BasePrice = basePrice;
        Capacity = capacity;
    }

    public bool TryReserveSeat()
    {
        lock (_lock)
        {
            if (SoldQuantity >= Capacity)
                return false;

            SoldQuantity++;
            return true;
        }
    }

    // Hoàn lại chỗ nếu bước phát hành vé thất bại.
    public void ReleaseSeat()
    {
        lock (_lock)
        {
            if (SoldQuantity > 0)
                SoldQuantity--;
        }
    }

    public abstract decimal CalculatePrice();
    public abstract string GetPerks();
}
