using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace videopuzzle
{
    class VideoPuzzleException : Exception
    {
        public string description;
        public VideoPuzzleException()
        {
            description = "";
        }

        public VideoPuzzleException(string dscr)
        {
            description = dscr;
        }
    }
}
