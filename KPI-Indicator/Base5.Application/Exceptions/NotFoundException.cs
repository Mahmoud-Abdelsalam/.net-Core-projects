using System;

namespace Base5.Application.Exceptions
{
    public class NotFoundException : Exception
    {

        public NotFoundException(string Msg)
            : base($"{Msg}")
        {
        }
    }
}