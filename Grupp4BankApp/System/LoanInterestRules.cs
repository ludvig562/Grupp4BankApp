using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Grupp4BankApp.System
{
    
    internal static class LoanInterestRules
    {
        // all the start intrest based on diffrent factors 
        public static decimal HighRate { get; private set; } = 0.05m;
        public static decimal MediumRate { get; private set; } = 0.08m;
        public static decimal LowRate { get; private set; } = 0.12m;

        public static decimal ShortTerm { get; private set; } = -0.01m;
        public static decimal MediumTerm { get; private set; } = 0.00m;
        public static decimal LongTerm { get; private set; } = 0.02m;

        // method to get the interest based on the rating
        public static decimal GetBaseInterest(Enums.CreditRating rating)
        {
            switch (rating)
            {
                case Enums.CreditRating.high:
                    return HighRate;
                case Enums.CreditRating.medium:
                    return MediumRate;
                case Enums.CreditRating.low:
                    return LowRate;
                default:
                    return MediumRate;
            }
        }

        // method to het the year intrest based on how logn they pay back the loan 
        public static decimal GetTermInterest(int years)
        {
            if (years <= 3)
            {
                return ShortTerm;
            }

            if (years <= 7)
            {
                return MediumTerm;
            }

            return LongTerm;
        }
        
        // method to caculate the total intrest rate
        public static decimal CalculateInterestRate(Enums.CreditRating rating, int years)
        {
            decimal baseInterest = GetBaseInterest(rating);
            decimal termInterest = GetTermInterest(years);

            return Math.Max(0m, baseInterest + termInterest);
        }

        // method to update the properties in this class for base interest
        public static void UpdateBaseInterest(Enums.CreditRating rating, decimal uppdate)
        {
            switch (rating)
            {
                case Enums.CreditRating.high:
                    HighRate = uppdate;
                    break;
                case Enums.CreditRating.low:
                    LowRate = uppdate;
                    break;
                case Enums.CreditRating.medium:
                    MediumRate = uppdate;
                    break;
            }
        }

        // method to update the properties in this class for years interest
        public static void UpdateTermInterest(int choise, decimal uppdate)
        {
            switch (choise)
            {
                case 1:
                    ShortTerm = uppdate;
                    break;
                case 2:
                    MediumTerm = uppdate;
                    break;
                case 3:
                    LongTerm = uppdate;
                    break;
            }
        }
    }
}

