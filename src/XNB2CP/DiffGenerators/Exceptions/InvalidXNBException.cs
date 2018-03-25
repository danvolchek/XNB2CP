using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.DiffGenerators.Exceptions
{
    internal class InvalidXNBException : Exception
    {
        public InvalidXNBException()
        {
        }

        public InvalidXNBException(string message)
        : base(message)
        {
        }

        public InvalidXNBException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
