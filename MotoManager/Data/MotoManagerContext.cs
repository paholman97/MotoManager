using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoManager.Data
{
    public class MotoManagerContext : DbContext
    {
        public MotoManagerContext(DbContextOptions<MotoManagerContext> options)
            : base(options)
        {
        }

        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<MaintenanceSpec> MaintenanceSpecs { get; set; }
        public DbSet<BikeMaintenanceSpec> BikeMaintenanceSpecs { get; set; }
        public DbSet<ServiceHistory> ServiceHistory { get; set; }
    }

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
        public string? ImagePath { get; set; }
        public Manufacturer? Manufacturer { get; set; }
    }

    public class Manufacturer
    {
        public int ManufacturerID { get; set; }
        public string ManufacturerName { get; set; }
    }

    public class MaintenanceSpec
    {
        public int MaintenanceSpecID { get; set; }
        public string SpecName { get; set; }
    }

    public class BikeMaintenanceSpec
    {
        public int BikeMaintenanceSpecID { get; set; }
        public int? BikeID { get; set; }
        public int? MaintenanceSpecID { get; set; }
        public string SpecValue { get; set; }
        public MaintenanceSpec? MaintenanceSpec { get; set; }
    }

    public class ServiceHistory
    {
        public int ServiceHistoryID { get; set; }
        public int? BikeID { get; set; }
        public int? Mileage { get; set; }
        public int? DueIn { get; set; }
        public string PerformedBy { get; set; }
        public string Description { get; set; }
    }
}