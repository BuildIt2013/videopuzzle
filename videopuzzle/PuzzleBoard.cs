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

            // if there is a difference of 1 in only the other coordinate, swap tiles
            if((Math.Abs(emptyCoordinates[0]-tileCoordinates[0])==1 && Math.Abs(emptyCoordinates[1]-tileCoordinates[1])==0)
                ||(Math.Abs(emptyCoordinates[0] - tileCoordinates[0]) == 0 && Math.Abs(emptyCoordinates[1] - tileCoordinates[1]) == 1))
            {
                squares[emptyIndex] = squares[tileNumber];
                squares[tileNumber] = null;
                squares[emptyIndex].SetPosition(emptyCoordinates[0], emptyCoordinates[1]);
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

        // convert Coordinate values to array index
        public int CoordinateToIndex(int x, int y)
        {
            if (x > ARRAYWIDTH - 1 || y > ARRAYHEIGHT - 1)
                throw new VideoPuzzleException("Coordinates out of bound.");
            return x % ARRAYWIDTH + y * ARRAYWIDTH;
        }

        // Convert array index too coordinates presented as List<int>
        public List<int> IndexToCoordinate(int idx)
        {
            return new List<int> { idx % ARRAYWIDTH, idx / ARRAYWIDTH };
        }

        // shuffle tiles
        public void Shuffle()
        {
            //TODO: Implement
            return;
        }
    }
}
