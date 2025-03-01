using APIQuanLyNhaHang.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace APIQuanLyNhaHang.Data
{
    public class QLNhaHangDbContext : DbContext
    {
        public QLNhaHangDbContext(DbContextOptions<QLNhaHangDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Table> Tables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table>().HasData(
                new Table { Id = 1, Name = "Bàn 1", Status = "Available" },
                new Table { Id = 2, Name = "Bàn 2", Status = "Available" },
                new Table { Id = 3, Name = "Bàn 3", Status = "Available" }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    CustomerName = "John Doe",
                    PhoneNumber = "123-456-7890",
                    BookingDate = new DateTime(2025, 1, 28),
                    BookingTime = new TimeSpan(18, 30, 0),
                    NumberOfGuests = 2,
                    Notes = "No special requests",
                    TableId = 1
                },
                new Booking
                {
                    Id = 2,
                    CustomerName = "Jane Smith",
                    PhoneNumber = "987-654-3210",
                    BookingDate = new DateTime(2025, 1, 29),
                    BookingTime = new TimeSpan(19, 0, 0),
                    NumberOfGuests = 3,
                    Notes = "Window seat preferred",
                    TableId = 2
                }
            );

            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    Id = 1,
                    Name = "Cheeseburger",
                    Description = "Juicy cheeseburger with fresh ingredients",
                    Price = 5.99m,
                    ImagePath = "images/cheeseburger.jpg",
                    Category = "Hamburger",
                    BookingId = 1
                },
                new MenuItem
                {
                    Id = 2,
                    Name = "French Fries",
                    Description = "Crispy golden fries",
                    Price = 2.99m,
                    ImagePath = "images/fries.jpg",
                    Category = "Fries",
                    BookingId = 1
                },
                new MenuItem
                {
                    Id = 3,
                    Name = "Soft Drink",
                    Description = "Refreshing beverage",
                    Price = 1.50m,
                    ImagePath = "images/softdrink.jpg",
                    Category = "Drink",
                    BookingId = 2
                }
            );

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Table)
                .WithMany()
                .HasForeignKey(b => b.TableId);



            modelBuilder.Entity<MenuItem>()
            .HasOne(m => m.Booking)
            .WithMany(b => b.MenuItems)
            .HasForeignKey(m => m.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
