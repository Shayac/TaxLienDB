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

        public ActionResult RedemptionDate(int id)
        {
            return View(_entityManager.Property(id));
        }
        
        
        [HttpPost]
        public ActionResult CalculateEarnings(DateTime redemptionDate, int propertyId)
        {
            Property property = _entityManager.Property(propertyId);
            
            foreach (Certificate certificate in property.Certificates)
            {
                certificate.RedemptionDate = redemptionDate;
                EarningsCalculator.CalculateCertificatesInterest(certificate, propertyId);
                if (certificate.Earning != null)
                {
                    property.Earnings.Add(certificate.Earning);
                }

            }

            foreach (Subsequent subsequent in property.Subsequents)
            {
                subsequent.AccrualPeriod = property.Certificates.First().RedemptionDate - subsequent.OutLayDate;
                EarningsCalculator.CalculateSubsequentInterest(subsequent);
            }

            EarningsCalculator.Calculate246Penalty(property);
            EarningsCalculator.CalculateYearEndPenalty(property);

            _entityManager.Add(property.Earnings);

           return View(property);

        }

      }
}