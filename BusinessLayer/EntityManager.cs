using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.Data.Entity;

namespace BusinessLayer
{
    public class EntityManager
    {

        #region Add Entities

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
                context.Counties.Add(county);
                context.SaveChanges();
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

                foreach (Subsequent s in property.Subsequents)
                //adds Amount to above1500 if any previous sub has above1500 and exits method
                {
                    if (s.Above1500 != 0)
                    {
                        subsequent.Above1500 = subsequent.SubsequentAmount;
                        return;
                    }
                }

                decimal total= property.Certificates.Sum(c => c.LienAmount);

                
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

        public IEnumerable<County> Counties()
        {
            IEnumerable<County> counties = new List<County>();
            using (var context = new TaxLienDataBaseEntities())
            {
                counties = context.Counties.ToList();
            }
            return counties;
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
                    context.Properties.Include(x => x.Certificates).Include(x=>x.Subsequents)
                    .Include(x=>x.Earnings).Include(x => x.Municipality).FirstOrDefault(p => p.Id == propertyId);
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
                    context.Properties.Include(p => p.Certificates).Where(
                        p => p.MunicipalityId == municipalityId && p.Certificates.Any() && p.Certificates.All(c => c.RedemptionDate == null))
                           .ToList();
            }
            return properties;
        }


        #endregion
    }
}
