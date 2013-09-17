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
        public static void UpdateLiveTile(List<Image> images)
        {
            
            // Create custom live tile image
            var customTile = new CustomTileControl();
            customTile.Measure(new Size(691, 336));
            customTile.Arrange(new Rect(0, 0, 691, 336));

            var bmp = new WriteableBitmap(691, 336);
            bmp.Render(customTile, null);
            bmp.Invalidate();

            const string filename = "/Shared/ShellContent/CustomTile.jpg";

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
            }

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
        }
    }

    
}
