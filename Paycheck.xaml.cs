using Goalie.Lib;
using Goalie.Lib.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Goalie
{
    /// <summary>
    /// Interaction logic for Paycheck.xaml
    /// </summary>
    public partial class Paycheck : Window
    {
        public Profile Profile { get; set; }
        public List<Account> Accounts { get; set; }
        public bool Save { get; set; }
        private List<AccountListView> AccountList { get; set; }
        public Paycheck(Profile profile)
        {
            InitializeComponent();
            Save = false;
            Profile = profile;
            List<Account> accounts = profile.Accounts
                                .Where(account => account.Type == AccountType.Goal &&
                                  account.SavingsType != GoalSavingsType.Manual).ToList();
            AccountList = new List<AccountListView>();
            foreach(Account account in accounts)
            {
                AccountList.Add(new AccountListView(account));
            }
            SelectGoalCheckboxes.ItemsSource = AccountList;
            NetPay.Focus();
            DoSummary();
        }

        private void SomethingChecked(object sender, RoutedEventArgs e)
        {
            DoSummary();
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DoSummary()
        {
            List<Account> selectedAccounts = new List<Account>();
            foreach (var account in AccountList.Where(item => item.IsSelected))
            {
                selectedAccounts.Add(account);
            }
            decimal minimumForGoals = PaycheckDistributor.MinimumRequired(selectedAccounts);
            Minimum.Text = $"Minimum: {minimumForGoals:C}";
            try
            {
                decimal income = decimal.Parse(NetPay.Text, NumberStyles.Currency);
                if(income < minimumForGoals)
                {
                    SummaryLabel.Text = $"Minimum not met (Required: {minimumForGoals:C}, provided: {income:C})";
                    return;
                }
                try
                {
                    var distribution = PaycheckDistributor.DistributePaycheck(selectedAccounts, income);
                    decimal distributionSum = 0;
                    foreach(var item in distribution)
                    {
                        distributionSum += item.Value;
                    }
                    SummaryLabel.Text = $"Distributing {distributionSum:C} towards goals and {income - distributionSum:C} to general savings";
                }
                catch (PaycheckDistributionError err)
                {
                    SummaryLabel.Text = $"Error: {err.Message}";
                }
            }
            catch
            {
                SummaryLabel.Text = "Please input a valid currency value";
            }
        }

        private void NetPay_TextChanged(object sender, TextChangedEventArgs e)
        {
            DoSummary();
        }

    }
}
