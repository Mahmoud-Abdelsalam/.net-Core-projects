using System;
using System.Collections.Generic;
using System.Text;

namespace Base5.Application.Exceptions
{
  public class KeyIsAlreadyExistsException: Exception
    {
        public KeyIsAlreadyExistsException(string msg)
          : base($"{msg}")
        {

        }
    }
}
