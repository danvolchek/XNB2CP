using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.DiffGenerators.Differences
{
    internal class Area
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; } = 1;
        public int Height { get; set; } = 1;

        public Area(int x, int y)
        {
            Left = x;
            Top = y;
        }
    }
}
