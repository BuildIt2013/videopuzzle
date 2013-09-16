using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static void UpdateLiveTile()
        {
            CycleTileData tileData = new CycleTileData();
            tileData.Title = "Sliding Puzzle";
            tileData.Count = 0;
            ShellTile tile = ShellTile.ActiveTiles.First();
            tileData.SmallBackgroundImage = new Uri("Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative);

            // Images could be max Nine images.
            tileData.CycleImages = new List<Uri>
               {
                    //TODO: Functionality to change the picture to the last played picture
                  new Uri(@"\Assets\Tiles\001.jpg", UriKind.Relative), 
                  new Uri(@"\Assets\Tiles\002.jpg", UriKind.Relative), 
                  
               };

            var clearTileData = new CycleTileData("<?xml version=\"1.0\" encoding=\"utf-8\"?><wp:Notification xmlns:wp=\"WPNotification\" Version=\"2.0\"> <wp:Tile Id=\"TileID\" Template=\"CycleTile\"> <wp:SmallBackgroundImage Action=\"Clear\" /> <wp:CycleImage1 Action=\"Clear\" /> <wp:CycleImage2 Action=\"Clear\" /> <wp:CycleImage3 Action=\"Clear\" /> <wp:CycleImage4 Action=\"Clear\" /> <wp:CycleImage5 Action=\"Clear\" /> <wp:CycleImage6 Action=\"Clear\" /> <wp:CycleImage7 Action=\"Clear\" /> <wp:CycleImage8 Action=\"Clear\" /> <wp:CycleImage9 Action=\"Clear\" /> <wp:Count Action=\"Clear\" /> <wp:Title Action=\"Clear\" /> </wp:Tile></wp:Notification>");
            tile.Update(clearTileData);
            tile.Update(tileData);
        }
    }

    
}
