using A1_AutoDetail.App.UI_Models;

namespace A1_AutoDetail.App.Services;

// Read-only UI queries used by controllers.
// These return UI-shaped models (not entities) to keep EF/persistence concerns
// and business decisions out of the UI layer.
public interface IBookingDataAdapter
{
    List<CustomerOption> GetCustomerOptions();
    List<DetailServiceOption> GetDetailServiceOptions();
    List<TimeSlotOption> GetTimeSlotOptions();
    List<BookingListRow> GetCurrentBookings();
}
