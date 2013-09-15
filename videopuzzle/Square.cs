using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;

namespace videopuzzle
{
    class Square
    {
        private int TILEDIMENSION = 150;
        public Image block;
        public int number;

        public Square(Image bl, int nmbr, int tileDimension)
        {
            block = bl;
            number = nmbr;
            TILEDIMENSION = tileDimension;
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

        public void AnimateToPosition(int x, int y)
        {
            Duration duration = new Duration(TimeSpan.FromMilliseconds(100));

            // Create two DoubleAnimations and set their properties.
            DoubleAnimation myDoubleAnimation1 = new DoubleAnimation();
            DoubleAnimation myDoubleAnimation2 = new DoubleAnimation();

            myDoubleAnimation1.Duration = duration;
            myDoubleAnimation2.Duration = duration;

            Storyboard sb = new Storyboard();
            sb.Duration = duration;

            sb.Children.Add(myDoubleAnimation1);
            sb.Children.Add(myDoubleAnimation2);

            Storyboard.SetTarget(myDoubleAnimation1, block);
            Storyboard.SetTarget(myDoubleAnimation2, block);

            // Set the attached properties of Canvas.Left and Canvas.Top
            // to be the target properties of the two respective DoubleAnimations.
            Storyboard.SetTargetProperty(myDoubleAnimation1, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTargetProperty(myDoubleAnimation2, new PropertyPath("(Canvas.Top)"));

            myDoubleAnimation1.To = x * TILEDIMENSION;
            myDoubleAnimation2.To = y * TILEDIMENSION;

            // Begin the animation.
            sb.Begin();
        }
    }
}
