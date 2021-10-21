using Goalie.Lib.Data;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Goalie.Lib.Models
{
    public class Account : UniqueModel
    {
        public Account()
        {
            Name = "";
            Type = AccountType.GeneralSavings;
            SavingsType = GoalSavingsType.Fixed;
            FixedGoal = null;
            ContinueSavingAfterGoalMet = true;
            CreatedDate = DateTime.Now;
        }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public AccountType Type { get; set; }
        public GoalSavingsType SavingsType { get; set; }
        public decimal? SavingsAmount { get; set; }
        public decimal? FixedGoal { get; set; }
        public bool ContinueSavingAfterGoalMet { get; set; }
        // Stats, just as an interesting metric for the user
        public int TransactionsAllTime { get; set; }
        public decimal SavedAllTime { get; set; }
        public decimal SpentAllTime { get; set; }
        public DateTime CreatedDate { get; set; }

        // Behavior
        public DataDir GetTransactionDataDir(Profile profile)
        {
            return new DataDir("transactions", AccountService.GetAccountDataDir(profile, this.ID));
        }
        public async Task TransferAsync(Profile profile, Account otherAccount, decimal amount, string desc=null)
        {
            Balance += amount;
            otherAccount.Balance -= amount;
            Transaction thisTransaction = new Transaction(), otherTransaction = new Transaction();

            if (desc != null && desc == "")
                desc = null;

            thisTransaction.NewID();
            otherTransaction.NewID();

            thisTransaction.Amount = amount;
            otherTransaction.Amount = amount;

            thisTransaction.OtherAccountID = otherAccount.ID;
            otherTransaction.OtherAccountID = ID;

            thisTransaction.Description = desc;
            otherTransaction.Description = desc;

            TransactionsAllTime++;
            otherAccount.TransactionsAllTime++;

            // Start the tasks to get them going then "join" them by awaiting them all
            var task1 = RecordTransactionAsync(profile, thisTransaction);
            var task2 = otherAccount.RecordTransactionAsync(profile, otherTransaction);

            await task1;
            await task2;

        }
        public async Task RecordTransactionAsync(Profile profile, Transaction transaction)
        {
            DataDir transactionDataDir = GetTransactionDataDir(profile);
            transactionDataDir.Ensure();
            await File.WriteAllTextAsync(Path.Combine(transactionDataDir.Path, $"{transaction.ID}.transaction.json"),
                JsonSerializer.Serialize(transaction));
        }

    }
}
