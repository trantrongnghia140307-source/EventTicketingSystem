namespace EventTicketing.Domain;

public class Event
{
    private readonly List<TicketTier> _tiers = new();
    private readonly Dictionary<string, Ticket> _issuedTickets = new();

    // Dùng chung khóa cho các thao tác đọc/ghi liên quan đến vé của sự kiện.
    public object SyncRoot { get; } = new();

    public string EventId { get; private set; }
    public string EventName { get; private set; }

    public IReadOnlyList<TicketTier> Tiers => _tiers;
    public IReadOnlyDictionary<string, Ticket> IssuedTickets => _issuedTickets;

    public Event(string eventId, string eventName)
    {
        if (string.IsNullOrWhiteSpace(eventId))
            throw new ArgumentException("Mã sự kiện không được để trống!");
        if (string.IsNullOrWhiteSpace(eventName))
            throw new ArgumentException("Tên sự kiện không được để trống!");

        EventId = eventId;
        EventName = eventName;
    }

    public void AddTier(TicketTier tier)
    {
        ArgumentNullException.ThrowIfNull(tier);

        lock (SyncRoot)
        {
            if (_tiers.Any(t => t.TierName == tier.TierName))
                throw new ArgumentException($"Hạng vé '{tier.TierName}' đã có trong sự kiện!");

            _tiers.Add(tier);
        }
    }

    public TicketTier? FindTier(string tierName)
    {
        lock (SyncRoot)
        {
            return _tiers.FirstOrDefault(t => t.TierName == tierName);
        }
    }

    public bool HasTicket(string ticketCode)
    {
        lock (SyncRoot)
        {
            return _issuedTickets.ContainsKey(ticketCode);
        }
    }

    // false nếu mã vé đã tồn tại.
    public bool TryAddTicket(Ticket ticket)
    {
        lock (SyncRoot)
        {
            return _issuedTickets.TryAdd(ticket.TicketCode, ticket);
        }
    }
}
