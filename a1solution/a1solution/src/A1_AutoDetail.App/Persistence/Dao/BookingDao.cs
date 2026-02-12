using A1_AutoDetail.App.Domain.Contracts;
using A1_AutoDetail.App.Domain.Entities;
using A1_AutoDetail.App.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A1_AutoDetail.App.Persistence.Dao
{
    public class BookingDao : IBookingDao
    {
        private readonly AppDbContext _db;

        public BookingDao(AppDbContext db)
        {
            _db = db;
        }


        public Booking FindBooking(int bookingId)
        {
            return _db.Bookings.Find(bookingId);
        }

        public Customer FindCustomer(int customerId)
        {
            return _db.Customers.Find(customerId);
        }

        public DetailService FindDetailService(int detailId)
        {
            return _db.DetailServices.Find(detailId);
        }

        public TimeSlot FindTimeSlot (int timeSlotId)
        {
            return _db.TimeSlots.Find(timeSlotId);
        }

        public int AddBooking(Booking booking)
        {
            _db.Bookings.Add(booking);
            _db.SaveChanges();

            return booking.BookingId;
        }

        public bool IsTimeSlotBooked(int timeSlotId)
        {
            var q =
            from b in _db.Bookings.AsNoTracking()
            where b.TimeSlotId == timeSlotId
            select b.BookingId;

            return q.Any();
        }

        public bool HasCustomerBookedOnDate(int customerId, DateOnly date)
        {
            var start = date.ToDateTime(TimeOnly.MinValue);
            var end = date.AddDays(1).ToDateTime(TimeOnly.MinValue);

            var q =
                from b in _db.Bookings.AsNoTracking()
                join ts in _db.TimeSlots.AsNoTracking() on b.TimeSlotId equals ts.TimeSlotId
                where b.CustomerId == customerId
                   && ts.StartTime >= start
                   && ts.StartTime < end
                select b.BookingId;

            return q.Any();
        }
    }
}
