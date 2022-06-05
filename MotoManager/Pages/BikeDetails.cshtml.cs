using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MotoManager.Data;

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

        public JsonResult OnGetServicesDue(int bikeId, int mileage)
        {
            var bikeIdParam = new SqlParameter("BikeID", bikeId);
            var mileageParam = new SqlParameter("Mileage", mileage);

            var servicesDue = motoManagerContext.ServiceHistory.FromSqlRaw("GetServicesDue @BikeID, @Mileage", bikeIdParam, mileageParam).ToList();

            return new JsonResult(servicesDue);
        }

        public JsonResult OnPostCompleteService(int serviceHistoryId, int mileage)
        {
            ServiceHistory serviceHistory = motoManagerContext.ServiceHistory.FirstOrDefault(x => x.ServiceHistoryID == serviceHistoryId);

            if (serviceHistory != null)
            {
                serviceHistory.Mileage = mileage;
                motoManagerContext.SaveChanges();
            }
            return new JsonResult(null);
        }
    }
}