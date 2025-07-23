using Microsoft.VisualBasic.FileIO;
using Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Shared.Helpers
{
    /// <summary>
    /// Custom CSV reader to handle complex parsing
    /// </summary>
    /// <remarks>I obviously could've just made a data class that matches the excel csv so I can use a
    /// library to parse the data, but that's boring.</remarks>
    public static class CsvReader
    {
        /// <summary>
        /// Parses a line of a csv file.
        /// </summary>
        /// <typeparam name="T">The type to parse to.</typeparam>
        /// <param name="fields">The csv data.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>A serialized <typeparamref name="T"/></returns>
        public static T Parse<T>(string[] fields, string[] headers) where T: new() 
        {
            var result = new T();
            if (fields.Length != headers.Length)
            {
                //TODO Handle mismatch.
                return default(T);
            }

            //Check to see if the parent object we are parsing has a prefix attribute.
            var parentObjectPrefix = typeof(T).GetCustomAttribute<ObjectPrefixAttribute>();
            string prefix = "EmptyPrefix";
            if (parentObjectPrefix != null)
            {
                prefix = parentObjectPrefix.Prefix;
            }

            var nestedPrefixs = new List<string>();
            for (int i = 0; i < headers.Length; i++)
            {
                string header = headers[i].Replace(" ", string.Empty).Trim();
                object value = fields[i];

                if (nestedPrefixs.Any(x => header.StartsWith(x))) continue; //Already checked this header in a nested object.
                if (header.StartsWith(prefix))
                {
                    header = header.Substring(prefix.Length); //If the prefix is present then the property we are looking for doesn't include it.
                }

                var property = typeof(T).GetProperty(header);

                if (property != null)
                {
                    value = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(result, value);
                    continue;
                }

                // Property is null, recursively search for nested objects where it may exist.
                foreach(var prop in typeof(T).GetProperties())
                {
                    if (prop.GetCustomAttribute<NestedObjectAttribute>() == null) continue;
                    
                    var childObjectPrefix = prop.PropertyType.GetCustomAttribute<ObjectPrefixAttribute>();
                    if (childObjectPrefix != null) 
                    {
                        nestedPrefixs.Add(childObjectPrefix.Prefix);
                    }
                    
                    var parseMethod = typeof(CsvReader).GetMethod(nameof(Parse));
                    var genericParseMethod = parseMethod!.MakeGenericMethod(prop.PropertyType);

                    prop.SetValue(result, genericParseMethod.Invoke(null, [fields, headers]));
                }
            }

            return result;
        }
    }
}
