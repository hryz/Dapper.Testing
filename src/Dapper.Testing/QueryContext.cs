namespace Lib
{
    public class QueryContext
    {
        internal QueryContext(string queryText, object parameters, QueryMetadata metadata)
        {
            QueryText = queryText;
            Parameters = parameters;
            Metadata = metadata;
        }

        public string QueryText { get; }
        public object Parameters { get; }
        public QueryMetadata Metadata { get; }
    }
}