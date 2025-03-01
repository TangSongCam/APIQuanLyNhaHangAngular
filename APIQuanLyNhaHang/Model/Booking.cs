namespace APIQuanLyNhaHang.Model
{
    public class Booking
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan? BookingTime { get; set; }
        public int NumberOfGuests { get; set; }
        public string? Notes { get; set; }

        public int TableId { get; set; }

        public Table? Table { get; set; }
        public ICollection<MenuItem>? MenuItems { get; set; }
    }
}
