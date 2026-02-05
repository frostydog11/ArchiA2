using A1_AutoDetail.App.Entities;
using Microsoft.EntityFrameworkCore;

namespace A1_AutoDetail.App.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<DetailService> DetailServices { get; set; } = null!;
    public DbSet<TimeSlot> TimeSlots { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().ToTable("Customer").HasKey(x => x.CustomerId);
        modelBuilder.Entity<DetailService>().ToTable("DetailService").HasKey(x => x.DetailServiceId);
        modelBuilder.Entity<TimeSlot>().ToTable("TimeSlot").HasKey(x => x.TimeSlotId);
        modelBuilder.Entity<Booking>().ToTable("Booking").HasKey(x => x.BookingId);

        modelBuilder.Entity<Booking>()
            .HasIndex(x => x.TimeSlotId)
            .IsUnique();

        // The 'Z' indicates UTC
        modelBuilder.Entity<TimeSlot>()
            .Property(x => x.StartTime)
            .HasConversion(
                v => v.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss'Z'"),
                v => DateTimeOffset.Parse(v).UtcDateTime);

        // The 'Z' indicates UTC
        modelBuilder.Entity<Booking>()
            .Property(x => x.CreatedAt)
            .HasConversion(
                v => v.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss'Z'"),
                v => DateTimeOffset.Parse(v).UtcDateTime);
    }
}
