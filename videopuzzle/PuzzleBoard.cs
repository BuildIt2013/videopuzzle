﻿using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



namespace videopuzzle
{

    class PuzzleBoard
    {
        const int ARRAYWIDTH = 3;
        const int ARRAYHEIGHT = 4;
        public List<Square> squares;

        public PuzzleBoard(List<Square> sq)
        {
            squares = sq;   
        }


        // Move tile to a desired position, two overloaded versions
        public void MoveTile(int x, int y)
        {
            MoveTile(CoordinateToIndex(x, y), true);
        }
        public void MoveTile(int position, bool animate, bool ignoreSquareMovement=false)
        {
            int emptyIndex = GetEmptyIndex();
            List<int> emptyCoordinates = IndexToCoordinate(emptyIndex);
            List<int> tileCoordinates = IndexToCoordinate(position);

            // if there is a difference of 1 in only the other coordinate, swap tiles
            if((Math.Abs(emptyCoordinates[0]-tileCoordinates[0])==1 && Math.Abs(emptyCoordinates[1]-tileCoordinates[1])==0)
                ||(Math.Abs(emptyCoordinates[0] - tileCoordinates[0]) == 0 && Math.Abs(emptyCoordinates[1] - tileCoordinates[1]) == 1))
            {
                squares[emptyIndex] = squares[position];
                squares[position] = null;
                if (!ignoreSquareMovement)
                {
                    if (animate)
                        squares[emptyIndex].AnimateToPosition(emptyCoordinates[0], emptyCoordinates[1]);
                    else
                        squares[emptyIndex].SetPosition(emptyCoordinates[0], emptyCoordinates[1]);
                }
            }            
        }

        // Update tile positions according to the list, with animation
        private void UpdateTilePositionsAnimate()
        {
            for (int i = 0; i < squares.Count; i++)
            {
                if (squares[i] != null)
                {
                    List<int> position = IndexToCoordinate(i);
                    squares[i].AnimateToPosition(position[0], position[1], 400);
                }
            }
        }


        // get the index of the empty tile
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
            if (Utils.IsChallengeMode())
            {
                if (x > ARRAYWIDTH - 1 || y > ARRAYHEIGHT - 1)
                    throw new VideoPuzzleException("Coordinates out of bound.");
                return x % ARRAYWIDTH + y * ARRAYWIDTH;
            }
            else
            {
                if (x > ARRAYWIDTH * 2 - 1 || y > ARRAYHEIGHT * 2 - 1)
                    throw new VideoPuzzleException("Coordinates out of bound.");
                return x % (ARRAYWIDTH * 2) + y * (ARRAYWIDTH * 2);
            }
        }

        // Convert array index to coordinates presented as List<int>
        public List<int> IndexToCoordinate(int idx)
        {
            if (Utils.IsChallengeMode())
                return new List<int> { idx % ARRAYWIDTH, idx / ARRAYWIDTH };
            else
                return new List<int> { idx % (ARRAYWIDTH * 2), idx / (ARRAYWIDTH * 2) };
        }

        // shuffle tiles
        public void Shuffle()
        {
            Random rand = new Random();
            if (Utils.IsChallengeMode())
            {
                for (int i = 0; i < 1000; i++)
                {
                    int randTile = (int)(rand.NextDouble() * ARRAYHEIGHT * ARRAYWIDTH);
                    MoveTile(randTile, false, true);
                }
                UpdateTilePositionsAnimate();
            }
            else
            {
                int randTile = 43;
                for (int i = 0; i < 10000; i++)
                {
                    int temp = randTile;
                    int direction = (int)(rand.NextDouble() * 4);
                    if (direction == 0)
                        temp = temp - 1;
                    else if (direction == 1)
                        temp = temp + 1;
                    else if (direction == 2)
                        temp = temp - 6;
                    else
                        temp = temp + 6;
                    if (temp >= 0 && temp < 48)
                        randTile = temp;
                    MoveTile(randTile, false, true);
                }
                UpdateTilePositionsAnimate();
            }
        }

        // is game won
        public bool IsWon()
        {
            for (int i = 0; i < squares.Count - 1; i++) {
                if (squares[i] == null || squares[i].number != i + 1) 
                {
                    return false;
                }
            }
            return true;
        }
    }
}
