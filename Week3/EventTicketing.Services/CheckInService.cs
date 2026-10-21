using System.Globalization;
using EventTicketing.Domain;

namespace EventTicketing.Services;

public class CheckInService
{
    public bool CheckIn(Event evt, string ticketCode)
    {
        ArgumentNullException.ThrowIfNull(evt);

        if (string.IsNullOrWhiteSpace(ticketCode))
            throw new ArgumentException("Vui lòng nhập mã vé!");

        string code = ticketCode.Trim().ToUpperInvariant();

        lock (evt.SyncRoot)
        {
            if (!evt.IssuedTickets.TryGetValue(code, out var ticket))
                throw new ArgumentException("Mã vé không tồn tại trong sự kiện này!");

            if (ticket.IsCheckedIn)
            {
                string when = ticket.CheckedInAt?.ToString("HH:mm:ss dd/MM/yyyy", CultureInfo.InvariantCulture) ?? "?";
                throw new InvalidOperationException($"Vé '{code}' đã được check-in trước đó vào lúc {when}!");
            }

            ticket.MarkAsCheckedIn();
            return true;
        }
    }
}
