using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public static class EarningsCalculator
    {
        public static void CalculateCertificatesInterest(Certificate certificate, int propertyId)
        {
            if (certificate.InterestRate != null && certificate.InterestRate > 0)
            {
                certificate.Earning = new Earning();
                certificate.Earning.Id = certificate.Id;
                certificate.Earning.EarningsTypeId = 1;
                certificate.Earning.PropertyId = propertyId;

                certificate.Earning.Amount = ((certificate.LienAmount*(decimal) certificate.InterestRate)/365)*
                                             certificate.AccrualPeriod.Value.Days;
            }
        }

        public static void CalculateSubsequentInterest(Subsequent subsequent) //fills each subsequent.SubInterest prop
        {
            if (subsequent.AccrualPeriod != null)
            {
                 decimal below1500Accrual = 0m;
                 decimal above1500Accrual = 0m;
                if (subsequent.Below1500 != 0)
                {
                    below1500Accrual =
                        Math.Round(((subsequent.Below1500*.08m)/365m)*(subsequent.AccrualPeriod.Value.Days + 1), 2);
                }

                if (subsequent.Above1500 != 0)
                {
                 above1500Accrual = Math.Round(((subsequent.Above1500*.18m)/365m)*(subsequent.AccrualPeriod.Value.Days+ 1),2);
                }
                subsequent.InterestEarnings = above1500Accrual + below1500Accrual;
            }
        }

        public static void Calculate246Penalty(Property property)
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

            property.Earnings.Add(new Earning()
                {
                    Amount = Math.Round(penalty, 2),
                    EarningsTypeId = 2,
                    PropertyId = property.Id
                   
                });
        }

        public static void CalculateYearEndPenalty(Property property)
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

            property.Earnings.Add(new Earning()
                {
                    Amount = Math.Round(yearEndPenalty, 2),
                    EarningsTypeId = 3,
                    PropertyId = property.Id
                    
                });
        }

        public static void CalculateLookUpFee(Property property)
        {
            if (!property.Municipality.LookUpRequirer || property.Certificates.First().LookedUp == true)
            {
                property.Earnings.Add(new Earning()
                    {
                        Amount = 12m,
                        EarningsTypeId = 4,
                        PropertyId = property.Id
                    });
            }
        }
    }
}