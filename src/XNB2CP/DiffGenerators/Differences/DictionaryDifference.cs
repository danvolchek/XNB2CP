using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.DiffGenerators.Differences
{
    internal class DictionaryDifference : Difference
    {
        public IDictionary<string, string> Differences { get; set; }

        public DictionaryDifference(IDictionary<string, string> differences)
        {
            Differences = differences;
        }
    }
}
