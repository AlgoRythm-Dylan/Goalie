using Goalie.Lib.Models;
using System.Windows;

namespace Goalie
{
    /// <summary>
    /// Interaction logic for Transfer.xaml
    /// </summary>
    public partial class Transfer : Window
    {
        public Profile Profile;
        public bool ShouldSave;
        public Transfer(Profile profile)
        {
            InitializeComponent();
            Profile = profile;
            ShouldSave = false;

            Source.ItemsSource = profile.Accounts;
            Destination.ItemsSource = profile.Accounts;
        }
    }
}
