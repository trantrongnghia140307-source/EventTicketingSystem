namespace EventTicketing.Domain;

public abstract class TicketTier
{
    // "Chìa khóa" của riêng hạng vé này: tại một thời điểm chỉ MỘT người (một request) cầm được chìa.
    // Người khác phải đợi bên ngoài cánh cửa lock { } cho tới khi người trước xong.
    private readonly object _lock = new();

    public string TierName { get; private set; }
    public decimal BasePrice { get; private set; }
    public int Capacity { get; private set; }
    public int SoldQuantity { get; private set; }   // Encapsulation: bên ngoài chỉ đọc, không sửa trực tiếp
    public int Remaining => Capacity - SoldQuantity;

    protected TicketTier(string tierName, decimal basePrice, int capacity)
    {
        if (string.IsNullOrWhiteSpace(tierName)) throw new ArgumentException("Tên hạng vé không được để trống!");
        if (basePrice < 0) throw new ArgumentException("Giá vé không được âm!");
        if (capacity <= 0) throw new ArgumentException("Sức chứa phải lớn hơn 0!");

        TierName = tierName;
        BasePrice = basePrice;
        Capacity = capacity;
        SoldQuantity = 0;
    }

    // Giữ chỗ 1 vé. Bước "kiểm tra còn chỗ" và bước "tăng số vé đã bán" phải nằm TRỌN trong một khóa,
    // nếu không 2 người có thể cùng thấy "còn 1 chỗ" rồi cùng giành chỗ đó -> bán quá (oversell).
    public bool TryReserveSeat()
    {
        lock (_lock)
        {
            if (SoldQuantity >= Capacity) return false;   // hết vé
            SoldQuantity++;
            return true;
        }
    }

    // Trả lại 1 chỗ (dùng khi đã giữ chỗ nhưng bước sau thất bại, ví dụ không sinh được mã vé)
    public void ReleaseSeat()
    {
        lock (_lock)
        {
            if (SoldQuantity > 0) SoldQuantity--;
        }
    }

    // Đa hình: mỗi hạng vé tự quyết định giá cuối và quyền lợi của nó
    public abstract decimal CalculatePrice();
    public abstract string GetPerks();
}
