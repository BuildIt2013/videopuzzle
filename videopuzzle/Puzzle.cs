using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace videopuzzle
{
    class Puzzle
    {
        public int Id { get; set; }
        public string ImageUri { get; set; }
        public int[] InitialPosition { get; set; }
        public string ImageCredits { get; set; }
    }
}
