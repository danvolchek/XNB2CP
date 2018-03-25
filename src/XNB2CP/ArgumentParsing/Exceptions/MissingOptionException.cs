using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.ArgumentParsing.Exceptions
{
    internal class MissingOptionException : Exception
    {
        public MissingOptionException()
        {
        }

        public MissingOptionException(string message)
        : base(message)
        {
        }

        public MissingOptionException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
