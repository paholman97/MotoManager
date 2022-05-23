using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MotoManager.Data;

namespace MotoManager.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MotoManagerContext _motoManagerContext;

        public IndexModel(MotoManagerContext motoManagerContext)
        {
            _motoManagerContext = motoManagerContext;
        }

        public List<Bike> Bikes { get; set; }

        public void OnGet()
        {
            Bikes = _motoManagerContext.Bikes
                .Include(m => m.Manufacturer)
                .ToList();
        }
    }
}