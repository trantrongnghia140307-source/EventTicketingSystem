namespace EventTicketing.Domain;

public class VIPTier : TicketTier
{
    public VIPTier(decimal basePrice, int capacity)
        : base("VIP Tier", basePrice, capacity) { }

    public override decimal CalculatePrice() => BasePrice * 1.2m;
    public override string GetPerks() => "Ghế hàng đầu, tặng đồ uống & lối đi riêng";
}
