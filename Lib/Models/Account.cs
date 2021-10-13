namespace Goalie.Lib.Models
{
    public class Account
    {
        public Account()
        {
            Name = "";
            Type = AccountType.GeneralSavings;
            SavingsType = GoalSavingsType.Fixed;
            FixedGoal = null;
            ContinueSavingAfterGoalMet = true;
        }
        public string ID { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public AccountType Type { get; set; }
        public GoalSavingsType SavingsType { get; set; }
        public double SavingsAmount { get; set; }
        public double? FixedGoal { get; set; }
        public bool ContinueSavingAfterGoalMet { get; set; }
    }
}
