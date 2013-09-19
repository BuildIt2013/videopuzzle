using CustomTile;
using Microsoft.Phone.Shell;
using Nokia.Graphics.Imaging;
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

        public static string GetTimeString(int secs)
        {
            int min = secs / 60;
            int sec = secs % 60;
            string minutes = (min < 10) ? "0" + min.ToString() : min.ToString();
            string seconds = (sec < 10) ? "0" + sec.ToString() : sec.ToString();

            return minutes + ":" + seconds;
        }

        // This function updates the application live tile
        public static void UpdateLiveTile(Canvas canv)
         {
            var bmp = new WriteableBitmap(450, 600);
            bmp.Render(canv, null);
            bmp.Invalidate();

            CycleTileData tileData = new CycleTileData();
            tileData.Title = "Sliding Puzzle";
            ShellTile tile = ShellTile.ActiveTiles.First();
            tileData.SmallBackgroundImage = new Uri("Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative);

            // shifting value for filename rotation
            int shift = 0;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("imageShift"))
            {
                shift = (int)IsolatedStorageSettings.ApplicationSettings["imageShift"];
            }
                        
            if (shift == 7)
                IsolatedStorageSettings.ApplicationSettings["imageShift"] = 0;
            else
                IsolatedStorageSettings.ApplicationSettings["imageShift"] = shift + 1;

            string filename = "/Shared/ShellContent/";
            filename = filename + shift + ".jpg";
             
            List<Uri> cycleImages = new List<Uri>();
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isf.DirectoryExists("/CustomLiveTiles"))
                {
                    isf.CreateDirectory("/CustomLiveTiles");
                }
                using (var stream = isf.OpenFile(filename, System.IO.FileMode.OpenOrCreate))
                {
                    bmp.SaveJpeg(stream, 450, 600, 0, 100);
                }
                for (int i = 0; i < 8; i++)
                {
                    string fname = "Shared/ShellContent/" + (i + shift) % 8 + ".jpg";
                    if (isf.FileExists(fname))
                        cycleImages.Add(new Uri("isostore:" + fname, UriKind.Absolute));
                }
            }
            // Clear and update live tile
            tileData.CycleImages = cycleImages;
            var clearTileData = new CycleTileData("<?xml version=\"1.0\" encoding=\"utf-8\"?><wp:Notification xmlns:wp=\"WPNotification\" Version=\"2.0\"> <wp:Tile Id=\"TileID\" Template=\"CycleTile\"> <wp:SmallBackgroundImage Action=\"Clear\" /> <wp:CycleImage1 Action=\"Clear\" /> <wp:CycleImage2 Action=\"Clear\" /> <wp:CycleImage3 Action=\"Clear\" /> <wp:CycleImage4 Action=\"Clear\" /> <wp:CycleImage5 Action=\"Clear\" /> <wp:CycleImage6 Action=\"Clear\" /> <wp:CycleImage7 Action=\"Clear\" /> <wp:CycleImage8 Action=\"Clear\" /> <wp:CycleImage9 Action=\"Clear\" /> <wp:Count Action=\"Clear\" /> <wp:Title Action=\"Clear\" /> </wp:Tile></wp:Notification>");
            tile.Update(clearTileData);
            tile.Update(tileData);
        }
    }
}
