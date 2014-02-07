using System.Linq;
using System.Web.Mvc;
using BusinessLayer;
using TaxLienTracker4.Models;

namespace TaxLienTracker4.Controllers
{
    public class ReportsController : Controller
    {
        private readonly EntityManager _entityManager = new EntityManager();

        public ActionResult OutstandingPropertiesForMunicipality(int municipalityId)
        {
            var model = new OutstandingPropertiesForMunicipalityViewModel
                {
                    Properties = _entityManager.OutstandingProperties(municipalityId)
                };


            return View(model);
        }

        public ActionResult Property(int id)
        {
           return View(_entityManager.Property(id));
        }
    }
}