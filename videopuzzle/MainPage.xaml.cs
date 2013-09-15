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
using System.Windows.Threading;
using Windows.Phone.Media.Capture;
using Microsoft.Devices;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows.Resources; 



namespace videopuzzle
{
    public partial class MainPage : PhoneApplicationPage
    {
        private PuzzleBoard puzzleBoard;
        private List<Square> squares;
        private EditingSession _session;
        private List<Image> images;
        private DispatcherTimer timer;
        private int playTime;
        private bool isGameStarted;
        private bool isLoading;
        Random rand;
        private PlayMode playMode;

        private AudioVideoCaptureDevice camera;
        private WriteableBitmap frameBitmap;

        private const double MediaElementWidth = 640;
        private const double MediaElementHeight = 480;


        // Constructor
        public MainPage()
        {
            InitializeComponent();
            playMode = PlayMode.OnlineImage;
            playTime = 0;
            UpdateTime();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Stop();
            rand = new Random();
            isGameStarted = false;
            squares = new List<Square>();
            images = new List<Image>();
            InitializeSquares();
            puzzleBoard = new PuzzleBoard(squares);
            SetImageBackgrounds();
        }

        // ***************** THESE FUNCTIONS ARE FOR PLAYTIME TIMER *******************
        private void timer_Tick(object sender, EventArgs e)
        {
            playTime++;
            UpdateTime();
        }

        private void UpdateTime()
        {
            PlayTimer.Text = getTimeString(playTime);
        }

        private void ResetTime()
        {
            playTime = 0;
        }
        //********************************************************************************

        private void SetImageBackgrounds() {
            bool offline = false;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("offlineMode"))
            {
                offline = (bool)IsolatedStorageSettings.ApplicationSettings["offlineMode"];
            }
            SetImageBackgrounds(offline);
        }

        private void SetImageBackgrounds(bool offline)
        {

            if (offline)
            {
                int index = (int)(rand.NextDouble() * 6);
                string imgPath = "/Assets/Images/Offline/";
                switch (index) 
                {
                    case 0: 
                        imgPath += "0001.jpg";
                        break;
                    case 1:
                        imgPath += "0002.jpg";
                        break;
                    case 2:
                        imgPath += "0003.jpg";
                        break;
                    case 3:
                        imgPath += "0004.jpg";
                        break;
                    case 4:
                        imgPath += "0005.jpg";
                        break;
                    case 5:
                        imgPath += "0006.jpg";
                        break;
                    default:
                        imgPath += "0001.jpg";
                        break;
                }

                BitmapImage bmp = new BitmapImage(new Uri(imgPath, UriKind.Relative));
                bmp.CreateOptions = BitmapCreateOptions.None;
                bmp.ImageOpened += bmp_ImageOpened;

                
            } 
            else if (!isLoading)
            {
                WebClient client = new WebClient();
                client.OpenReadCompleted += client_OpenReadCompleted;
                client.OpenReadAsync(new Uri("http://lorempixel.com/450/600/?v=" + Guid.NewGuid(), UriKind.Absolute));
                isLoading = true;
                progressbarIndeterminateDownload.Visibility = System.Windows.Visibility.Visible;
                progressbarDescription.Visibility = System.Windows.Visibility.Visible;
            }
        }

        void bmp_ImageOpened(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wbmp = new WriteableBitmap((BitmapImage) sender);
            SplitImageFromBitmap(wbmp);

        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            
            isLoading = false;
            progressbarIndeterminateDownload.Visibility = System.Windows.Visibility.Collapsed;
            progressbarDescription.Visibility = System.Windows.Visibility.Collapsed;

            if (e.Error == null)
            {
                SplitImage(e.Result);
            }
            else {
                SetImageBackgrounds(true);
            }
        }

        private async void SplitImage(Stream stream)
        {
            int dimension;
            _session = await EditingSessionFactory.CreateEditingSessionAsync(stream);
            IFilter selectedFilter = GetFilter();
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("challengeMode") || !(bool)IsolatedStorageSettings.ApplicationSettings["challengeMode"])
            {
                dimension = 150;
            }
            else
            {
                dimension = 75;
            }
            try
            {
                stream.Position = 0;

                foreach (Image img in images)
                {
                    _session.UndoAll();
                    _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(Canvas.GetLeft(img), Canvas.GetTop(img), dimension, dimension)));
                    if (selectedFilter != null) _session.AddFilter(selectedFilter);                   
                    await _session.RenderToImageAsync(img, OutputOption.PreserveAspectRatio);
                }

                progressbarIndeterminateDownload.Visibility = System.Windows.Visibility.Collapsed;
                progressbarDescription.Visibility = System.Windows.Visibility.Collapsed;
                isLoading = false;
                playButton.Visibility = System.Windows.Visibility.Visible;

            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception:" + exception.Message);
                return;
            }
        }


        private async void SplitImageFromBitmap(WriteableBitmap bmp)
        {
            _session = new EditingSession(bmp.AsBitmap());
            if (playMode == PlayMode.CameraVideo) _session.AddFilter(FilterFactory.CreateStepRotationFilter(Rotation.Rotate90));
            if (playMode == PlayMode.CameraVideo) _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(15, 20, 450, 600)));
            IFilter selectedFilter = GetFilter();
            try
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("challengeMode") || !(bool)IsolatedStorageSettings.ApplicationSettings["challengeMode"])
                {
                    foreach (Image img in images)
                    {
                        _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(images.IndexOf(img) % 3 * 150, images.IndexOf(img) / 3 * 150, 150, 150)));
                        if (selectedFilter != null ) _session.AddFilter(selectedFilter);
                        await _session.RenderToImageAsync(img, OutputOption.PreserveAspectRatio);
                        if (selectedFilter != null && _session.CanUndo()) _session.Undo();
                        if (_session.CanUndo()) _session.Undo();
                    }
                }
                else
                {
                    foreach (Image img in images)
                    {
                        _session.AddFilter(FilterFactory.CreateCropFilter(new Windows.Foundation.Rect(images.IndexOf(img) % 6 * 75, images.IndexOf(img) / 6 * 75, 75, 75)));
                        if (selectedFilter != null) _session.AddFilter(selectedFilter);
                        await _session.RenderToImageAsync(img, OutputOption.PreserveAspectRatio);
                        if (selectedFilter != null && _session.CanUndo()) _session.Undo();
                        if (_session.CanUndo()) _session.Undo();
                    }
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception:" + exception.Message);
                return;
            }
            if (playMode == PlayMode.CameraVideo) { processNextFrame(); }

        }

        // In this function the user picks the desired image
        private void PickImageCallback(object sender, PhotoResult e)
        {
            if (e.TaskResult != TaskResult.OK)
            {
                return;
            }
            ResetPuzzle();
            SplitImage(e.ChosenPhoto);
        }

        private void InitializeSquares()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("challengeMode") || !(bool)IsolatedStorageSettings.ApplicationSettings["challengeMode"])
            {
                for (int i = 0; i < 12; i++)
                {
                    Image img = new Image();
                    img.Width = 150;
                    img.Height = 150;
                    Canvas.SetLeft(img, (i % 3) * 150); // columns
                    Canvas.SetTop(img, (i / 3) * 150); // rows
                    MainGrid.Children.Add(img);
                    images.Add(img);
                    if (i != 11) squares.Add(new Square(img, i + 1, 150));
                    else squares.Add(null);
                }
            }
            else
            {
                for (int i = 0; i < 48; i++)
                {
                    Image img = new Image();
                    img.Width = 75;
                    img.Height = 75;
                    Canvas.SetLeft(img, (i % 6) * 75); // columns
                    Canvas.SetTop(img, (i / 6) * 75); // rows
                    MainGrid.Children.Add(img);
                    images.Add(img);
                    if (i != 47) squares.Add(new Square(img, i + 1, 75));
                    else squares.Add(null);
                }
            }
            timer.Stop();
            isGameStarted = false;

        }

        private void MainGrid_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (e.ManipulationContainer.GetType() != typeof(Canvas) && isGameStarted)
            {
                int idx = 0;
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("challengeMode") || !(bool)IsolatedStorageSettings.ApplicationSettings["challengeMode"])
                {
                    idx = ((int)Canvas.GetLeft(e.ManipulationContainer)) / 150 + ((int)Canvas.GetTop(e.ManipulationContainer)) / 150 * 3;
                }
                else
                {
                    idx = ((int)Canvas.GetLeft(e.ManipulationContainer)) / 75 + ((int)Canvas.GetTop(e.ManipulationContainer)) / 75 * 6;
                }
                puzzleBoard.MoveTile(idx, true);
                if (puzzleBoard.IsWon())
                {
                    timer.Stop();
                    images.Last().Visibility = System.Windows.Visibility.Visible;
                }
                else
                    images.Last().Visibility = System.Windows.Visibility.Collapsed;
            }

        }

        // ********* HERE ARE THE APPLICATIONBAR BUTTON CALLBACK FUNCTIONS *************
        private void ApplicationBarShuffle_Click(object sender, EventArgs e)
        {
            if (isGameStarted)
            {
                images.Last().Visibility = System.Windows.Visibility.Collapsed;
                puzzleBoard.Shuffle();
                ResetTime();
                UpdateTime();
            }
        }

        private void ApplicationBarNext_Click(object sender, EventArgs e)
        {
            playMode = PlayMode.OnlineImage;
            CameraOff();
            ResetPuzzle();
            ResetTime();
            SetImageBackgrounds();           
        }

        private void ApplicationBarNew_Click(object sender, EventArgs e)
        {
            playMode = PlayMode.GalleryImage;
            CameraOff();
            PhotoChooserTask chooser = new PhotoChooserTask();
            chooser.PixelHeight = 600;
            chooser.PixelWidth = 450;
            chooser.Completed += PickImageCallback;
            chooser.Show();
        }

        private void ApplicationBarMenuItemSettings_Click(object sender, EventArgs e)
        {
            CameraOff();
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItemAbout_Click(object sender, EventArgs e)
        {
            string contributors = @"Contributors:
Tuomo Vierros
Janne Kemppainen
Vidhuran Harichandra Babu
Petri Martinmäki
Siddhart Rao";
            string imageRef = @"Images from
http://lorempixel.com/";

            string prgCreated = @"Aaltoes Build It 2013";
            MessageBox.Show(contributors + "\n\n" + imageRef + "\n\n" + prgCreated);
        }
        // **************************************************************************
        private void ResetPuzzle()
        {
            foreach (Image im in images)
            {
                MainGrid.Children.Remove(im);
            }
            images = new List<Image>();
            squares = new List<Square>();
            puzzleBoard = new PuzzleBoard(squares);
            playTime = 0;
            UpdateTime();
            InitializeSquares();
        }

        private async void ApplicationBarLive_Click(object sender, EventArgs e)
        {
            playMode = PlayMode.CameraVideo;
            await initCamera(CameraSensorLocation.Back);
            processNextFrame();

        }

        private void playButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            playButton.Visibility = System.Windows.Visibility.Collapsed;
            images.Last().Visibility = System.Windows.Visibility.Collapsed;
            timer.Start();
            puzzleBoard.Shuffle();
            isGameStarted = true;
        }


        private static string getTimeString(int secs)
        {
            int min = secs / 60;
            int sec = secs % 60;
            string minutes = (min < 10) ? "0" + min.ToString() : min.ToString();
            string seconds = (sec < 10) ? "0" + sec.ToString() : sec.ToString();

            return minutes + ":" + seconds;
        }

        private async Task initCamera(CameraSensorLocation sensorLocation)
        {
            Windows.Foundation.Size res = new Windows.Foundation.Size(MediaElementWidth, MediaElementHeight);
            CameraOff();
            camera = await AudioVideoCaptureDevice.OpenForVideoOnlyAsync(sensorLocation, res);

            await camera.SetPreviewResolutionAsync(res);

            frameBitmap = new WriteableBitmap((int)camera.PreviewResolution.Width,
                   (int)camera.PreviewResolution.Height);

        }

        private void CameraOff()
        {
            if (camera != null)
            {
                camera.Dispose();
                camera = null;
            }
        }

        private void processNextFrame()
        {
            try
            {
                camera.GetPreviewBufferArgb(frameBitmap.Pixels);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Safe Exit : Expecting null pointer exception" + e.Message);
            }
            SplitImageFromBitmap(frameBitmap);
        }

        private IFilter GetFilter()
        {
            int index = 0;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("filterIndex"))
                index = (int)IsolatedStorageSettings.ApplicationSettings["filterIndex"];
            switch (index)
            {
                case 0: return null;
                case 1: return FilterFactory.CreateCartoonFilter(true);
                case 2: return FilterFactory.CreateGrayscaleFilter();
                case 3: return FilterFactory.CreateNegativeFilter();
                case 4: return FilterFactory.CreateSepiaFilter();
            }
            return null;
        }

    }
}