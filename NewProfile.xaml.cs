using Goalie.Lib.Models;
using Goalie.Lib;
using System;
using System.Collections.Generic;
using System.Windows;

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
            double initialSavings;
            try
            {
                initialSavings = double.Parse(InitialSavings.Text);

                Profile = new Profile();
                Profile.Name = Name.Text;
                Profile.NewID();

                var generalSavingsAccount = new Account();
                generalSavingsAccount.NewID();
                generalSavingsAccount.Type = AccountType.GeneralSavings;
                generalSavingsAccount.Balance = initialSavings;

                Profile.Accounts.Add(generalSavingsAccount);

                Close();
            }
            catch
            {
                MessageBox.Show("Initial savings must be a number");
            }
        }
    }
}
