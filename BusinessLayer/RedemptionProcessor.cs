using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class RedemptionProcessor
    {
        private readonly EntityManager _entityManager = new EntityManager();

        public RedemptionProcessor(int propertyId)
        {
            Property = _entityManager.Property(propertyId);
        }

        public void SetRedemptionAndAccrual(DateTime redemptionDate)
        {
            foreach (Certificate certificate in Property.Certificates)
            {
                certificate.RedemptionDate = redemptionDate;
                TimeSpan? accrualSpan = certificate.RedemptionDate.Value - certificate.DateOfPurchase;
                certificate.AccrualPeriod = accrualSpan.Value.Days;
                _entityManager.Update(certificate);
            }

            

            foreach (Subsequent subsequent in Property.Subsequents)
            {
                TimeSpan? accrualSpan = redemptionDate - subsequent.OutLayDate;
                subsequent.AccrualPeriod = accrualSpan.Value.Days;
                _entityManager.Update(subsequent);
            }
        }
        
        public void CreateEarnings()
        {
            //@todo replace earningsTypeId with enum
            List<Earning> earnings = new List<Earning>();

            Earning certificateInterest = EarningsFactory.GenerateEarning(Property, 1,
                                                                          EarningsCalculator
                                                                              .CalculateCertificatesInterest);

            EarningsCalculator.CalculateEachSubsequentInterest(Property.Subsequents);
            foreach (Subsequent subsequent in Property.Subsequents)
            {
                _entityManager.Update(subsequent);
            }
            Earning subsequentsInterest = EarningsFactory.GenerateEarning(Property, 2,
                                                                          EarningsCalculator
                                                                              .CalculateTotalSubsequentsInterest);
            Earning twoFourSixPenalty = EarningsFactory.GenerateEarning(Property, 3,
                                                                        EarningsCalculator.Calculate246Penalty);
            Earning yearEndPenalty = EarningsFactory.GenerateEarning(Property, 4,
                                                                     EarningsCalculator.CalculateYearEndPenalty);
            Earning lookUpFee = EarningsFactory.GenerateEarning(Property, 5, EarningsCalculator.CalculateLookUpFee);

            earnings.Add(certificateInterest);
            earnings.Add(subsequentsInterest);
            earnings.Add(twoFourSixPenalty);
            earnings.Add(yearEndPenalty);
            earnings.Add(lookUpFee);

            _entityManager.Add(earnings.Where(e => e.Amount > 0));
        }
        
        public Property Property { get; private set; }
    }
}