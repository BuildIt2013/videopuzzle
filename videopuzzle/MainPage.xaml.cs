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
        Random rand;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            squares = new List<Square>();
            InitializeSquares(squares);
            puzzleBoard = new PuzzleBoard(squares);
            rand = new Random();
            SetImageBackgrounds();
        }


        private void SetImageBackgrounds() {

            WebClient client = new WebClient();
            client.OpenReadCompleted += client_OpenReadCompleted;
            client.OpenReadAsync(new Uri("http://lorempixel.com/450/600/?v=" + Guid.NewGuid(), UriKind.Absolute));
            
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
                // Decode the jpeg for showing the original image
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(0, 0, 150, 150)));
                await _session.RenderToImageAsync(Image01, OutputOption.PreserveAspectRatio);
                _session.UndoAll();
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(150, 0, 150, 150)));
                await _session.RenderToImageAsync(Image02, OutputOption.PreserveAspectRatio);
                _session.UndoAll();
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(300, 0, 150, 150)));
                await _session.RenderToImageAsync(Image03, OutputOption.PreserveAspectRatio);
                _session.UndoAll();
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(0, 150, 150, 150)));
                await _session.RenderToImageAsync(Image04, OutputOption.PreserveAspectRatio);
                _session.UndoAll();
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(150, 150, 150, 150)));
                await _session.RenderToImageAsync(Image05, OutputOption.PreserveAspectRatio);
                _session.UndoAll();
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(300, 150, 150, 150)));
                await _session.RenderToImageAsync(Image06, OutputOption.PreserveAspectRatio);
                _session.UndoAll();
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(0, 300, 150, 150)));
                await _session.RenderToImageAsync(Image07, OutputOption.PreserveAspectRatio);
                _session.UndoAll();
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(150, 300, 150, 150)));
                await _session.RenderToImageAsync(Image08, OutputOption.PreserveAspectRatio);
                _session.UndoAll();
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(300, 300, 150, 150)));
                await _session.RenderToImageAsync(Image09, OutputOption.PreserveAspectRatio);
                _session.UndoAll();
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(0, 450, 150, 150)));
                await _session.RenderToImageAsync(Image10, OutputOption.PreserveAspectRatio);
                _session.UndoAll();
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(150, 450, 150, 150)));
                await _session.RenderToImageAsync(Image11, OutputOption.PreserveAspectRatio);
                _session.UndoAll();
                _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(300, 450, 150, 150)));
                await _session.RenderToImageAsync(Image12, OutputOption.PreserveAspectRatio);
                Image12.Visibility = System.Windows.Visibility.Collapsed;


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

        private void InitializeSquares(List<Square> squares)
        {
            squares.Add(new Square(Image01, 1));
            squares.Add(new Square(Image02, 2));
            squares.Add(new Square(Image03, 3));
            squares.Add(new Square(Image04, 4));
            squares.Add(new Square(Image05, 5));
            squares.Add(new Square(Image06, 6));
            squares.Add(new Square(Image07, 7));
            squares.Add(new Square(Image08, 8));
            squares.Add(new Square(Image09, 9));
            squares.Add(new Square(Image10, 10));
            squares.Add(new Square(Image11, 11));
            squares.Add(null);
        }

        private void MainGrid_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (e.ManipulationContainer.GetType() != typeof (Canvas)) 
            {
                int idx = ((int)Canvas.GetLeft(e.ManipulationContainer)) / 150 + ((int)Canvas.GetTop(e.ManipulationContainer)) / 150 * 3;
                
                puzzleBoard.MoveTile(idx);
                if (puzzleBoard.IsWon())
                    Image12.Visibility = System.Windows.Visibility.Visible;
                else
                    Image12.Visibility = System.Windows.Visibility.Collapsed;
            }
            
        }

        private void ApplicationBarShuffle_Click(object sender, EventArgs e)
        {
            Image12.Visibility = System.Windows.Visibility.Collapsed;
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


    }
}