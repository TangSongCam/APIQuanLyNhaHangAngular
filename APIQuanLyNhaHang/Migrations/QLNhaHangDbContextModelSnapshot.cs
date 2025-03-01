﻿// <auto-generated />
using System;
using APIQuanLyNhaHang.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APIQuanLyNhaHang.Migrations
{
    [DbContext(typeof(QLNhaHangDbContext))]
    partial class QLNhaHangDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("APIQuanLyNhaHang.Model.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("BookingTime")
                        .HasColumnType("time");

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfGuests")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TableId")
                        .HasColumnType("int");

                    b.Property<int?>("TableId1")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TableId");

                    b.HasIndex("TableId1");

                    b.ToTable("Bookings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BookingDate = new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            BookingTime = new TimeSpan(0, 18, 30, 0, 0),
                            CustomerName = "John Doe",
                            Notes = "No special requests",
                            NumberOfGuests = 2,
                            PhoneNumber = "123-456-7890",
                            TableId = 1
                        },
                        new
                        {
                            Id = 2,
                            BookingDate = new DateTime(2025, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            BookingTime = new TimeSpan(0, 19, 0, 0, 0),
                            CustomerName = "Jane Smith",
                            Notes = "Window seat preferred",
                            NumberOfGuests = 3,
                            PhoneNumber = "987-654-3210",
                            TableId = 2
                        });
                });

            modelBuilder.Entity("APIQuanLyNhaHang.Model.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("MenuItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BookingId = 1,
                            Category = "Hamburger",
                            Description = "Juicy cheeseburger with fresh ingredients",
                            ImagePath = "images/cheeseburger.jpg",
                            Name = "Cheeseburger",
                            Price = 5.99m
                        },
                        new
                        {
                            Id = 2,
                            BookingId = 1,
                            Category = "Fries",
                            Description = "Crispy golden fries",
                            ImagePath = "images/fries.jpg",
                            Name = "French Fries",
                            Price = 2.99m
                        },
                        new
                        {
                            Id = 3,
                            BookingId = 2,
                            Category = "Drink",
                            Description = "Refreshing beverage",
                            ImagePath = "images/softdrink.jpg",
                            Name = "Soft Drink",
                            Price = 1.50m
                        });
                });

            modelBuilder.Entity("APIQuanLyNhaHang.Model.Table", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tables");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Bàn 1",
                            Status = "Available"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Bàn 2",
                            Status = "Available"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Bàn 3",
                            Status = "Available"
                        });
                });

            modelBuilder.Entity("APIQuanLyNhaHang.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("APIQuanLyNhaHang.Model.Booking", b =>
                {
                    b.HasOne("APIQuanLyNhaHang.Model.Table", "Table")
                        .WithMany()
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APIQuanLyNhaHang.Model.Table", null)
                        .WithMany("Bookings")
                        .HasForeignKey("TableId1");

                    b.Navigation("Table");
                });

            modelBuilder.Entity("APIQuanLyNhaHang.Model.MenuItem", b =>
                {
                    b.HasOne("APIQuanLyNhaHang.Model.Booking", "Booking")
                        .WithMany("MenuItems")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("APIQuanLyNhaHang.Model.Booking", b =>
                {
                    b.Navigation("MenuItems");
                });

            modelBuilder.Entity("APIQuanLyNhaHang.Model.Table", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
