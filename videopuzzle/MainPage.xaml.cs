using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using videopuzzle.Resources;
using System.Windows.Documents;

namespace videopuzzle
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MediaElement mediaElement;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            mediaElement = new MediaElement();
            mediaElement.Source = new Uri("/Assets/Videos/gokarts.mp4", UriKind.Relative);
            mediaElement.Volume = 100;
            mediaElement.Play();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void textBlock11_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // Create a WriteableBitmap and set it to the MediaElement (video).
            // The WriteableBitmap represents a "snapshot" of the video.
            WriteableBitmap wb = new WriteableBitmap(mediaElement, null);

            // Create an image of the desired size and set its source to
            // the WriteableBitmap representing a snapshot of the video.
            Image image = new Image();
            image.Height = 150;
            image.Margin = new Thickness(10);
            image.Source = wb;

            InlineUIContainer iuc = new InlineUIContainer();
            iuc.Child = image;
            textBlock11.Inlines.Add(iuc);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}