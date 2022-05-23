using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotoManager.Data;

namespace MotoManager.Pages
{
    public class NewBikeDetailsModel : PageModel
    {
        private IWebHostEnvironment environment;
        private readonly MotoManagerContext motoManagerContext;

        public NewBikeDetailsModel(IWebHostEnvironment _environment, MotoManagerContext _motoManagerContext)
        {
            environment = _environment;
            motoManagerContext = _motoManagerContext;
        }

        public List<SelectListItem> Manufacturers { get; set; }
        public List<MaintenanceSpec> MaintenanceSpecs { get; set; }

        [BindProperty]
        public Models.Bike BikeModel { get; set; }

        [BindProperty]
        public List<BikeMaintenanceSpec> BikeMaintenanceSpecs { get; set; }

        [BindProperty]
        public List<ServiceHistory> ServiceHistory { get; set; }

        public void OnGet()
        {
            Manufacturers = motoManagerContext.Manufacturers.Select(m =>
                                  new SelectListItem
                                  {
                                      Value = m.ManufacturerID.ToString(),
                                      Text = m.ManufacturerName
                                  }).ToList();

            MaintenanceSpecs = motoManagerContext.MaintenanceSpecs.ToList();
        }

        public IActionResult OnPostSaveNewBikeDetails()
        {
            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            if (BikeModel.File != null)
            {
                string imageFolderPath = Path.Combine(environment.WebRootPath, "Images");
                string imageFileName = Path.GetFileName(BikeModel.File.FileName);

                using (FileStream stream = new FileStream(Path.Combine(imageFolderPath, imageFileName), FileMode.Create))
                {
                    BikeModel.File.CopyTo(stream);
                }

                BikeModel.ImagePath = "/Images/" + imageFileName;
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Bike, Bike>());
            var mapper = new Mapper(config);

            Bike bike = mapper.Map<Bike>(BikeModel);

            motoManagerContext.Bikes.Add(bike);
            motoManagerContext.SaveChanges();

            foreach (var bikeMaintenanceSpec in BikeMaintenanceSpecs)
            {
                bikeMaintenanceSpec.BikeID = bike.BikeID;

                if (bikeMaintenanceSpec.SpecValue != null)
                    motoManagerContext.BikeMaintenanceSpecs.Add(bikeMaintenanceSpec);
            }

            foreach (var serviceHistory in ServiceHistory)
            {
                serviceHistory.BikeID = bike.BikeID;
                motoManagerContext.ServiceHistory.Add(serviceHistory);
            }
            motoManagerContext.SaveChanges();

            return RedirectToPage("BikeDetails", bike.BikeID);
        }

        public IActionResult OnPostCancelNewBike()
        {
            return RedirectToPage("Index");
        }
    }
}