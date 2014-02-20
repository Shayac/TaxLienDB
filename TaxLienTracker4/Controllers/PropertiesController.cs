using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using DataLayer;
using TaxLienTracker4.Models;

namespace TaxLienTracker4.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly EntityManager _entityManager = new EntityManager();
        //
        // GET: /Properties/
        public ActionResult Purchase()
        {
            var model = new PurchaseViewModel {Counties = _entityManager.Counties()};
            return View(model);
        }

        [HttpPost]
        public ActionResult Municipalities(int countyId)
        {
            var muncipalities = _entityManager.Municipalities(countyId).Select(m => new
                {
                    MunicipalityId = m.Id,
                    m.MunicipalityName
                });
            return Json(muncipalities);
        }

        [HttpPost]
        public ActionResult Purchase(Property property, Certificate certificate)
        {
            property.Certificates.Add(certificate);
            _entityManager.Add(property);
            
            return RedirectToAction("OutstandingPropertiesForMunicipality", "Reports", new { municipalityId = property.MunicipalityId });
        }

        public ActionResult EditProperty(int propertyId)
        {
            return View(_entityManager.Property(propertyId));
        }

        
    }
}