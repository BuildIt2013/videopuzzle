using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace videopuzzle
{

    class PuzzleBoard
    {
        const int ARRAYWIDTH = 3;
        const int ARRAYHEIGHT = 3;
        public List<Square> squares;

        PuzzleBoard(List<Square> sq)
        {
            squares = sq;   
        }

        public void MoveTile(int tileNumber)
        {
            int emptyIndex = GetEmptyIndex();
            List<int> emptyCoordinates = IndexToCoordinate(emptyIndex);
            List<int> tileCoordinates = IndexToCoordinate(tileNumber);
            if((Math.Abs(emptyCoordinates[0]-tileCoordinates[0])==1 && Math.Abs(emptyCoordinates[1]-tileCoordinates[1])==0)
                ||(Math.Abs(emptyCoordinates[0] - tileCoordinates[0]) == 0 && Math.Abs(emptyCoordinates[1] - tileCoordinates[1]) == 1))
            {
                // if there is a difference of 1 in only the other coordinate, swap tiles
                squares[emptyIndex] = squares[tileNumber];
                squares[tileNumber] = null;
            }
           
        }

        private int GetEmptyIndex()
        {
            for (int i = 0; i < squares.Count; i++)
            {
                if (squares[i] == null)
                    return i;
            }
            return -1;
        }

        public int CoordinateToIndex(int x, int y)
        {
            if (x > ARRAYWIDTH - 1 || y > ARRAYHEIGHT - 1)
                throw new VideoPuzzleException("Coordinates out of bound.");
            return x % ARRAYWIDTH + y * ARRAYWIDTH;
        }

        public List<int> IndexToCoordinate(int idx)
        {
            return new List<int> { idx % ARRAYWIDTH, idx / ARRAYWIDTH };
        }

        // shuffle tiles
        public void Shuffle()
        {
            //TODO
            return;
        }
    }
}
