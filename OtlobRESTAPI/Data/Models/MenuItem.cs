namespace OtlobRESTAPI.Data.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int restaurantId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
