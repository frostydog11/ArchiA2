namespace A1_AutoDetail.App.Domain.Entities;

public sealed class Booking
{
    public int BookingId { get; set; }
    public int CustomerId { get; set; }
    public int DetailServiceId { get; set; }
    public int TimeSlotId { get; set; }
    public DateTime CreatedAt { get; set; }
}
