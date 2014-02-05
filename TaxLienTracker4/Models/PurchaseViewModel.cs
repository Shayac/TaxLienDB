using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace TaxLienTracker4.Models
{
    public class PurchaseViewModel
    {
        public IEnumerable<County> Counties { get; set; }
    }
}