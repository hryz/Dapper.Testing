using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib
{
    public class DapperDataSourceAttribute : Attribute, ITestDataSource
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public DapperDataSourceAttribute(params Type[] scanAssembliesOfTypes)
        {
            _assemblies = scanAssembliesOfTypes.Select(x => x.Assembly);
        }

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var types = _assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(DapperQueryAttribute)))
                .ToList();

            //get the query and parameters
            foreach (var type in types)
            {
                //there might be many queries per class. One attribute per query
                var queries = GetCustomAttributes(type, typeof(DapperQueryAttribute)).Cast<DapperQueryAttribute>();
                foreach (var query in queries)
                {
                    var queryText = GetFieldOrProperty<string>(type, null, query.QueryField);

                    var parameters = !string.IsNullOrEmpty(query.ParamsField)
                        ? GetFieldOrProperty<object>(type, null, query.ParamsField)
                        : null;

                    var complements = query.QueryComplements
                        .Select(x => GetFieldOrProperty<string>(type, null, x))
                        .Cast<object>().ToArray();

                    //merge the main query with its complements
                    queryText = string.Format(queryText, complements);
                    var metadata = new QueryMetadata(type.Name, query.QueryField, query.QueryComplements);

                    yield return new object[]
                    {
                        new QueryContext(queryText, parameters, metadata)
                    };
                }
            }
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            // ReSharper disable once UseNegatedPatternMatching
            var context = data?.FirstOrDefault() as QueryContext;
            if (context == null)
                return null;

            // ReSharper disable once UseStringInterpolation
            return string.Format("{0} : {1} ({2})", 
                context.Metadata.TypeName, 
                context.Metadata.QueryName,
                string.Join(", ", context.Metadata.Complements));

        }

        private static T GetFieldOrProperty<T>(Type type, object obj, string fieldName) where T : class
        {
            const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic;

            //field
            var queryField = type.GetField(fieldName, flags);
            if (queryField != null)
                return queryField.GetValue(obj) as T;

            //property
            var queryProperty = type.GetProperty(fieldName, flags);
            if (queryProperty != null)
                return queryProperty.GetValue(obj) as T;

            throw new DapperMetadataException($"Query attribute is incorrect. There is no {fieldName} in {type.Name}");
        }
    }
}

