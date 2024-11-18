using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Patient.Exceptions
{
    public class InvalidCpfException : Exception
    {
        public InvalidCpfException(string message) : base(message) { }
    }
}
