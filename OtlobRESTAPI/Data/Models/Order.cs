namespace OtlobRESTAPI.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int restaurantId { get; set; }
        public int statusId { get; set; }
        public double totalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
