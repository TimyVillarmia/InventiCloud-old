using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities;

public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Occupation { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
    }