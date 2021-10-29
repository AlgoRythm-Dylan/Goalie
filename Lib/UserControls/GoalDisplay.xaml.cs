using Goalie.Lib.Data;
using Goalie.Lib.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Goalie.Lib.UserControls
{
    /// <summary>
    /// Interaction logic for GoalDisplay.xaml
    /// </summary>
    public partial class GoalDisplay : UserControl
    {
        public Account Account { get; set; }
        private bool ShouldSave { get; set; }
        private bool ShouldDelete { get; set; }
        public static readonly RoutedEvent UpdateEvent = EventManager.RegisterRoutedEvent(
            "Update", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GoalDisplay));
        public GoalDisplay()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler Update
        {
            add { AddHandler(UpdateEvent, value); }
            remove { RemoveHandler(UpdateEvent, value); }
        }

        void RaiseUpdateEvent()
        {
            AccountRoutedEventArgs newEventArgs = new AccountRoutedEventArgs(UpdateEvent, Account);
            newEventArgs.ShouldSave = ShouldSave;
            newEventArgs.ShouldDelete = ShouldDelete;
            RaiseEvent(newEventArgs);
        }

        protected override void OnRender(DrawingContext ctx)
        {
            base.OnRender(ctx);
            UpdateGoalProgressbar();
        }

        public GoalDisplay(Account account)
        {
            InitializeComponent();
            SetAccount(account);
            ShouldSave = false;
            ShouldDelete = false;
        }

        private void UpdateGoalProgressbar()
        {
            if (Account.Type != AccountType.Goal || Account.FixedGoal == null
                || double.IsNaN(GoalProgressbarOuter.ActualWidth) || GoalProgressbarOuter.ActualWidth <= 0)
                return;
            double progressBarWidth = (double)(Account.Balance / (Account.FixedGoal ?? 0)) * GoalProgressbarOuter.ActualWidth;
            if (progressBarWidth > GoalProgressbarOuter.ActualWidth)
                progressBarWidth = GoalProgressbarOuter.ActualWidth; // Prevent overflow of accounts at more than 100%
            GoalProgressbarInner.Width = progressBarWidth > 0 ? progressBarWidth : 0;
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
                    GoalRules.Text = $"{Account.SavingsAmount??0:C} from each paycheck";
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

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var editor = new EditGoal(Account);
            editor.Owner = Window.GetWindow(this);
            editor.ShowDialog();
            Account = editor.Account;
            ShouldSave = editor.ShouldSave;
            ShouldDelete = editor.ShouldDelete;
            RaiseUpdateEvent();
        }
    }
}
