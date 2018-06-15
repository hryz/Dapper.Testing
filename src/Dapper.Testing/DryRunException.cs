using System;

namespace Lib
{
    public class DryRunException : Exception
    {
        public DryRunException(){ }

        public DryRunException(string message) : base(message) { }
    }
}
