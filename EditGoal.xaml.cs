using Goalie.Lib;
using Goalie.Lib.Models;
using System.Globalization;
using System.Windows;

namespace Goalie
{
    /// <summary>
    /// Interaction logic for EditGoal.xaml
    /// </summary>
    public partial class EditGoal : Window, IShouldSave, IShouldDelete
    {
        public Account Account { get; set; }
        public bool IsNewMode { get; set; }
        public bool ShouldSave { get; set; }
        public bool ShouldDelete { get; set; }
        public EditGoal()
        {
            InitializeComponent();
            IsNewMode = true;
            ShouldSave = false;
            ShouldDelete = false;
            Account = new Account();
            Account.Type = AccountType.Goal;
            Account.SavingsAmount = 5;
            Account.SavingsType = GoalSavingsType.Percentage;
            Account.Name = "My New Goal";
            Account.ContinueSavingAfterGoalMet = true;
            DisplayAccount();
        }
        public EditGoal(Account account)
        {
            InitializeComponent();
            IsNewMode = false;
            ShouldSave = false;
            ShouldDelete = false;
            Account = account;
            DisplayAccount();
        }
        protected void DisplayAccount()
        {
            if (IsNewMode)
            {
                Title = "New Goal";
                GoalLabel.Text = "New Goal";
                DeleteButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                if(Account.Type == AccountType.GeneralSavings)
                {
                    AmountPerPaycheck.Visibility = Visibility.Collapsed;
                    GoalSettings.Visibility = Visibility.Collapsed;
                    ContinueSettings.Visibility = Visibility.Collapsed;
                    Title = $"Edit Account \"{Account.Name}\"";
                    GoalLabel.Text = $"Edit Account \"{Account.Name}\"";
                    DeleteButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Title = $"Edit Goal \"{Account.Name}\"";
                    GoalLabel.Text = $"Edit Goal \"{Account.Name}\"";
                }
            }
            SavingsType.ItemsSource = DropDownItems.GoalSavingsTypes;
            AccountName.Text = Account.Name;
            InitialBalance.Text = Account.Balance.ToString("C");
            SavingsAmount.Text = Account.SavingsAmount.ToString();
            SavingsType.SelectedValue = Account.SavingsType;
            Goal.Text = (Account.FixedGoal ?? (decimal)0.0).ToString("C");
            ContinueAfterGoalMet.IsChecked = Account.ContinueSavingAfterGoalMet;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            decimal balance;
            decimal? goal = null, savingsAmount = null;
            try
            {
                balance = decimal.Parse(InitialBalance.Text, NumberStyles.Currency);
            }
            catch
            {
                MessageBox.Show("\"Balance\" must be a numerical value");
                return;
            }
            try
            {
                goal = decimal.Parse(Goal.Text, NumberStyles.Currency);
                if (goal == 0)
                    goal = null;
            }
            catch { }

            try
            {
                savingsAmount = decimal.Parse(SavingsAmount.Text, NumberStyles.Currency);
                if (savingsAmount == 0)
                    savingsAmount = null;
            }
            catch { }

            Account.Name = AccountName.Text;
            Account.Balance = balance;

            if (Account.Type != AccountType.GeneralSavings)
            {
                Account.SavingsAmount = savingsAmount;
                Account.SavingsType = (GoalSavingsType)SavingsType.SelectedValue;
                Account.FixedGoal = goal;
                Account.ContinueSavingAfterGoalMet = ContinueAfterGoalMet.IsChecked ?? true;
            }
            if (IsNewMode)
                Account.NewID();

            ShouldSave = true;
            Close();

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Delete this goal? The balance will be transferred to the general savings account",
                "Confirm", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ShouldDelete = true;
                Close();
            }
        }
    }
}
