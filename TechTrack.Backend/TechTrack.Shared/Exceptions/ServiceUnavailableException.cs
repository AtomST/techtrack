using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Exceptions
{
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException(string? message) : base(message) { }
    }
}
