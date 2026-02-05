namespace A1_AutoDetail.App.Services;

public interface IBookingService
{
    BookingResult Book(int customerId, int detailServiceId, int timeSlotId, DateTime nowUtc);
}
