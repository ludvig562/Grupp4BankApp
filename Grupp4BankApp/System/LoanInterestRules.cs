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

        public static decimal GetBaseInterest(Enums.CreditRating rating)// method to get the interest based on the rating
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

        public static decimal GetTermInterest(int years)// method to het the year intrest based on how logn they pay back the loan 
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
        public static decimal CalculateInterestRate(Enums.CreditRating rating, int years)// method to caculate the total intrest rate
        {
            decimal baseInterest = GetBaseInterest(rating);
            decimal termInterest = GetTermInterest(years);

            return Math.Max(0m, baseInterest + termInterest);
        }

        public static void UpdateBaseInterest(Enums.CreditRating rating, decimal uppdate)// method to update the properties in this class for base interest
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

        public static void UpdateTermInterest(int choise, decimal uppdate)// method to update the properties in this class for years interest
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

