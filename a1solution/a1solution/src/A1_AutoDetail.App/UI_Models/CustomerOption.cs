namespace A1_AutoDetail.App.UI_Models
{
    public sealed class CustomerOption
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = "";
        public bool IsBlocklisted { get; set; }
    }
}
