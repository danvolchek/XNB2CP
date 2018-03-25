using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.DirectoryMapping.Exceptions
{
    class InvalidMappingException : Exception
    {
        public InvalidMappingException()
        {
        }

        public InvalidMappingException(string message)
        : base(message)
        {
        }

        public InvalidMappingException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
