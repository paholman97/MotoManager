using MotoManager.Data;

namespace MotoManager.Models
{
    public class Bike
    {
        public int BikeID { get; set; }
        public int? ManufacturerID { get; set; }
        public string? Model { get; set; }
        public string? Colour { get; set; }
        public int? EngineSize { get; set; }
        public int? Year { get; set; }
        public string? Registration { get; set; }
        public string? VIN { get; set; }
        public string? EngineNumber { get; set; }
        public int? Mileage { get; set; }
        public decimal? PurchasePrice { get; set; }
        public IFormFile? File { get; set; }
        public string? ImagePath { get; set; }
        public Manufacturer? Manufacturer { get; set; }
    }
}