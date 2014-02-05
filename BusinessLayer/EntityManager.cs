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

        public void Add(Earning earning)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                context.Earnings.Add(earning);
                context.SaveChanges();
            }
        }

        public void Add(Subsequent subsequent)
        {
            using (var context = new TaxLienDataBaseEntities())
            {
                context.Subsequents.Add(subsequent);
                context.SaveChanges();
            }
        }

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
                        p => p.MunicipalityId == municipalityId && p.Certificates.All(c => c.RedemptionDate != null))
                           .ToList();
            }
            return properties;
        }


        #endregion
    }
}
