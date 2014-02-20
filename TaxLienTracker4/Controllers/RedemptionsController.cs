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
        private readonly EntityManager _entityManager = new EntityManager();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Redeem(int id)
        {
            return View(_entityManager.Property(id));
        }

        [HttpPost]
        public ActionResult Redeem(DateTime redemptionDate, int propertyId)
        {
            RedemptionProcessor redemptionProcessor = new RedemptionProcessor(propertyId);
            redemptionProcessor.SetRedemptionAndAccrual(redemptionDate);
            redemptionProcessor.CreateEarnings();
           

            #region oldStuff to be deleted
            //            Property property = _entityManager.Property(propertyId);
//
//            foreach (Certificate certificate in property.Certificates)
//            {
//                certificate.RedemptionDate = redemptionDate;
//                TimeSpan? accrualSpan = certificate.RedemptionDate.Value - certificate.DateOfPurchase;
//                certificate.AccrualPeriod = accrualSpan.Value.Days;
//                EarningsCalculator.CalculateCertificatesInterest(certificate, propertyId);
////                _entityManager.Update(certificate);
//                if (certificate.Earning != null)
//                {
//                    property.Earnings.Add(certificate.Earning);
//                }
//            }
//
//            foreach (Subsequent subsequent in property.Subsequents)
//            {
//                TimeSpan? accrualSpan = property.Certificates.First().RedemptionDate - subsequent.OutLayDate;
//                subsequent.AccrualPeriod = accrualSpan.Value.Days;
//                EarningsCalculator.CalculateSubsequentInterest(subsequent);
//                property.Earnings.Add(new Earning()
//                    {
//                        Amount = (decimal) subsequent.InterestEarnings,
//                        EarningsTypeId = 2,
//                        PropertyId = property.Id
//                    });
//            }
//
//            EarningsCalculator.Calculate246Penalty(property);
//            EarningsCalculator.CalculateYearEndPenalty(property);
//
//            _entityManager.Add(property.Earnings);
            ////            _entityManager.Update(property);
            #endregion

            return RedirectToAction("Property", "Reports", new {propertyId = propertyId});
        }


        public ActionResult EditEarnings(int propertyId)
        {
            return View(new PropertyEarningsViewModel() {Property = _entityManager.Property(propertyId)});
        }

        [HttpPost]
        public ActionResult Edit(Property property)
        {
            _entityManager.Update(property);
            return RedirectToAction("Property", "Reports", new {id = property.Id});
        }
    }
}