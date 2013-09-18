using CustomTile;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace videopuzzle
{
    /// <summary>
    /// This is a utility class for various functions
    /// </summary>
    class Utils
    {
        // return challenge mode value
        public static bool IsChallengeMode() { return (!IsolatedStorageSettings.ApplicationSettings.Contains("challengeMode") || !(bool)IsolatedStorageSettings.ApplicationSettings["challengeMode"]); }

        // This function updates the application live tile
       /* public static void UpdateLiveTile(List<Image> images)
        {
            
            // Create custom live tile image
            var customTile = new CustomTileControl();
            customTile.Measure(new Size(691, 336));
            customTile.Arrange(new Rect(0, 0, 691, 336));

            var bmp = new WriteableBitmap(691, 336);
            bmp.Render(customTile, null);
            bmp.Invalidate();

            const string filename = "/Shared/ShellContent/CustomTile.jpg";
            const string fname = "CustomTile.jpg";
            /*
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isf.DirectoryExists("/CustomLiveTiles"))
                {
                    isf.CreateDirectory("/CustomLiveTiles");
                }
                using (var stream = isf.OpenFile(filename, System.IO.FileMode.OpenOrCreate))
                {
                    bmp.SaveJpeg(stream, 691, 336, 0, 100);
                }
            }*/
            /*RenderText("Testi", 691, 336, 40, fname);

            // Update live tile
            CycleTileData tileData = new CycleTileData();
            tileData.Title = "Sliding Puzzle";
            tileData.Count = 0;
            ShellTile tile = ShellTile.ActiveTiles.First();
            tileData.SmallBackgroundImage = new Uri("Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative);
            tileData.CycleImages = new List<Uri>
            {
                new Uri("isostore:" + filename, UriKind.Absolute),
                new Uri("Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative)    
            };
            var clearTileData = new CycleTileData("<?xml version=\"1.0\" encoding=\"utf-8\"?><wp:Notification xmlns:wp=\"WPNotification\" Version=\"2.0\"> <wp:Tile Id=\"TileID\" Template=\"CycleTile\"> <wp:SmallBackgroundImage Action=\"Clear\" /> <wp:CycleImage1 Action=\"Clear\" /> <wp:CycleImage2 Action=\"Clear\" /> <wp:CycleImage3 Action=\"Clear\" /> <wp:CycleImage4 Action=\"Clear\" /> <wp:CycleImage5 Action=\"Clear\" /> <wp:CycleImage6 Action=\"Clear\" /> <wp:CycleImage7 Action=\"Clear\" /> <wp:CycleImage8 Action=\"Clear\" /> <wp:CycleImage9 Action=\"Clear\" /> <wp:Count Action=\"Clear\" /> <wp:Title Action=\"Clear\" /> </wp:Tile></wp:Notification>");
            tile.Update(clearTileData);
            tile.Update(tileData);
        }*/

        public static void UpdateLiveTile(List<Image> images)
        {
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault();
            if (tile != null)
            {
                CycleTileData flipTile = new CycleTileData();
                flipTile.Title = "Title Text";
                //flipTile.BackTitle = "Back Title Text";

                //flipTile.BackContent = " ";
                //flipTile.WideBackContent = " ";
                string info = "Testitestitestitestitestitestitesti";
                //Medium size Tile 336x336 px
                //Crete image for BackBackgroundImage in IsoStore
                if (info.Length >= 135)
                {
                    RenderText(info.Substring(0, 135) + "...", 336, 336, 30, "BackBackgroundImage");
                }
                else
                {
                    RenderText(info, 336, 336, 28, "BackBackgroundImage");
                }

                //flipTile.BackBackgroundImage = new Uri(@"isostore:/Shared/ShellContent/BackBackgroundImage.jpg", UriKind.Absolute); //Generated image for Back Background 336x336
                //flipTile.BackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative); //Default image for Background Image Medium Tile 336x336 px
                //End Medium size Tile 336x336 px

                //Wide size Tile 691x336 px
                //flipTile.WideBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative); ////Default image for Background Image Wide Tile 691x336 px

                //Create image for WideBackBackgroundImage in IsoStore
                RenderText(info, 691, 336, 40, "WideBackBackgroundImage");
                //flipTile.WideBackBackgroundImage = new Uri(@"isostore:/Shared/ShellContent/WideBackBackgroundImage.jpg", UriKind.Absolute);
                //End Wide size Tile 691x336 px
                flipTile.CycleImages = new List<Uri>
            {
                new Uri(@"isostore:/Shared/ShellContent/WideBackBackgroundImage.jpg", UriKind.Absolute) 
            };
                //Update Live Tile
                tile.Update(flipTile);
            }
        }

        private static void RenderText(string text, int width, int height, int fontsize, string imagename)
        {
            WriteableBitmap b = new WriteableBitmap(width, height);

            var canvas = new Grid();
            canvas.Width = b.PixelWidth;
            canvas.Height = b.PixelHeight;

            var background = new Canvas();
            background.Height = b.PixelHeight;
            background.Width = b.PixelWidth;

            //Created background color as Accent color
            SolidColorBrush backColor = new SolidColorBrush((Color)Application.Current.Resources["PhoneAccentColor"]);
            background.Background = backColor;

            var textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.TextAlignment = TextAlignment.Left;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Stretch;
            textBlock.Margin = new Thickness(35);
            textBlock.Width = b.PixelWidth - textBlock.Margin.Left * 2;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Foreground = new SolidColorBrush(Colors.White); //color of the text on the Tile
            textBlock.FontSize = fontsize;

            canvas.Children.Add(textBlock);

            b.Render(background, null);
            b.Render(canvas, null);
            b.Invalidate(); //Draw bitmap

            //Save bitmap as jpeg file in Isolated Storage
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream imageStream = new IsolatedStorageFileStream("/Shared/ShellContent/" + imagename + ".jpg", System.IO.FileMode.Create, isf))
                {
                    b.SaveJpeg(imageStream, b.PixelWidth, b.PixelHeight, 0, 100);
                }
            }
        }
    }    
}
