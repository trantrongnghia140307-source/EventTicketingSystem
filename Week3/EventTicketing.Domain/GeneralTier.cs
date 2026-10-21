namespace EventTicketing.Domain;

public class GeneralTier : TicketTier
{
    public GeneralTier(decimal basePrice, int capacity)
        : base("General Tier", basePrice, capacity) { }

    public override decimal CalculatePrice() => BasePrice;
    public override string GetPerks() => "Vé vào cổng tiêu chuẩn";
}
