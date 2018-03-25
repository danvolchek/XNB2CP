using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.ArgumentParsing.Exceptions
{
    internal class InvalidOptionException : Exception
    {
        public InvalidOptionException()
        {
        }

        public InvalidOptionException(string message)
        : base(message)
        {
        }

        public InvalidOptionException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}