namespace A1_AutoDetail.App.Entities;

public sealed class Customer
{
    public int CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public bool IsBlocklisted { get; set; }
}
