using System.Linq;
using System.Web.Mvc;
using BusinessLayer;
using TaxLienTracker4.Models;

namespace TaxLienTracker4.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly EntityManager _entityManager = new EntityManager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OutstandingPropertiesForMunicipality(int municipalityId)
        {
            var model = new OutstandingPropertiesForMunicipalityViewModel
                {
                    Properties = _entityManager.OutstandingProperties(municipalityId)
                };

            return View(model);
        }

        public ActionResult RedeemedLiens(int municipalityId)
        {
            return View(_entityManager.RedeemedProperties(municipalityId));
        }

        public ActionResult Property(int propertyId)
        {
            var model = new PropertyEarningsViewModel()
                {
                    Property = _entityManager.Property(propertyId)
                };
            return View(model);
        }

        public ActionResult PropertyEarnings(int propertyId)
        {
            var model = new PropertyEarningsViewModel()
                {
                    Property = _entityManager.Property(propertyId)
                };
            return View(model);
        }

        public ActionResult CertificateEarnings(int propertyId)
        {
            return View(_entityManager.Property(propertyId));
        }

        public ActionResult SubsequentsEarnings(int propertyId)
        {
            return View(_entityManager.Property(propertyId));
        }
    }
}