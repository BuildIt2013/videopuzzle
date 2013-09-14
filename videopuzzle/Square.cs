using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace videopuzzle
{
    class Square
    {
        private const int TILEDIMENSION = 150;
        public Image block;
        public int number;

        public Square(Image bl, int nmbr)
        {
            block = bl;
            number = nmbr;
        }
        // takes an image and updates the block properties
        public void Update()
        {
            return;
        }

        // This function takes the desired position in the range of 3x4 and
        // converts the coordinates to be used in xaml. The textblock properties
        // are updated according to the calculated values.
        public void SetPosition(int x, int y)
        {
            Canvas.SetTop(block, y * TILEDIMENSION);
            Canvas.SetLeft(block, x * TILEDIMENSION);
        }
     
    }
}
