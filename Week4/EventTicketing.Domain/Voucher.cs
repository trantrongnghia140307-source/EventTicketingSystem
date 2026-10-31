namespace EventTicketing.Domain;

// Lớp trừu tượng: mọi loại voucher đều có Mã và biết cách giảm giá của riêng nó (đa hình).
public abstract class Voucher
{
    public string Code { get; private set; }

    protected Voucher(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Mã voucher không được để trống!");
        Code = code.Trim().ToUpperInvariant();
    }

    public abstract decimal ApplyDiscount(decimal originalPrice);
}

// Giảm theo phần trăm. discountFraction = 0.10m nghĩa là giảm 10%.
public class PercentVoucher : Voucher
{
    private readonly decimal _fraction;

    public PercentVoucher(string code, decimal discountFraction) : base(code)
    {
        if (discountFraction <= 0 || discountFraction > 1)
            throw new ArgumentException("Phần trăm giảm phải trong khoảng (0, 1]!");
        _fraction = discountFraction;
    }

    public override decimal ApplyDiscount(decimal originalPrice)
        => Math.Max(0, Math.Round(originalPrice * (1 - _fraction), 0, MidpointRounding.AwayFromZero));
}

// Giảm thẳng một số tiền cố định (VNĐ), giá không bao giờ âm.
public class FixedAmountVoucher : Voucher
{
    private readonly decimal _amount;

    public FixedAmountVoucher(string code, decimal amount) : base(code)
    {
        if (amount <= 0) throw new ArgumentException("Số tiền giảm phải lớn hơn 0!");
        _amount = amount;
    }

    public override decimal ApplyDiscount(decimal originalPrice) => Math.Max(0, originalPrice - _amount);
}
