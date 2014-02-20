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
    public class SettingsController : Controller
    {
        //
        // GET: /Settings/

        private readonly EntityManager _entityManager = new EntityManager();

        #region County Crud
        public ActionResult Counties()
        {
            return View(_entityManager.Counties());
        }

        [HttpPost]
        public ActionResult AddCounty(County county)
        {
            _entityManager.Add(county);
            return RedirectToAction("Counties");
        }

        public ActionResult EditCounty(int id)
        {
            return View(_entityManager.County(id));
        }

        [HttpPost]
        public ActionResult EditCounty(County county)
        {
            _entityManager.Update(county);
            return RedirectToAction("Counties");
        }


        public ActionResult DeleteCounty(int id)
        {
            _entityManager.Delete(_entityManager.County(id));
            return RedirectToAction("Counties");
        }
        #endregion


        #region Municipality Crud
        public ActionResult Municipalities(int countyId)
        {
            var model = new MunicipalitiesViewModel()
                {
                    County = _entityManager.County(countyId),
                    Municipalities = _entityManager.Municipalities(countyId)
                };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddMunicipality(Municipality municipality)
        {
            _entityManager.Add(municipality);
            return RedirectToAction("Municipalities", new {municipality.CountyId});
        }

        public ActionResult EditMunicipality(int id)
        {
            return View(_entityManager.Municipality(id));
        }

        [HttpPost]
        public ActionResult EditMunicipality(Municipality municipality)
        {
            _entityManager.Update(municipality);
            return RedirectToAction("Municipalities", new {municipality.CountyId});
        }

        public ActionResult DeleteMunicipality(int id)
        {
            Municipality municipality = _entityManager.Municipality(id);
            _entityManager.Delete(municipality);
            return RedirectToAction("Municipalities", new{municipality.CountyId});
        }

        #endregion
    }
}