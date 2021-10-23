using Goalie.Lib.Models;
using System.Windows;
using System.Windows.Controls;

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
    }
}
