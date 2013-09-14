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
        public Settings()
        {
            InitializeComponent();
            LoadSettings();
        }

        // Load application settings from memory
        private void LoadSettings()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("gridMode"))
            {
                int gridMode = (int)IsolatedStorageSettings.ApplicationSettings["gridMode"];
                if (gridMode == 0)
                {
                    radioButton1.IsChecked = true;
                }
                else
                {
                    radioButton2.IsChecked = true;
                }
            }
            else
            {
                radioButton1.IsChecked = true;
            }
            // add event handlers
            radioButton1.Checked += RadioButton1_Checked;
            radioButton2.Checked += RadioButton2_Checked;

            // Load offline mode settings from memory
            if (IsolatedStorageSettings.ApplicationSettings.Contains("offlineMode"))
            {
                int offlineMode = (int)IsolatedStorageSettings.ApplicationSettings["offlineMode"];
                if(offlineMode==1)
                    offlineToggle.IsChecked = true;
                offlineToggle.IsChecked = false;
            }
            // add event handlers
            offlineToggle.Checked += ToggleSwitch_Checked;
            offlineToggle.Unchecked += ToggleSwitch_Unchecked;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void RadioButton1_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            // gridMode describes the grid layout. 0 = 3x4, 1 = 6x8            
            if (!settings.Contains("gridMode"))
            {
                settings.Add("gridMode", 0);
            }
            else
            {
                settings["gridMode"] = 0;
            }
            settings.Save();

        }

        private void RadioButton2_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            // gridMode describes the grid layout. 0 = 3x4, 1 = 6x8
            if (!settings.Contains("gridMode"))
            {
                settings.Add("gridMode", 1);
            }
            else
            {
                settings["gridMode"] = 1;
            }
            settings.Save();
        }

        // set Offline-mode on
        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
           
           
        }

        // set Offline-mode off
        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
           
            
        }

        
    }
}