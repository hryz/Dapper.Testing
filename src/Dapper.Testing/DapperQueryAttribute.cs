using System;

namespace Dapper.Testing
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DapperQueryAttribute : Attribute
    {
        public DapperQueryAttribute(string queryField, string paramsField = null, params string[] queryComplements)
        {
            ParamsField = paramsField;
            QueryField = queryField;
            QueryComplements = queryComplements;
        }

        public string QueryField { get; }
        public string[] QueryComplements { get; }
        public string ParamsField { get; }
    }
}
