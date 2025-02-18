using System.Collections.Generic;

namespace APIQuanLyNhaHang.Model
{
    public class Table
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public string? Name { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }

}
