using System;

namespace EventTicketing.Domain;

public class Ticket
{
    public string TicketCode { get; private set; }
    public TicketTier Tier { get; private set; }
    
    // Thuộc tính mới lưu giá tiền thực tế khách trả sau khi giảm giá (để làm báo cáo không bị sai)
    public decimal PricePaid { get; private set; } 
    
    public bool IsCheckedIn { get; private set; }
    public DateTime? CheckedInAt { get; private set; }

    // Constructor gộp duy nhất, nhận đủ mã vé, hạng vé và giá tiền
    public Ticket(string ticketCode, TicketTier tier, decimal pricePaid)
    {
        TicketCode = ticketCode;
        Tier = tier;
        PricePaid = pricePaid;
        IsCheckedIn = false;
    }

    public void MarkAsCheckedIn()
    {
        IsCheckedIn = true;
        CheckedInAt = DateTime.Now;
    }
}