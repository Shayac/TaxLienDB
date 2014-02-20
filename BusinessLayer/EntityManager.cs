using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace BusinessLayer
{
    public class EntityManager
    {
        #region Create Methods

        public void Add(Property property)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                context.Properties.Add(property);
                context.SaveChanges();
            }
        }

        public void Add(Certificate certificate)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                context.Certificates.Add(certificate);
                context.SaveChanges();
            }
        }

        public void Add(Municipality municipality)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                context.Municipalities.Add(municipality);
                context.SaveChanges();
            }
        }

        public void Add(County county)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                try
                {
                    context.Counties.Add(county);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                                                   validationError.ErrorMessage);
                        }
                    }
                }
            }
        }

        public void Add(IEnumerable<Earning> earnings)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                foreach (var earning in earnings)
                {
                    context.Earnings.Add(earning);
                }

                context.SaveChanges();
            }
        }

        public void Add(Subsequent subsequent)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                Property property = Property(subsequent.PropertyId);

                //adds Amount to above1500 if any previous sub has above1500 and exits method
                foreach (Subsequent previousSubsequent in property.Subsequents)
                {
                    if (previousSubsequent.Above1500 != 0)
                    {
                        subsequent.Above1500 = subsequent.SubsequentAmount;
                        context.Subsequents.Add(subsequent);
                        context.SaveChanges();
                        return;
                    }
                }

                decimal total = property.Certificates.Sum(c => c.LienAmount);


                if (property.Subsequents.Any())
                {
                    total += property.Subsequents.Sum(s => s.SubsequentAmount);
                }

                if (total <= 1500m && (total + subsequent.SubsequentAmount) <= 1500m) //before any subs hit 1500
                {
                    subsequent.Below1500 = subsequent.SubsequentAmount;
                }
                else if (total <= 1500m && (total + subsequent.SubsequentAmount) > 1500m) //this sub hits 1500
                {
                    subsequent.Below1500 = 1500m - total;
                    subsequent.Above1500 = subsequent.SubsequentAmount - subsequent.Below1500;
                }
                else if (total > 1500) //previous subs hit 1500
                {
                    subsequent.Above1500 = subsequent.SubsequentAmount;
                }

                context.Subsequents.Add(subsequent);
                context.SaveChanges();
            }
        }

        #endregion

        #region Retrieval Methods

        public County County(int id)
        {
            County county;
            using (var context = new TaxLienDataBaseEntities())
            {
                county = context.Counties.FirstOrDefault(c => c.Id == id);
            }
            return county;
        }

        public IEnumerable<County> Counties()
        {
            IEnumerable<County> counties = new List<County>();
            using (var context = new TaxLienDataBaseEntities())
            {
                counties = context.Counties.ToList();
            }
            return counties;
        }

        public Municipality Municipality(int id)
        {
            Municipality municipality;
            using (var context = new TaxLienDataBaseEntities())
            {
                municipality = context.Municipalities.FirstOrDefault(m => m.Id == id);
            }
            return municipality;
        }

        public IEnumerable<Municipality> Municipalities()
        {
            IEnumerable<Municipality> municipalities = new List<Municipality>();
            using (var context = new TaxLienDataBaseEntities())
            {
                municipalities = context.Municipalities.ToList();
            }
            return municipalities;
        }

        public IEnumerable<Municipality> Municipalities(int countyId)
        {
            IEnumerable<Municipality> municipalities = new List<Municipality>();
            using (var context = new TaxLienDataBaseEntities())
            {
                municipalities = context.Municipalities.Where(m => m.CountyId == countyId).ToList();
            }
            return municipalities;
        }

        public Property Property(int propertyId)
        {
            Property property;
            using (var context = new TaxLienDataBaseEntities())
            {
                property =
                    context.Properties.Include(x => x.Certificates).Include(x => x.Subsequents)
                           .Include(x => x.Earnings)
                           .Include(x => x.Municipality)
                           .FirstOrDefault(p => p.Id == propertyId);
            }

            return property;
        }

        public IEnumerable<Property> Properties(int municipalityId)
        {
            IEnumerable<Property> properties = new List<Property>();
            using (var context = new TaxLienDataBaseEntities())
            {
                properties = context.Properties.Where(p => p.MunicipalityId == municipalityId).ToList();
            }
            return properties;
        }

        public IEnumerable<Property> OutstandingProperties(int municipalityId)
        {
            IEnumerable<Property> properties = new List<Property>();
            using (var context = new TaxLienDataBaseEntities())
            {
                properties =
                    context.Properties.Where(
                        p => p.MunicipalityId == municipalityId && p.Certificates.Any() &&
                        p.Certificates.All(c => c.RedemptionDate == null)).Include(p => p.Certificates)
                           .ToList();
            }
            return properties;
        }

        public IEnumerable<Property> RedeemedProperties(int municipalityId)
        {
            IEnumerable<Property> properties = new List<Property>();
            using (var context = new TaxLienDataBaseEntities())
            {
                properties =
                    context.Properties.Where(
                        p => p.MunicipalityId == municipalityId && p.Certificates.Any(c => c.RedemptionDate != null))
                           .Include(p => p.Certificates)
                           .ToList();
            }
            return properties;
        } 

        #endregion

        #region Update Methods
        public void Update(Property property)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                
                context.SaveChanges();
            }
        }

        public void Update(County county)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                try
                {
                    context.Entry(county).State = EntityState.Modified;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                                                   validationError.ErrorMessage);
                        }
                    }
                }
            }
        }

        public void Update(Municipality municipality)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                try
                {
                    context.Entry(municipality).State = EntityState.Modified;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                                                   validationError.ErrorMessage);
                        }
                    }
                }
            }
        }

        public void Update(Certificate certificate)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                try
                {
                    context.Entry(certificate).State = EntityState.Modified;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                                                   validationError.ErrorMessage);
                        }
                    }
                }
            }
        }

        public void Update(Subsequent subsequent)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                try
                {
                    context.Entry(subsequent).State = EntityState.Modified;

                    context.SaveChanges();
                }
                catch (Exception)
                {
                    
                }
            }
        }

        #endregion

        #region Delete Methods
        public void Delete(County county)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                context.Counties.Attach(county);
                context.Counties.Remove(county);
                context.SaveChanges();
            }
        }

        public void Delete(Municipality municipality)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                context.Municipalities.Attach(municipality);
                context.Municipalities.Remove(municipality);
                context.SaveChanges();
            }
        }

        #endregion

        #region Redemption Methods

//        public void Redeem(DateTime redemptionDate, int propertyId)
//        {
//            Property property = Property(propertyId);
//            InsertRedemptionAndAccrual(redemptionDate, property.Certificates, property.Subsequents);
//
//        }
//
//
//        public void InsertRedemptionAndAccrual(DateTime redemptionDate, IEnumerable<Certificate> certificates, IEnumerable<Subsequent> subsequents)
//        {
//            foreach (Certificate certificate in certificates)
//            {
//                certificate.RedemptionDate = redemptionDate;
//                TimeSpan? accrualSpan = certificate.RedemptionDate.Value - certificate.DateOfPurchase;
//                certificate.AccrualPeriod = accrualSpan.Value.Days;
//            }
//
//            foreach (Subsequent subsequent in subsequents)
//            {
//                TimeSpan? accrualSpan = certificates.First().RedemptionDate - subsequent.OutLayDate;
//                subsequent.AccrualPeriod = accrualSpan.Value.Days;   
//            }
//        }

//        public void CalculateInterest(IEnumerable<Certificate> certificates, IEnumerable<Subsequent> subsequents)
//        {
//            EarningsCalculator.CalculateCertificatesInterest(certificate, propertyId);
//        }

        #endregion

    }
}