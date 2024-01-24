namespace OtlobRESTAPI.Data.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public int orderId { get; set; }
        public int driverId { get; set; }
        public int vehicleId { get; set; }
        public string deliveryStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
