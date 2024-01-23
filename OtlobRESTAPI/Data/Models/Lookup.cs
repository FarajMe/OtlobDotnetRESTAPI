using System.ComponentModel.DataAnnotations;

namespace OtlobRESTAPI.Data.Models
{
    public class Lookup
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public string ReferenceType { get; set; }
    }
}
