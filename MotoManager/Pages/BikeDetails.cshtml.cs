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
        private IWebHostEnvironment environment;

        public BikeDetailsModel(MotoManagerContext _motoManagerContext, IWebHostEnvironment _environment)
        {
            motoManagerContext = _motoManagerContext;
            environment = _environment;
        }

        public Bike bike;
        public List<BikeMaintenanceSpec> bikeMaintenanceSpecs;
        public List<ServiceHistory> serviceHistory;

        public void OnGet(int id)
        {
            bike = GetBikeDetails(id);
            bikeMaintenanceSpecs = motoManagerContext.BikeMaintenanceSpecs.Include(m => m.MaintenanceSpec).Where(x => x.BikeID == id).ToList();
            serviceHistory = motoManagerContext.ServiceHistory.Where(x => x.BikeID == id).ToList();
        }

        public JsonResult OnGetBikeDetails(int bikeId)
        {
            bike = GetBikeDetails(bikeId);
            return new JsonResult(bike);
        }

        public Bike GetBikeDetails(int bikeId)
        {
            return motoManagerContext.Bikes.Include(x => x.Manufacturer).FirstOrDefault(x => x.BikeID == bikeId);
        }

        public JsonResult OnPostUpdateBikeImage(int bikeId, IFormFile bikeImage)
        {
            string imageFolderPath = Path.Combine(environment.WebRootPath, "Images");
            string imageFileName = Path.GetFileName(bikeImage.FileName);

            using (FileStream stream = new FileStream(Path.Combine(imageFolderPath, imageFileName), FileMode.Create))
            {
                bikeImage.CopyTo(stream);
            }

            var bike = motoManagerContext.Bikes.Include(x => x.Manufacturer).FirstOrDefault(x => x.BikeID == bikeId);

            if (bike != null)
                bike.ImagePath = "/Images/" + imageFileName;
            
            motoManagerContext.SaveChanges();

            return new JsonResult(bike.ImagePath);
        }

        public async Task SaveBikeDetails(Bike bikeVm)
        {
            Bike bike = motoManagerContext.Bikes.FirstOrDefault(x => x.BikeID == bikeVm.BikeID);


            motoManagerContext.SaveChanges();
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