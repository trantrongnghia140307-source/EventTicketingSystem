namespace EventTicketing.Domain;

// Lớp cha trừu tượng cho các hạng vé.
// Inventory được giữ bên trong class và chỉ thay đổi qua TryReserveSeat().
public abstract class TicketTier
{
    public string TierName { get; private set; }
    public decimal BasePrice { get; private set; }
    public int Capacity { get; private set; }
    public int SoldQuantity { get; private set; }

    protected TicketTier(string tierName, decimal basePrice, int capacity)
    {
        TierName = tierName;
        BasePrice = basePrice;
        Capacity = capacity;
        SoldQuantity = 0;
    }

    // Encapsulation: TicketTier tự bảo vệ inventory và không cho oversell.
    public bool TryReserveSeat()
    {
        if (SoldQuantity >= Capacity)
        {
            return false;
        }

        SoldQuantity++;
        return true;
    }

    // Polymorphism: từng hạng vé tự quyết định giá và quyền lợi.
    public abstract decimal CalculatePrice();
    public abstract string GetPerks();
}
