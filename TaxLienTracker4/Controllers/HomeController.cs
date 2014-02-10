using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;

namespace TaxLienTracker4.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private readonly EntityManager entityManager = new EntityManager();

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult CountyPicker()
        {
            return PartialView("_CountySelector", entityManager.Counties());
        }
    }
}