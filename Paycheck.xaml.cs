using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Goalie.Lib.Models;

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
        public Paycheck(Profile profile)
        {
            InitializeComponent();
            Save = false;
            Profile = profile;
            List<Account> accounts = profile.Accounts
                                .Where(account => account.Type == Lib.AccountType.Goal &&
                                  account.SavingsType != Lib.GoalSavingsType.Manual).ToList();
            List<AccountListView> accountsList = new List<AccountListView>();
            foreach(Account account in accounts)
            {
                accountsList.Add(new AccountListView(account));
            }
            SelectGoalCheckboxes.ItemsSource = accountsList;
            NetPay.Focus();
        }

        private void SomethingChecked(object sender, RoutedEventArgs e)
        {

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

        }

    }
}
