using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace TaxLienTracker4.Models
{
    public class MunicipalitiesViewModel
    {
        public County County { get; set; }
        public IEnumerable<Municipality> Municipalities  { get; set; }
    }
}