namespace EventTicketing.Domain;

public class Voucher
{
    public string Code { get; private set; }
    private decimal DiscountPercent { get; set; } 

    public Voucher(string code, decimal discountPercent)
    {
        Code = code;
        DiscountPercent = discountPercent;
    }

    
    public decimal ApplyDiscount(decimal originalPrice)
    {
        if (originalPrice <= 0) return 0;
        return originalPrice * (1 - DiscountPercent);
    }
}