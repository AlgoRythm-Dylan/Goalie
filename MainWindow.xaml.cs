using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Goalie.Lib.Models;

namespace Goalie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            var newProfile = new NewProfile();
            newProfile.Owner = this;
            newProfile.ShowDialog();
            Profile profile = newProfile.Profile;
            if(profile == null)
            {
                Close();
            }
        }
    }
}
