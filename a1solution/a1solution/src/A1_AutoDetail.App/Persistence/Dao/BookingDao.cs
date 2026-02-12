using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A1_AutoDetail.App.Domain.Contracts;
using A1_AutoDetail.App.Domain.Entities;
using A1_AutoDetail.App.Persistence;

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
    }
}
