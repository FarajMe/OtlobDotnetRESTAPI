namespace OtlobRESTAPI.Data.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int orderId { get; set; }
        public int menuItemId { get; set; }
        public int quantity { get; set; }
        public decimal subtotal { get; set; }
    }
}
