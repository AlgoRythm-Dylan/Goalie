namespace Goalie.Lib.Models
{
    public class AccountListView : Account
    {
        public AccountListView(Account account)
        {
            Name = account.Name;
            ID = account.ID;
            Balance = account.Balance;
            Type = account.Type;
            SavingsType = account.SavingsType;
            SavingsAmount = account.SavingsAmount;
            FixedGoal = account.FixedGoal;
            ContinueSavingAfterGoalMet = account.ContinueSavingAfterGoalMet;
            TransactionsAllTime = account.TransactionsAllTime;
            SavedAllTime = account.SavedAllTime;
            SpentAllTime = account.SpentAllTime;
            CreatedDate = account.CreatedDate;
            Enabled = account.Enabled;
            IsSelected = true;
        }
        public bool IsSelected { get; set; }
    }
}
