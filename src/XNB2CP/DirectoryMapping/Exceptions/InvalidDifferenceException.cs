using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.DirectoryMapping.Exceptions
{
    class InvalidDifferenceException : Exception
    {
        public InvalidDifferenceException()
        {
        }

        public InvalidDifferenceException(string message)
        : base(message)
        {
        }

        public InvalidDifferenceException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
