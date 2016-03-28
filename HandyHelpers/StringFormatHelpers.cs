using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection;

namespace HandyHelpers
{
    /// <summary>
    /// Helper methods for formating strings in different ways
    /// </summary>
    public static class StringFormatHelpers
    {
        public static string Format(this string template, IDictionary<string, object> properties)
        {
            return Format(template, properties, null);
        }

        /// <summary>
        /// Replaces the format item or items in a specified string with the string representation of the corresponding object. 
        /// </summary>
        /// <param name="template">
        ///   A composite format string, in which the placeholders should be in the form of {index,align:format},
        ///   where index is the key in the properties dictionary, and align and format in the same form as in string.Format
        /// </param>
        /// <param name="properties">A property bag containing zero or more objects to format</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information</param>
        /// <returns>A copy of format in which the format item or items have been replaced by the string representation of the values in the property bag. </returns>
        public static string Format(this string template, IDictionary<string, object> properties, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(template))
            {
                // throw NullReferenceException instead of ArgumentNullException to mimic the bahavior of an instance method
                throw new NullReferenceException("string template cannot be null");
            }

            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            var convertedTemplate = string.Empty;
            ICollection<string> propertyIndexes = null;

            TryParseAndConvertTemplate(template, out convertedTemplate, out propertyIndexes);

            var args = propertyIndexes.Select(key => properties[key]).ToArray();

            return string.Format(formatProvider, convertedTemplate, args);
        }

        public static string Format(this string template, object objectToFormat)
        {
            return Format(template, objectToFormat, null);
        }

        public static string Format(this string template, object objectToFormat, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(template))
            {
                // throw NullReferenceException instead of ArgumentNullException to mimic the bahavior of an instance method
                throw new NullReferenceException("string template cannot be null");
            }

            if (objectToFormat == null)
            {
                throw new ArgumentNullException(nameof(objectToFormat));
            }

            var convertedTemplate = string.Empty;
            ICollection<string> propertyIndexes = null;

            TryParseAndConvertTemplate(template, out convertedTemplate, out propertyIndexes);

            var args = propertyIndexes.Select(key => ExtractObjectFromPropertyPath(key, objectToFormat)).ToArray();

            return string.Format(formatProvider, convertedTemplate, args);
        }

        /// <summary>
        /// Convert the template into string.Format template and return the referenced property indexes in order
        /// </summary>
        /// <param name="template"></param>
        /// <param name="convertedTemplate"></param>
        /// <param name="propertyKeysInOrder"></param>
        internal static void TryParseAndConvertTemplate(string template, out string convertedTemplate, out ICollection<string> propertyKeysInOrder)
        {
            var stringBuilder = new StringBuilder(template);
            var pos = 0;
            var propertyKeys = new List<string>();
            var currentParamsIndex = 0;

            // convert the template into the normal {0,xxx:yyy} format and let string.Format handle the rest
            while (pos < stringBuilder.Length)
            {
                // search for a '{'
                if (stringBuilder[pos] == '{')
                {
                    ++pos;

                    // make sure it's not escaped
                    if (stringBuilder[pos] != '{')
                    {
                        var endPos = pos;
                        while (stringBuilder[endPos] != '}')
                        {
                            if (endPos == stringBuilder.Length)
                            {
                                throw new FormatException("The given template is malformed.");
                            }

                            ++endPos;
                        }

                        var key = stringBuilder.ToString(pos, endPos - pos);
                        var indexInString = currentParamsIndex.ToString(CultureInfo.InvariantCulture);
                        propertyKeys.Add(key);
                        stringBuilder.Replace(key, indexInString, pos, endPos - pos);
                        pos += indexInString.Length;
                        ++currentParamsIndex;
                    }
                }

                ++pos;
            }

            convertedTemplate = stringBuilder.ToString();
            propertyKeysInOrder = propertyKeys;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        internal static object ExtractObjectFromPropertyPath(string path, object root)
        {
            var obj = root;

            foreach(var propertyName in path.Split('.'))
            {
                if (obj == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot access '{0}' in null", propertyName));
                }

                var propertyInfo = obj.GetType().GetProperty(propertyName);
                obj = propertyInfo.GetValue(obj);
            }

            return obj;
        }
    }
}
