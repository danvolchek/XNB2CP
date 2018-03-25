using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.ArgumentParsing.Exceptions
{
    internal class MissingArgumentException : Exception
    {
        public MissingArgumentException()
        {
        }

        public MissingArgumentException(string message)
        : base(message)
        {
        }

        public MissingArgumentException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
