namespace OtlobRESTAPI.Data.Models
{
    public class RestaurantRequest
    {
        public int Id { get; set; }
        public string address { get; set; }
        public string floor { get; set; }
        public string storeName { get; set; }
        public string brandName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public int statusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
