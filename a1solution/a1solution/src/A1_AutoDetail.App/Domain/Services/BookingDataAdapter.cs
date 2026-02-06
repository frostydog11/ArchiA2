using A1_AutoDetail.App.Persistence;
using A1_AutoDetail.App.UI_Models;
using Microsoft.EntityFrameworkCore;

namespace A1_AutoDetail.App.Services;

public sealed class BookingDataAdapter : IBookingDataAdapter
{
    private readonly AppDbContext _db;

    public BookingDataAdapter(AppDbContext db)
    {
        _db = db;
    }

    public List<CustomerOption> GetCustomerOptions()
    {
        return _db.Customers
            .OrderBy(c => c.FullName)
            .Select(c => new CustomerOption
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                IsBlocklisted = c.IsBlocklisted
            })
            .ToList();
    }


    public List<DetailServiceOption> GetDetailServiceOptions()
    {
        return _db.DetailServices
            .Select(d => new DetailServiceOption
            {
                DetailServiceId = d.DetailServiceId,
                DetailServiceName = d.DetailServiceName,
                IsPremium = d.IsPremium
            })
            .ToList();
    }

    public List<TimeSlotOption> GetTimeSlotOptions()
    {
        return _db.TimeSlots
            .OrderBy(t => t.StartTime)
            .Select(t => new TimeSlotOption
            {
                TimeSlotId = t.TimeSlotId,
                StartTimeText = t.StartTime.ToString("yyyy-MM-dd HH:mm")
            })
            .ToList();
    }


    public List<BookingListRow> GetCurrentBookings()
    {
        var q =
            from b in _db.Bookings.AsNoTracking()
            join c in _db.Customers.AsNoTracking() on b.CustomerId equals c.CustomerId
            join s in _db.DetailServices.AsNoTracking() on b.DetailServiceId equals s.DetailServiceId
            join t in _db.TimeSlots.AsNoTracking() on b.TimeSlotId equals t.TimeSlotId
            orderby t.StartTime
            select new BookingListRow
            {
                CustomerName = c.FullName,
                DetailServiceName = s.DetailServiceName,
                StartTimeText = t.StartTime.ToString("yyyy-MM-dd HH:mm"),
            };

        return q.ToList();
    }
}
