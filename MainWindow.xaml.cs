using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Goalie.Lib;
using Goalie.Lib.Data;
using Goalie.Lib.Models;
using Goalie.Lib.UserControls;

namespace Goalie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Profile Profile { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            LoadAppData();
        }

        public async void LoadAppData()
        {
            var appData = await AppDataService.ReadAsync();
            if(appData == null || appData.CurrentProfileID == null)
            {
                var newProfileDialog = new NewProfile();
                newProfileDialog.Owner = this;
                newProfileDialog.ShowDialog();
                Profile profile = newProfileDialog.Profile;
                if (!newProfileDialog.ShouldSave || profile == null)
                {
                    Close();
                }
                else
                {
                    Profile = profile;
                    if(appData == null)
                        appData = new AppData();
                    appData.CurrentProfileID = profile.ID;
                    try
                    {
                        await AppDataService.WriteAsync(appData);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"FATAL: Could not save app data: {ex.Message}");
                        Close();
                    }
                    try
                    {
                        await ProfileService.WriteAsync(profile);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"ERROR: Could not save profile: {ex.Message}");
                        appData.CurrentProfileID = null;
                        await AppDataService.WriteAsync(appData);
                    }
                    DisplayProfile();
                }
            }
            else
            {
                Profile = await ProfileService.ReadAsync(appData.CurrentProfileID);
                DisplayProfile();
            }
        }

        public void DisplayProfile()
        {
            decimal totalSavings = 0;
            decimal savingsTowardsGoals = 0;
            int goalCount = 0;
            foreach(Account account in Profile.Accounts)
            {
                totalSavings += account.Balance;
                if (account.Type == Lib.AccountType.Goal)
                {
                    savingsTowardsGoals += account.Balance;
                    goalCount++;
                }
            }

            TotalSavings.Text = totalSavings.ToString("C");
            TotalSavingsTowardsGoals.Text = savingsTowardsGoals.ToString("C");
            GoalCount.Text = goalCount.ToString();

            DisplayAccounts();
        }

        protected void DisplayAccounts()
        {
            GoalScroll.Children.Clear();
            Account pinnedAccount = null;
            List<Account> nonPinnedAccounts = new List<Account>();
            foreach(Account account in Profile.Accounts)
            {
                if (account.ID == Profile.PinnedAccount)
                    pinnedAccount = account;
                else
                    nonPinnedAccounts.Add(account);
            }
            nonPinnedAccounts = nonPinnedAccounts.OrderBy(account => account.Name).ToList();
            if(pinnedAccount != null)
                GoalScroll.Children.Add(new GoalDisplay(pinnedAccount));
            foreach(Account account in nonPinnedAccounts)
            {
                var display = new GoalDisplay(account);
                GoalScroll.Children.Add(display);
            }
            foreach(GoalDisplay child in GoalScroll.Children)
            {
                child.Update += Display_Update;
                child.Margin = new Thickness(20, 5, 20, 5);
                child.Height = 100;
            }
            if(GoalScroll.Children.Count > 0)
            {
                if(GoalScroll.Children.Count == 1)
                {
                    ((GoalDisplay)GoalScroll.Children[0]).Margin = new Thickness(20, 20, 20, 20);
                }
                else
                {
                    ((GoalDisplay)GoalScroll.Children[0]).Margin = new Thickness(20, 20, 20, 5);
                    ((GoalDisplay)GoalScroll.Children[GoalScroll.Children.Count - 1]).Margin = new Thickness(20, 5, 20, 20);
                }
            }

        }

        private async void Display_Update(object sender, RoutedEventArgs e)
        {
            var ev = (AccountRoutedEventArgs)e;
            if (ev.ShouldSave)
            {
                var account = ev.Account;
                Profile.SetAccountByID(account.ID, account);
                await ProfileService.WriteAsync(Profile);
                DisplayProfile();
            }
            else if (ev.ShouldDelete)
            {
                var account = ev.Account;
                Profile.GeneralAccount.Balance += account.Balance;
                Profile.DeleteAccountByID(account.ID);
                AccountService.Delete(Profile, account.ID);
                await ProfileService.WriteAsync(Profile);
                DisplayProfile();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var editor = new EditGoal();
            editor.Owner = this;
            editor.ShowDialog();
            if (editor.ShouldSave)
            {
                if (editor.IsNewMode)
                    Profile.Accounts.Add(editor.Account);

                try
                {
                    await ProfileService.WriteAsync(Profile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to save changes: {ex.Message}");
                }
                DisplayProfile();
            }
        }

        private async void Transfer_Click(object sender, RoutedEventArgs e)
        {
            var transfer = new Transfer(Profile);
            transfer.Owner = this;
            transfer.ShowDialog();
            if (transfer.ShouldSave)
            {
                Profile.SetAccountByID(transfer.SourceAccount.ID, transfer.SourceAccount);
                Profile.SetAccountByID(transfer.DestinationAccount.ID, transfer.DestinationAccount);
                await ProfileService.WriteAsync(Profile);
                DisplayProfile();
            }
        }

        private void PaycheckButton_Click(object sender, RoutedEventArgs e)
        {
            var paycheck = new Paycheck(Profile);
            paycheck.Owner = this;
            paycheck.ShowDialog();
            if (paycheck.ShouldSave)
            {
                DisplayProfile(); // Profile will be updated in memory, no need to reload from disk
            }
        }
    }
}
