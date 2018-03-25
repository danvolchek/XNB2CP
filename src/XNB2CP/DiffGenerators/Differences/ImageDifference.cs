using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.DiffGenerators.Differences
{
    internal class ImageDifference : Difference
    {
        public bool IsLoad { get; set; } = false;
        public IEnumerable<Area> Differences { get; set; }
        
        public ImageDifference(IEnumerable<Area> differences)
        {
            Differences = differences;
        }

        public ImageDifference(bool isLoad)
        {
            IsLoad = isLoad;
            Differences = null;
        }
    }
}
