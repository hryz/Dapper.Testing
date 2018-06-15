using System;

namespace Dapper.Testing
{
    public class DryRunException : Exception
    {
        public DryRunException(){ }

        public DryRunException(string message) : base(message) { }
    }
}
