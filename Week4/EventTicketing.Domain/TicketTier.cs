namespace EventTicketing.Domain
{
    public abstract class TicketTier
    {
        public string TierName { get; private set; }
        public decimal BasePrice { get; private set; }
        public int Capacity { get; private set; }
        public int SoldQuantity { get; private set; } // Encapsulation: Không cho bên ngoài sửa trực tiếp

        protected TicketTier(string tierName, decimal basePrice, int capacity)
        {
            TierName = tierName;
            BasePrice = basePrice;
            Capacity = capacity;
            SoldQuantity = 0;
        }

        // Logic tự bảo vệ inventory (Encapsulation - Chống oversell)
        public bool TryReserveSeat()
        {
            if (SoldQuantity >= Capacity)
            {
                return false; // Hết vé
            }
            SoldQuantity++;
            return true;
        }

        // Phương thức ảo để các class con override (Polymorphism)
        public abstract decimal CalculatePrice();
        public abstract string GetPerks();
    }
}