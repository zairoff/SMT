using System;

namespace SMT.ViewModel.Exceptions
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
