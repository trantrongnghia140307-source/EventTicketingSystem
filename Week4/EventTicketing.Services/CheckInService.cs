using System.Globalization;
using EventTicketing.Domain;

namespace EventTicketing.Services;

public class CheckInService
{
    // Check-in: tra cứu O(1) qua Dictionary. Ba bước "tìm vé -> kiểm tra đã dùng chưa -> đánh dấu"
    // nằm trong một khóa, nên 2 người quét cùng một mã cùng lúc thì chỉ một người thành công.
    public bool CheckIn(Event evt, string ticketCode)
    {
        if (string.IsNullOrWhiteSpace(ticketCode))
            throw new ArgumentException("Vui lòng nhập mã vé!");

        // Chuẩn hóa: bỏ khoảng trắng, đổi thành chữ HOA (nhập "a1b2c3d4" vẫn đúng)
        string code = ticketCode.Trim().ToUpperInvariant();

        lock (evt.SyncRoot)
        {
            // 1. Vé có tồn tại trong sự kiện này không? (O(1))
            if (!evt.IssuedTickets.TryGetValue(code, out var ticket))
                throw new ArgumentException("Mã vé không tồn tại trong sự kiện này!");

            // 2. Chống Double Check-in
            if (ticket.IsCheckedIn)
            {
                string when = ticket.CheckedInAt?.ToString("HH:mm:ss dd/MM/yyyy", CultureInfo.InvariantCulture) ?? "?";
                throw new InvalidOperationException($"Vé '{code}' đã được quét check-in trước đó vào lúc {when}!");
            }

            // 3. Đánh dấu đã check-in
            ticket.MarkAsCheckedIn();
            return true;
        }
    }
}
