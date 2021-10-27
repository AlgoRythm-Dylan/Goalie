using Goalie.Lib.Models;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

namespace Goalie
{
    /// <summary>
    /// Interaction logic for Transfer.xaml
    /// </summary>
    public partial class Transfer : Window
    {
        public Profile Profile { get; set; }
        public bool ShouldSave { get; set; }
        public Account SourceAccount { get; set; }
        public Account DestinationAccount { get; set; }
        public Transfer(Profile profile)
        {
            InitializeComponent();
            Profile = profile;
            ShouldSave = false;

            Source.ItemsSource = profile.Accounts;
            Destination.ItemsSource = profile.Accounts;
        }

        private void Source_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SourceAccount = Profile.GetAccountByID((string)Source.SelectedValue);
            SourceBalance.Text = $"Balance: {SourceAccount.Balance:C}";
        }

        private void Destination_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DestinationAccount = Profile.GetAccountByID((string)Destination.SelectedValue);
            DestinationBalance.Text = $"Balance: {DestinationAccount.Balance:C}";
        }

        private async void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal amount = decimal.Parse(Amount.Text, NumberStyles.Currency);
                if(SourceAccount == null || DestinationAccount == null)
                {
                    MessageBox.Show("Please select two accounts to transfer between", "Not Enough Accounts",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if(amount == 0)
                {
                    MessageBox.Show($"Transaction must not be {0:C}", "Empty Transaction",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string desc = Description.Text;
                if (desc.Length == 0)
                    desc = null;
                await SourceAccount.TransferAsync(Profile, DestinationAccount, amount, desc);
                ShouldSave = true;
                Close();
            }
            catch
            {
                MessageBox.Show("Please input a valid currency value", "Format Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Amount.Focus();
            }
        }
    }
}
