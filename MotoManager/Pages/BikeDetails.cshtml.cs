using Microsoft.AspNetCore.Mvc.RazorPages;
using MotoManager.Data;
using Microsoft.EntityFrameworkCore;

namespace MotoManager.Pages
{
    public class BikeDetailsModel : PageModel
    {
        private readonly MotoManagerContext motoManagerContext;

        public BikeDetailsModel(MotoManagerContext _motoManagerContext)
        {
            motoManagerContext = _motoManagerContext;
        }

        public Bike bike;
        public List<BikeMaintenanceSpec> bikeMaintenanceSpecs;
        public List<ServiceHistory> serviceHistory;

        public void OnGet(int id)
        {
            bike = motoManagerContext.Bikes.Include(x => x.Manufacturer).FirstOrDefault(x => x.BikeID == id);
            bikeMaintenanceSpecs = motoManagerContext.BikeMaintenanceSpecs.Include(m => m.MaintenanceSpec).Where(x => x.BikeID == id).ToList();
            serviceHistory = motoManagerContext.ServiceHistory.Where(x => x.BikeID == id).ToList();
        }
    }
}