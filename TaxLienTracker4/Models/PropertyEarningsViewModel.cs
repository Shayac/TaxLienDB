using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace TaxLienTracker4.Models
{
    public class PropertyEarningsViewModel
    {
        public Property Property { get; set; }

        #region Outlay
        public decimal TotalCertificateOutlay
        {
            get { return Property.Certificates.Sum(c => c.LienAmount); }
        }

        public decimal? TotalPremium
        {
            get { return Property.Certificates.Sum(c => c.Premium); }
        }

        public decimal? TotalSubsequentsOutlay
        {
            get { return Property.Subsequents.Sum(s => s.SubsequentAmount); }
        }

        public decimal? TotalInvestment
        {
            get { return TotalPremium + TotalCertificateOutlay + TotalSubsequentsOutlay; }
        }
        #endregion
        
        #region Earnings
        //@todo replace EarningsTypeId with enum
        public decimal? TotalCertificateInterestEarnings
        {
            get { return Property.Earnings.Where(e => e.EarningsTypeId == 1).Sum(e => e.Amount); }
        }

        public decimal? TotalSubsequentsInterestEarnings
        {
            get { return Property.Earnings.Where(e => e.EarningsTypeId == 2).Sum(e => e.Amount); }
        }
        
        public decimal? TotalInterestEarnings { get { return TotalCertificateInterestEarnings + TotalSubsequentsInterestEarnings;  } }

        public decimal? TwoFourSixPenalty { get { return Property.Earnings.Where(e => e.EarningsTypeId == 3).Sum(e => e.Amount); } }

        public decimal? YearEndPenalty { get { return Property.Earnings.Where(e => e.EarningsTypeId == 4).Sum(e => e.Amount); } }

        public decimal? LookUpFee { get { return Property.Earnings.Where(e => e.EarningsTypeId == 5).Sum(e => e.Amount); } }

        public decimal? MunicipalityMistake { get { return Property.Earnings.Where(e => e.EarningsTypeId == 6).Sum(e => e.Amount); } }

        public decimal? TotalEarnings { get { return Property.Earnings.Sum(e => e.Amount); } }
        #endregion
    }
}