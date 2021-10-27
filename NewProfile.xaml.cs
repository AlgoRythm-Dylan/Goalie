using Goalie.Lib;
using Goalie.Lib.Models;
using System;
using System.Globalization;
using System.Windows;

namespace Goalie
{
    /// <summary>
    /// Interaction logic for NewProfile.xaml
    /// </summary>
    public partial class NewProfile : Window, IShouldSave
    {
        public Profile Profile { get; set; }
        public bool ShouldSave { get; set; }
        public NewProfile()
        {
            InitializeComponent();
            Profile = null;
            ShouldSave = false;
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
                Profile.Name = ProfileName.Text;
                Profile.NewID();

                var generalSavingsAccount = new Account();
                generalSavingsAccount.NewID();
                generalSavingsAccount.Type = AccountType.GeneralSavings;
                generalSavingsAccount.Balance = initialSavings;
                generalSavingsAccount.Name = "General Savings";

                Profile.Accounts.Add(generalSavingsAccount);
                Profile.PinnedAccount = generalSavingsAccount.ID;

                ShouldSave = true;
                Close();
            }
            catch
            {
                MessageBox.Show("Initial savings must be a number");
            }
        }
    }
}
