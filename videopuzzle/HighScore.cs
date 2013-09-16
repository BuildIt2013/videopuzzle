using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace videopuzzle
{
    class HighScore
    {
        public int Id { get; set; }
        public int Time { get; set; }
        public int PuzzleId { get; set; }
        public int PlayerId { get; set; }
    }

}
