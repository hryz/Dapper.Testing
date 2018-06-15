using System;

namespace Dapper.Testing
{
    public class DapperMetadataException : Exception
    {
        public DapperMetadataException(){ }

        public DapperMetadataException(string message) : base(message) { }
    }
}