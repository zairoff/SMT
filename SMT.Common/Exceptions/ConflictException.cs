using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Common.Exceptions
{
    [Serializable]
    public class ConflictException : Exception
    {
        public ConflictException()
        {

        }

        public ConflictException(string message) : base(message)
        {

        }
    }
}
