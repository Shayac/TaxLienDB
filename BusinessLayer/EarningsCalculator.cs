using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class EarningsCalculator
    {
        private Property _property;

        public EarningsCalculator(Property property)
        {
            _property = property;
        }

        //@todo delete all this stuff after verifying that new stuff works
//        public static Earning CalculateCertificatesInterest(Certificate certificate, int propertyId)
//        {
//            Earning earning = new Earning();
//            if (certificate.InterestRate != null && certificate.InterestRate > 0 && certificate.AccrualPeriod != null)
//            {
//                earning.EarningsTypeId = 1;
//                earning.PropertyId = propertyId;
//
//                earning.Amount = ((certificate.LienAmount*(decimal) certificate.InterestRate)/365)*
//                                 certificate.AccrualPeriod.Value;
//            }
//            return earning;
//        }

//        public static Earning CalculateSubsequentsInterest(IEnumerable<Subsequent> subsequents, int propertyId)
        //fills each subsequent.SubInterest prop
//        {
//            Earning earning = new Earning();
//            decimal? totalEarnings = null;
//            bool first = true;
//            foreach (Subsequent subsequent in subsequents)
//            {
//                if (subsequent.AccrualPeriod != null)
//                {

//                    decimal below1500Accrual = 0m;
//                    decimal above1500Accrual = 0m;
//                    if (subsequent.Below1500 != 0)
//                    {
//                        below1500Accrual =
//                            Math.Round(((subsequent.Below1500 * .08m) / 365m) * (subsequent.AccrualPeriod.Value + 1), 2);
//                    }

//                    if (subsequent.Above1500 != 0)
//                    {
//                        above1500Accrual =
//                            Math.Round(((subsequent.Above1500 * .18m) / 365m) * (subsequent.AccrualPeriod.Value + 1), 2);
//                    }
//                    subsequent.InterestEarnings = above1500Accrual + below1500Accrual;

//                    if (subsequent.InterestEarnings != null)
//                    {
//                        if (first)
//                        {
//                            totalEarnings = 0m;
//                            first = false;
//                        }
//                        totalEarnings += subsequent.InterestEarnings;
//                    }

//                }


//            }
//            return earning;
//        }

        public static decimal Calculate246Penalty(Property property)
        {
            decimal penalty = 0m;
            decimal amount = property.Certificates.Sum(c => c.LienAmount);
            if (amount >= 200.00m && amount <= 4999.99m)
            {
                penalty = amount*.02m;
            }
            else if (amount >= 5000.00m && amount <= 9999.99m)
            {
                penalty = amount*.04m;
            }
            else if (amount >= 10000.00m)
            {
                penalty = amount*.06m;
            }

            return penalty;
        }

        public static decimal CalculateYearEndPenalty(Property property)
        {
            decimal totalApplicableSubsequents = 0m;
            decimal yearEndPenalty = 0;

            if (property.Subsequents.Any()) //if there are subsequents:
            {
                if (property.Municipality.Calendar) //and the municipality uses the calendar year:
                {
                    totalApplicableSubsequents = property.Subsequents.Where(
                        s => s.OutLayDate.Year == property.Certificates.First().DateOfPurchase.Year)
                                                         .Sum(s => s.SubsequentAmount);
                    // total all subs until end of year
                }
                else if (property.Municipality.Fiscal) //else the municipality uses fiscal year:
                {
                    DateTime endOfFiscalYear = new DateTime(property.Certificates.First().DateOfPurchase.Year, 9, 30);
                    //set fiscal year end to the year of certificate purchase
                    totalApplicableSubsequents = property.Subsequents.Where(
                        s => s.OutLayDate <= endOfFiscalYear).Sum(s => s.SubsequentAmount);
                    // total all subs until end of fiscal year
                }
            }

            if (totalApplicableSubsequents > 10000m)
                // if total of all subs until end of year is greater than 10,000 , return y/e/p of 6%
            {
                yearEndPenalty = totalApplicableSubsequents*.06m;
            }

            return yearEndPenalty;
        }

        public static decimal CalculateLookUpFee(Property property)
        {
            if (!property.Municipality.LookUpRequirer || property.Certificates.First().LookedUp == true)
            {
                return 12m;
            }
            return 0;
        }

        public static decimal CalculateCertificatesInterest(Property property)
        {
            decimal total = 0m;
            foreach (Certificate certificate in property.Certificates)
            {
                if (certificate.InterestRate != null && certificate.AccrualPeriod != null &&
                    certificate.InterestRate > 0)
                {
                    total += ((certificate.LienAmount*(decimal) certificate.InterestRate)/365)*
                             certificate.AccrualPeriod.Value;
                }
            }
            return total;
        }

        public static decimal CalculateTotalSubsequentsInterest(Property property)
        {
            return property.Subsequents.Sum(s => s.InterestEarnings ?? 0);
        }

        public static void CalculateEachSubsequentInterest(IEnumerable<Subsequent> subsequents)
        {
            foreach (Subsequent subsequent in subsequents)
            {
                if (subsequent.AccrualPeriod != null)
                {
                    decimal below1500Accrual = 0m;
                    decimal above1500Accrual = 0m;
                    if (subsequent.Below1500 != 0)
                    {
                        below1500Accrual =
                            Math.Round(((subsequent.Below1500*.08m)/365m)*(subsequent.AccrualPeriod.Value + 1), 2);
                    }

                    if (subsequent.Above1500 != 0)
                    {
                        above1500Accrual =
                            Math.Round(((subsequent.Above1500*.18m)/365m)*(subsequent.AccrualPeriod.Value + 1), 2);
                    }
                    subsequent.InterestEarnings = above1500Accrual + below1500Accrual;
                }
            }
        }
    }
}