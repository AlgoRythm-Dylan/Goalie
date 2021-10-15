using System;
using System.Windows;
using Goalie.Lib.Data;
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
                if (profile == null)
                {
                    Close();
                }
                else
                {
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
                }
            }
        }

    }
}
