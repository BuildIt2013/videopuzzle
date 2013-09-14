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
using System.Windows.Media;
using Microsoft.Phone.Tasks;
using Nokia.Graphics.Imaging;
using Nokia.InteropServices.WindowsRuntime;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Xna.Framework.Media; 


namespace videopuzzle
{
    public partial class MainPage : PhoneApplicationPage
    {
        private PuzzleBoard puzzleBoard;
        private List<Square> squares;
        private EditingSession _session;
        private List<Image> images;
        Random rand;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            squares = new List<Square>();
            images = new List<Image>();
            InitializeSquares();
            puzzleBoard = new PuzzleBoard(squares);
            rand = new Random();
            SetImageBackgrounds();
        }


        private void SetImageBackgrounds() {

            WebClient client = new WebClient();
            client.OpenReadCompleted += client_OpenReadCompleted;
            client.OpenReadAsync(new Uri("http://lorempixel.com/450/600/?v=" + Guid.NewGuid(), UriKind.Absolute));
            progressbarIndeterminateDownload.Visibility = System.Windows.Visibility.Visible;
            progressbarDescription.Visibility = System.Windows.Visibility.Visible;
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                SplitImage(e.Result);
            }
        }

        private async void SplitImage(Stream stream)
        {
            _session = await EditingSessionFactory.CreateEditingSessionAsync(stream);

            try
            {

                foreach (Image img in images) 
                {
                    _session.UndoAll();
                    _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(Canvas.GetLeft(img), Canvas.GetTop(img), 150, 150)));
                    await _session.RenderToImageAsync(img, OutputOption.PreserveAspectRatio);
                }

                progressbarIndeterminateDownload.Visibility = System.Windows.Visibility.Collapsed;
                progressbarDescription.Visibility = System.Windows.Visibility.Collapsed;
                playButton.Visibility = System.Windows.Visibility.Visible;

            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception:" + exception.Message);
                return;
            }

        }

        private void PickImageCallback(object sender, PhotoResult e)
        {
            if (e.TaskResult != TaskResult.OK)
            {
                return;
            }
            SplitImage(e.ChosenPhoto);
        }

        private void InitializeSquares()
        {
            for (int i = 0; i < 12; i++)
            {
                Image img = new Image();
                img.Width = 150;
                img.Height = 150;
                Canvas.SetLeft(img, (i%3)*150); // columns
                Canvas.SetTop(img, (i/3)*150); // rows
                MainGrid.Children.Add(img);
                images.Add(img);
                if (i != 11) squares.Add(new Square(img, i + 1));
                else squares.Add(null);                    
            }
            images.Last().Visibility = System.Windows.Visibility.Collapsed;
        }

        private void MainGrid_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (e.ManipulationContainer.GetType() != typeof (Canvas)) 
            {
                int idx = ((int)Canvas.GetLeft(e.ManipulationContainer)) / 150 + ((int)Canvas.GetTop(e.ManipulationContainer)) / 150 * 3;
                
                puzzleBoard.MoveTile(idx);
                if (puzzleBoard.IsWon())
                    images.Last().Visibility = System.Windows.Visibility.Visible;
                else
                    images.Last().Visibility = System.Windows.Visibility.Collapsed;
            }
            
        }

        private void ApplicationBarShuffle_Click(object sender, EventArgs e)
        {
            images.Last().Visibility = System.Windows.Visibility.Collapsed;
            puzzleBoard.Shuffle();
            
        }

        private void ApplicationBarNext_Click(object sender, EventArgs e)
        {
            SetImageBackgrounds();
        }

        private void ApplicationBarNew_Click(object sender, EventArgs e)
        {
            PhotoChooserTask chooser = new PhotoChooserTask();
            chooser.PixelHeight = 600;
            chooser.PixelWidth = 450;
            chooser.Completed += PickImageCallback;
            chooser.Show();
        }

        private void ApplicationBarMenuItemSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void ApplicationBarLive_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Go Live!");
        }

        private void playButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            playButton.Visibility = System.Windows.Visibility.Collapsed;
            puzzleBoard.Shuffle();
        }


    }
}