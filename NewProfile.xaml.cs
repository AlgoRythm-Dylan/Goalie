using Goalie.Lib.Models;
using Goalie.Lib;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Globalization;

namespace Goalie
{
    /// <summary>
    /// Interaction logic for NewProfile.xaml
    /// </summary>
    public partial class NewProfile : Window
    {
        public Profile Profile { get; set; }
        public NewProfile()
        {
            InitializeComponent();
            Profile = null;
        }
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            decimal initialSavings;
            try
            {
                initialSavings = decimal.Parse(InitialSavings.Text, NumberStyles.Currency);

                Profile = new Profile();
                Profile.Name = Name.Text;
                Profile.NewID();

                var generalSavingsAccount = new Account();
                generalSavingsAccount.NewID();
                generalSavingsAccount.Type = AccountType.GeneralSavings;
                generalSavingsAccount.Balance = initialSavings;
                generalSavingsAccount.Name = "General Savings";

                Profile.Accounts.Add(generalSavingsAccount);
                Profile.PinnedAccount = generalSavingsAccount.ID;

                Close();
            }
            catch
            {
                MessageBox.Show("Initial savings must be a number");
            }
        }
    }
}
