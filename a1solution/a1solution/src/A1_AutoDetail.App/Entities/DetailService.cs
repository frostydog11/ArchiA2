namespace A1_AutoDetail.App.Entities;

public sealed class DetailService
{
    public int DetailServiceId { get; set; }
    public string DetailServiceName { get; set; } = string.Empty;
    public bool IsPremium { get; set; }
}
