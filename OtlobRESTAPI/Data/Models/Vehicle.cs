namespace OtlobRESTAPI.Data.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int vehicleTypeId { get; set; }
        public string vehicleType { get; set; }
        public string plateNumber { get; set; }
        public string model { get; set; }
        public string color { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
