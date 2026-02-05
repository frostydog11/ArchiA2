using A1_AutoDetail.App.Services;
using A1_AutoDetail.App.UI_Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace A1_AutoDetail.Web.Models;

public sealed class CreateBookingViewModel
{
    public int? CustomerId { get; set; }
    public int? DetailServiceId { get; set; }
    public int? TimeSlotId { get; set; }

    public List<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> Services { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> TimeSlots { get; set; } = new List<SelectListItem>();

    public List<BookingListRow> CurrentBookings { get; set; } = new List<BookingListRow>();


    public BookingOutcome? Outcome { get; set; }
    public string? Message { get; set; }
    public int? BookingId { get; set; }
}
