using System;
using System.Collections.Generic;
using System.Text;
using Grupp4BankApp.System;
namespace Grupp4BankApp.Transaction
{
    struct Amount
    {
        public Decimal Value;
        public string PlusMinus;
        public Enums.CurrencyTypes Currency;
    }
    internal class TransactionLogg
    {
        public DateTime Date { get; set; }

        public Amount Amount { get; set; } = new Amount();
        public string Description { get; set; } = string.Empty;
        public string FromAccountName { get; set; } = string.Empty;
        public string ToAccountName { get; set; } = string.Empty;
        public string FromCustomerName { get; set; } = string.Empty;
        public string ToCustomerName { get; set; } = string.Empty;


        public override string ToString()
        {
            return
               $"{Date:yyyy-MM-dd HH:mm}\n" +
               $"{Description}\n" +
               $"Från: {FromAccountName} ({FromCustomerName})\n" +
               $"Till: {ToAccountName} ({ToCustomerName})\n" +
               $"Belopp: {Amount.PlusMinus}{Amount.Value:N2} {Amount.Currency}\n";
        }

    }


}
