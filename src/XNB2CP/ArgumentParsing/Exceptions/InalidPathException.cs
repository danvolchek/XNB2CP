using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.ArgumentParsing.Exceptions
{
    internal class InvalidPathException : Exception
    {
        public InvalidPathException()
        {
        }

        public InvalidPathException(string message)
        : base(message)
        {
        }

        public InvalidPathException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
