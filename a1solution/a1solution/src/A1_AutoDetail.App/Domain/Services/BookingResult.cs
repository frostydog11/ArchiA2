namespace A1_AutoDetail.App.Services;

public sealed class BookingResult
{
    public BookingOutcome Outcome { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? BookingId { get; set; }
}
