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
            Enabled = true;
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
        public bool Enabled { get; set; }

        public override string ToString()
        {
            string description;
            if(Type == AccountType.GeneralSavings)
            {
                description = $"General Savings Account ({Balance:C})";
            }
            else
            {
                string rules;
                if(SavingsType == GoalSavingsType.Percentage)
                {
                    rules = $"{SavingsAmount}% of each paycheck";
                }
                else if(SavingsType == GoalSavingsType.Fixed)
                {
                    rules = $"{SavingsAmount:C} from each paycheck";
                }
                else
                {
                    rules = "managed manually";
                }
                description = $"{rules} (balance: {Balance:C})";
            }
            return $"{Name} - {description}";
        }

        // Behavior
        public DataDir GetTransactionDataDir(Profile profile, Transaction transaction)
        {
            var txnDir = new DataDir("transactions", AccountService.GetAccountDataDir(profile, ID));
            return new DataDir($"{transaction.Date.Month}-{transaction.Date.Year}", txnDir);

        }
        public async Task TransferAsync(Profile profile, Account destination, decimal amount, string desc=null)
        {
            Balance -= amount;
            destination.Balance += amount;
            Transaction thisTransaction = new Transaction(), otherTransaction = new Transaction();

            if (desc != null && desc == "")
                desc = null;

            thisTransaction.NewID();
            otherTransaction.NewID();

            thisTransaction.Amount = -amount;
            otherTransaction.Amount = amount;

            thisTransaction.OtherAccountID = destination.ID;
            otherTransaction.OtherAccountID = ID;

            thisTransaction.Description = desc;
            otherTransaction.Description = desc;

            TransactionsAllTime++;
            destination.TransactionsAllTime++;

            // Start the tasks to get them going then "join" them by awaiting them all
            var task1 = RecordTransactionAsync(profile, thisTransaction);
            var task2 = destination.RecordTransactionAsync(profile, otherTransaction);

            await task1;
            await task2;

        }
        public async Task RecordTransactionAsync(Profile profile, Transaction transaction)
        {
            DataDir transactionDataDir = GetTransactionDataDir(profile, transaction);
            transactionDataDir.Ensure();
            await File.WriteAllTextAsync(Path.Combine(transactionDataDir.Path, $"{transaction.ID}.transaction.json"),
                JsonSerializer.Serialize(transaction));
        }

    }
}
