using System;
using System.Collections.Generic;
using System.Text;

namespace Base5.Application.Exceptions
{
    public class DuplicatedIndexException : Exception
    {
        public DuplicatedIndexException(string action, string name, object index, string message= "Index Is already exists")
            : base($"{action} entity \"{name}\" ({index}) failed. {message}")
        {

        }
    }
}
