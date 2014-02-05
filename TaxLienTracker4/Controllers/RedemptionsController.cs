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
    public class RedemptionsController : Controller
    {
        //
        // GET: /Redemptions/
        EntityManager _entityManager = new EntityManager();

        public ActionResult Redeem(int id)
        {
            var model = new PropertyEarningsProjectionViewModel()
                {
                    Property = _entityManager.Property(id)
                };
            return View(model);

        }

        [HttpPost]
        public ActionResult InsertDate(DateTime redemptionDate, int propertyId)
        {
            var model = new PropertyEarningsProjectionViewModel()
                {
                    Property = _entityManager.Property(propertyId)
                };
            foreach (Certificate certificate in model.Property.Certificates)
            {
                certificate.RedemptionDate = redemptionDate;
            }
            
            return View("Redeem", model);
            
        }

    }
}
