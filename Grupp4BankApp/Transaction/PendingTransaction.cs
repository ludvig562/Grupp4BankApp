using System;
using Grupp4BankApp.System;

namespace Grupp4BankApp.Transaction
{
    internal class PendingTransaction
    {
        // Account from which the money will be sent
        public BankAccount FromAccount { get; }

        // Account that will receive the money
        public BankAccount ToAccount { get; }

        // Amount in the sender's currency
        public decimal Amount { get; }

        // Time when the transaction can be executed
        public DateTimeOffset ExecuteAtUtc { get; }

        public PendingTransaction(BankAccount fromAccount,BankAccount toAccount,decimal amount)
        {
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Amount = amount;

            // Schedule the transaction 15 minutes after creation
            ExecuteAtUtc = DateTimeOffset.UtcNow.AddMinutes(15);
        }
    }
}