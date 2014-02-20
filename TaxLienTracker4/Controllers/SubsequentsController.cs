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
    [Authorize]
    public class SubsequentsController : Controller
    {
        //
        // GET: /Subsequents/
        
        private readonly EntityManager _entityManager = new EntityManager();

        public ActionResult Index()
        {
            var counties = _entityManager.Counties();
            return View(counties);
        }

        public ActionResult Properties(int municipalityId)
        {
            var model = new OutstandingPropertiesForMunicipalityViewModel
                {
                    Properties = _entityManager.OutstandingProperties(municipalityId)
                };

            return View(model);
        }

        public ActionResult Pay(int id)
        {
            var model = new PaySubsequentsViewModel
                {
                    Property = _entityManager.Property(id)
                };
            return View(model);
        }


        [HttpPost]
        public ActionResult Pay(IEnumerable<Subsequent> subsequents)
        {
            foreach (Subsequent subsequent in subsequents)
            {
                if (subsequent.SubsequentAmount > 0)
                {
                    _entityManager.Add(subsequent);
                }
            }


            return RedirectToAction("Index");
        }

        public ActionResult ListForMunicipality(int municipalityId)
        {
            var model = new OutstandingPropertiesForMunicipalityViewModel()
                {
                    Properties = _entityManager.OutstandingProperties(municipalityId)
                };
            return View(model);
        }
    }
}