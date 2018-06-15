using System;

namespace Lib
{
    public class DapperMetadataException : Exception
    {
        public DapperMetadataException(){ }

        public DapperMetadataException(string message) : base(message) { }
    }
}