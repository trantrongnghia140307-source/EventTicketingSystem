using System;
using EventTicketing.Domain;

namespace EventTicketing.Services;

public class CheckInService
{
    // Logic Check-in tra cứu O(1) qua Dictionary
    public bool CheckIn(Event evt, string ticketCode)
    {
        // 1. Kiểm tra vé có tồn tại trong hệ thống không (Tra cứu O(1))
        if (!evt.IssuedTickets.TryGetValue(ticketCode, out var ticket))
        {
            throw new ArgumentException("Mã vé không tồn tại trên hệ thống!");
        }

        // 2. Chống Double Check-in
        if (ticket.IsCheckedIn)
        {
            throw new InvalidOperationException($"Vé '{ticketCode}' đã được quét check-in trước đó vào lúc {ticket.CheckedInAt}!");
        }

        // 3. Đánh dấu đã check-in
        ticket.MarkAsCheckedIn();
        return true;
    }
}