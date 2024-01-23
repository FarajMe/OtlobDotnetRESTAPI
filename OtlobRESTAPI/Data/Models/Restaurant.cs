namespace OtlobRESTAPI.Data.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
