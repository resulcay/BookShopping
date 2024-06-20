namespace BookShoppingUI.Models.DTOs
{
    public class OrderDetailModel
    {
        public string? DivId { get; set; }

        public IEnumerable<OrderDetail>? OrderDetail { get; set; }

        public double TotalPrice { get; set; }
    }
}
