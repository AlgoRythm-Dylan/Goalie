using Goalie.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Goalie.Lib.UserControls
{
    /// <summary>
    /// Interaction logic for GoalDisplay.xaml
    /// </summary>
    public partial class GoalDisplay : UserControl
    {
        public Account Account { get; set; }
        public GoalDisplay()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext ctx)
        {
            base.OnRender(ctx);
            UpdateGoalProgressbar();
        }

        private void UpdateGoalProgressbar()
        {
            if (Account.Type != AccountType.Goal || Account.FixedGoal == null
                || double.IsNaN(GoalProgressbarOuter.ActualWidth) || GoalProgressbarOuter.ActualWidth <= 0)
                return;
            double progressBarWidth = (double)(Account.Balance / (Account.FixedGoal ?? 0)) * GoalProgressbarOuter.ActualWidth;
            if (progressBarWidth > GoalProgressbarOuter.ActualWidth)
                progressBarWidth = GoalProgressbarOuter.ActualWidth; // Prevent overflow of accounts at more than 100%
            GoalProgressbarInner.Width = progressBarWidth;
            int remainingPixels = (int)(GoalProgressbarOuter.ActualWidth - progressBarWidth);
            if(remainingPixels <= 3)
            {
                GoalProgressbarInner.CornerRadius = new CornerRadius(0, 0, 3 - remainingPixels, 3);
            }
            else
            {
                GoalProgressbarInner.CornerRadius = new CornerRadius(0, 0, 0, 3);
            }
        }

        public GoalDisplay(Account account)
        {
            InitializeComponent();
            SetAccount(account);
        }
        public void SetAccount(Account account)
        {
            Account = account;
            GoalName.Text = Account.Name;
            if(Account.Type == AccountType.GeneralSavings)
            {
                GoalRules.Text = "General Savings Account";
                CurrentBalance.Visibility = Visibility.Collapsed;
                OfLabel.Visibility = Visibility.Collapsed;
                GoalAmountDisplay.Visibility = Visibility.Collapsed;
                GoalProgressPercentageText.Text = $"{account.Balance:C}";
                GoalProgressbarOuter.Visibility = Visibility.Collapsed;
            }
            else
            {
                // "Savings rule" display
                if(Account.SavingsType == GoalSavingsType.Manual)
                {
                    GoalRules.Text = "Manually Manged";
                }
                else if (Account.SavingsType == GoalSavingsType.Percentage)
                {
                    GoalRules.Text = $"{Account.SavingsAmount}% of each paycheck";
                }
                else if (Account.SavingsType == GoalSavingsType.Fixed)
                {
                    GoalRules.Text = $"${Account.SavingsAmount} from each paycheck";
                }
                // Account balance (and goal if present) display
                if(Account.FixedGoal != null && Account.FixedGoal != 0)
                {
                    CurrentBalance.Text = Account.Balance.ToString("C");
                    //                       At this point, this is safe, but compiler complains
                    GoalAmountDisplay.Text = (Account.FixedGoal??0).ToString("C");
                    GoalProgressPercentageText.Text = Math.Round(Account.Balance / (Account.FixedGoal ?? 0) * 100).ToString("0") + "%";
                    UpdateGoalProgressbar();
                }
                else
                {
                    CurrentBalance.Visibility = Visibility.Collapsed;
                    OfLabel.Visibility = Visibility.Collapsed;
                    GoalProgressPercentageText.Text = Account.Balance.ToString("C");
                    GoalAmountDisplay.Visibility = Visibility.Collapsed;
                    GoalProgressbarOuter.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
