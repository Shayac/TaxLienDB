using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public static class EarningsFactory
    {
        
        public static Earning GenerateEarning(Property property, int earningTypeId, Func<Property, decimal> earningCalculationMethod)
        {
            Earning earning = new Earning();
            earning.PropertyId = property.Id;
            earning.EarningsTypeId = earningTypeId;
            earning.Amount = earningCalculationMethod(property);
            return earning;

        }
    }

    
}
