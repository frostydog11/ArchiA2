using A1_AutoDetail.App.Domain.Entities;
using A1_AutoDetail.App.Persistence;
using A1_AutoDetail.App.Persistence.Dao;
using Microsoft.EntityFrameworkCore;

namespace A1_AutoDetail.App.Services;

public sealed class BookingService : IBookingService
{
    private readonly BookingDao _dao;

    public BookingService(BookingDao dao)
    {
        _dao = dao;
    }

    public BookingResult Book(int customerId, int detailServiceId, int timeSlotId, DateTime nowUtc)
    {
        var customer = _dao.FindCustomer(customerId);
        if (customer == null)
            return BookingFailed(BookingOutcome.CustomerNotFound, "Customer not found.");

        if (customer.IsBlocklisted)
            return BookingFailed(BookingOutcome.CustomerBlocklisted, "Customer is blocklisted.");

        var service = _dao.FindDetailService(detailServiceId);
        if (service == null)
            return BookingFailed(BookingOutcome.ServiceNotFound, "Service not found.");

        var slot = _dao.FindTimeSlot(timeSlotId);
        if (slot == null)
            return BookingFailed(BookingOutcome.TimeSlotNotFound, "Time slot not found.");

        var slotDate = DateOnly.FromDateTime(slot.StartTime);

        if (_dao.HasCustomerBookedOnDate(customerId, slotDate))
            return BookingFailed(BookingOutcome.CustomerAlreadyBookedThatDay, "Customer already has a booking for that day.");

        if (_dao.IsTimeSlotBooked(timeSlotId))
            return BookingFailed(BookingOutcome.TimeSlotUnavailable, "Time slot is unavailable.");

        if (!ServiceHasSufficientNotice(service, slot, nowUtc))
            return BookingFailed(BookingOutcome.TooSoonForService, "That service requires more notice.");
        
        var bookingId = AddBooking(customerId, detailServiceId, timeSlotId, nowUtc);
        return BookingSucceeded(BookingOutcome.Success, "Booking created.", bookingId); 
    }

    private int AddBooking(int customerId, int detailServiceId, int timeSlotId, DateTime nowUtc) // dao
    {
        var booking = new Booking();
        booking.CustomerId = customerId;
        booking.DetailServiceId = detailServiceId;
        booking.TimeSlotId = timeSlotId;
        booking.CreatedAt = nowUtc;

        return _dao.AddBooking(booking);
    }

    private BookingResult BookingSucceeded(BookingOutcome outcome, string message, int bookingId)
    {
        var r = new BookingResult();
        r.Outcome = outcome;
        r.Message = message;
        r.BookingId = bookingId;
        return r;
    }

    private BookingResult BookingFailed(BookingOutcome outcome, string message)
    {
        var r = new BookingResult();
        r.Outcome = outcome;
        r.Message = message;
        r.BookingId = null;
        return r;
    }

    //private bool IsTimeSlotAlreadyBooked(int timeSlotId) // dao
    //{
    //    var q =
    //        from b in _db.Bookings.AsNoTracking()
    //        where b.TimeSlotId == timeSlotId
    //        select b.BookingId;

    //    return q.Any();
    //}

    //private bool HasCustomerBookedOnDate(int customerId, DateOnly date) //dao
    //{
    //    var start = date.ToDateTime(TimeOnly.MinValue);
    //    var end = date.AddDays(1).ToDateTime(TimeOnly.MinValue);

    //    var q =
    //        from b in _db.Bookings.AsNoTracking()
    //        join ts in _db.TimeSlots.AsNoTracking() on b.TimeSlotId equals ts.TimeSlotId
    //        where b.CustomerId == customerId
    //           && ts.StartTime >= start
    //           && ts.StartTime < end
    //        select b.BookingId;

    //    return q.Any();
    //}

    private bool ServiceHasSufficientNotice(DetailService service, TimeSlot slot, DateTime currentUtc)
    {
        var hoursRequired = service.IsPremium ? 48 : 24;
        var minTime = currentUtc.AddHours(hoursRequired);

        return slot.StartTime >= minTime;
    }

}
