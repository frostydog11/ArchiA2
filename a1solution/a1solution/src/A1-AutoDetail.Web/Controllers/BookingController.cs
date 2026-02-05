using A1_AutoDetail.App.Services;
using A1_AutoDetail.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace A1_AutoDetail.Web.Controllers;

public sealed class BookingController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly IBookingDataAdapter _adapter;

    public BookingController(
        IBookingService bookingService,
        IBookingDataAdapter adapter)
    {
        _bookingService = bookingService;
        _adapter = adapter;
    }

    [HttpGet]
    public IActionResult Create()
    {
        var vm = BuildCreateVm();
        return View(vm);
    }

    [HttpPost]
    public IActionResult Create(CreateBookingViewModel vm)
    {
        if (vm.CustomerId == null || vm.DetailServiceId == null || vm.TimeSlotId == null)
        {
            var back = BuildCreateVm();
            back.Outcome = null;
            back.Message = "Please choose a customer, a service, and a time slot.";
            return View(back);
        }

        var result = _bookingService.Book(
            vm.CustomerId.Value,
            vm.DetailServiceId.Value,
            vm.TimeSlotId.Value,
            DateTime.UtcNow);

        if (result.Outcome == BookingOutcome.CustomerBlocklisted)
            return View("Denied");

        var outVm = BuildCreateVm();
        outVm.CustomerId = vm.CustomerId;
        outVm.DetailServiceId = vm.DetailServiceId;
        outVm.TimeSlotId = vm.TimeSlotId;
        outVm.Outcome = result.Outcome;
        outVm.Message = result.Message;
        outVm.BookingId = result.BookingId;

        return View(outVm);
    }

    private CreateBookingViewModel BuildCreateVm()
    {
        var vm = new CreateBookingViewModel();

        // UI-shaped read models only (no entities). This keeps the controller from
        // "knowing" persistence details or being tempted to implement business rules.
        var customerOptions = _adapter.GetCustomerOptions();
        vm.Customers = customerOptions.Select(o => new SelectListItem
            {
                Value = o.CustomerId.ToString(),
                Text = o.FullName + (o.IsBlocklisted ? " (BLOCKLISTED)" : "")
            }).ToList();


        var serviceOptions = _adapter.GetDetailServiceOptions();
        vm.Services = serviceOptions.Select(o => new SelectListItem
        {
            Value = o.DetailServiceId.ToString(),
            Text = o.DetailServiceName + (o.IsPremium ? " (Premium)" : "")
        }).ToList();

        var timeSlotOptions = _adapter.GetTimeSlotOptions();
        vm.TimeSlots = timeSlotOptions.Select(o => new SelectListItem
        {
            Value = o.TimeSlotId.ToString(),
            Text = o.StartTimeText
        }).ToList();

        var bookingRows = _adapter.GetCurrentBookings();
        //vm.CurrentBookings = _adapter.GetCurrentBookings();
        vm.CurrentBookings = bookingRows;

        return vm;
    }
}
