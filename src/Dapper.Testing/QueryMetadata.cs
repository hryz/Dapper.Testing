using System.Collections.Generic;

namespace Lib
{
    public class QueryMetadata
    {
        internal QueryMetadata(string typeName, string queryName, IEnumerable<object> complements)
        {
            TypeName = typeName;
            QueryName = queryName;
            Complements = complements;
        }

        public string TypeName { get; }
        public string QueryName { get; }
        public IEnumerable<object> Complements { get; }
    }
}