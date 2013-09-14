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
        private PuzzleBoard puzzleBoard;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            List<Square> squares = new List<Square>();
            InitializeSquares(squares);
            puzzleBoard = new PuzzleBoard(squares);

            mediaElement = new MediaElement();
            mediaElement.Source = new Uri("/Assets/Videos/gokarts.mp4", UriKind.Relative);
            mediaElement.Volume = 100;
            mediaElement.Play();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void InitializeSquares(List<Square> squares)
        {
            squares.Add(new Square(textBlock01, 1));
            squares.Add(new Square(textBlock02, 2));
            squares.Add(new Square(textBlock03, 3));
            squares.Add(new Square(textBlock04, 4));
            squares.Add(new Square(textBlock05, 5));
            squares.Add(new Square(textBlock06, 6));
            squares.Add(new Square(textBlock07, 7));
            squares.Add(new Square(textBlock08, 8));
            squares.Add(new Square(textBlock09, 9));
            squares.Add(new Square(textBlock10, 10));
            squares.Add(new Square(textBlock11, 11));
            squares.Add(null);
        }

        private void textBlock09_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            return;
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

        private void MainGrid_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (e.ManipulationContainer.GetType() == typeof (TextBlock)) 
            {
                TextBlock temp = (TextBlock)e.ManipulationContainer;
                int idx = ((int)Canvas.GetLeft(temp)) / 152 + ((int)Canvas.GetTop(temp)) / 152 * 3;
                puzzleBoard.MoveTile(idx);
                if (puzzleBoard.IsWon())
                    MessageBox.Show("You Won!! Wuhuu!");
            }
            
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            puzzleBoard.Shuffle();
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