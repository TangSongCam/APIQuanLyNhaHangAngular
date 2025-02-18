namespace APIQuanLyNhaHang.Model
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }  
        public string? Category { get; set; }
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }
    }
}
