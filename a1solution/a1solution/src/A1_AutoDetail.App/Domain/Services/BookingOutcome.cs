namespace A1_AutoDetail.App.Services;

public enum BookingOutcome
{
    Success = 0,
    CustomerNotFound = 1,
    ServiceNotFound = 2,
    TimeSlotNotFound = 3,
    CustomerBlocklisted = 4,
    CustomerAlreadyBookedThatDay = 5,
    TimeSlotUnavailable = 6,
    TooSoonForService = 7
}
