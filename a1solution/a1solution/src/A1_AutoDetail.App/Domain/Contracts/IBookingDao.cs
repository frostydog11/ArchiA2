using A1_AutoDetail.App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A1_AutoDetail.App.Domain.Contracts
{
    internal interface IBookingDao
    {
        public Booking FindBooking(int bookingId);

        public Customer FindCustomer(int customerId);

        public DetailService FindDetailService(int detailId);

        public TimeSlot FindTimeSlot(int timeSlotId);
    }
}
