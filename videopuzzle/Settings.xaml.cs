using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace videopuzzle
{
    public partial class Settings : PhoneApplicationPage
    {
        private bool challengeMode = false;
        private bool offlineMode = false;
        private int filterIndex = 0;

        public Settings()
        {
            InitializeComponent();
            LoadSettings();
        }

        // Load application settings from memory
        private void LoadSettings()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("challengeMode"))
            {
                challengeMode = (bool)IsolatedStorageSettings.ApplicationSettings["challengeMode"];
            }
            radioButton1.IsChecked = !challengeMode;
            radioButton2.IsChecked = challengeMode;

            // Load offline mode settings from memory
            if (IsolatedStorageSettings.ApplicationSettings.Contains("offlineMode"))
            {
                offlineMode = (bool)IsolatedStorageSettings.ApplicationSettings["offlineMode"];
            }
            offlineToggle.IsChecked = offlineMode;

            // Load filter index settings from memory
            if (IsolatedStorageSettings.ApplicationSettings.Contains("filterIndex"))
            {
                filterIndex = (int)IsolatedStorageSettings.ApplicationSettings["filterIndex"];
            }
            filterPicker.SelectedIndex = filterIndex;


        }

        private void CancelSettings(object sender, RoutedEventArgs e)
        {
            if(NavigationService.CanGoBack) NavigationService.GoBack();
            else NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void SaveSettings(object sender, System.Windows.Input.GestureEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains("challengeMode"))
            {
                settings.Add("challengeMode", false);
            }
            if (!settings.Contains("offlineMode"))
            {
                settings.Add("offlineMode", false);
            }
            if (!settings.Contains("filterIndex"))
            {
                settings.Add("filterIndex", 0);
            }
            settings["challengeMode"] = radioButton2.IsChecked;
            settings["offlineMode"] = offlineToggle.IsChecked;
            settings["filterIndex"] = filterPicker.SelectedIndex;
            settings.Save();
            if (NavigationService.CanGoBack) NavigationService.GoBack();
            else NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        
    }
}