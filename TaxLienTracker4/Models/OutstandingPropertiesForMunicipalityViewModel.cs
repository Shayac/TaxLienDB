using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace TaxLienTracker4.Models
{
    public class OutstandingPropertiesForMunicipalityViewModel
    {
        public IEnumerable<Property> Properties { get; set; }
    }
}