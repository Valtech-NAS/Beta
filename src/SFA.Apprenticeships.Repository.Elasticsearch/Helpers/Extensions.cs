using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Helpers
{
    /// <summary>
    /// TODO::High::Needs moving to 'common' helpers project.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// The currency format
        /// </summary>
        public const string CurrencyFormat = @"&pound;{0:#,###,##0.00}";

        /// <summary>
        /// Converts string to byte array.
        /// </summary>
        /// <param name="toConvert">string to convert.</param>
        /// <returns>
        /// Byte array containing converted string.
        /// </returns>
        public static byte[] ToByteArray(this string toConvert)
        {
            var data = new byte[toConvert.Length * sizeof(char)];
            Buffer.BlockCopy(toConvert.ToCharArray(), 0, data, 0, data.Length);
            return data;
        }

        /// <summary>
        /// Extension method to read a value from the SqlDataReader object protecting against getting Nulls
        /// </summary>
        /// <typeparam name="T">The type of object to return</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// The value of type read from the field
        /// </returns>
        public static T GetSafeValue<T>(this IDataReader reader, string fieldName)
        {
            var fieldNumber = reader.GetOrdinal(fieldName);
            var value = reader.GetValue(fieldNumber);
            if (value is DBNull)
            {
                return default(T);
            }

            return (T)value;
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("ExtensionMethods::Clone the type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Gets the formatted currency from a decimal eg £9,999.99.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>a formatted string</returns>
        public static string GetFormattedCurrency(this decimal value)
        {
            if (value > 0m)
            {
                return string.Format(CurrencyFormat, value);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the formatted currency from a double eg £9,999.99.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>a formatted string eg £9,999.99</returns>
        public static string GetFormattedCurrency(this double value)
        {
            if (value > 0d)
            {
                return string.Format(CurrencyFormat, value);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the Service Code for this Enumeration.
        /// </summary>
        /// <typeparam name="T">The type of attribute</typeparam>
        /// <param name="value">The Enumeration to extract the Service Code for.</param>
        /// <returns>
        /// The Service code for the given Enumeration
        /// </returns>
        public static T GetAttributeValues<T>(this Enum value) where T : Attribute
        {
            // Get the type
            var type = value.GetType();

            // Get fieldinfo for this type
            var fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            var attribs = fieldInfo.GetCustomAttributes(typeof(T), false);
            if (attribs != null && attribs.Length > 0)
            {
                var attrib = attribs[0] as T;
                return attrib;
            }
            ////            var attribs = fieldInfo.GetCustomAttributes(typeof(T), false)[0] as T;

            return default(T);
        }

        /// <summary>
        /// Tries the get enum from description attribute.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="description">The description.</param>
        /// <param name="result">The result.</param>
        /// <returns>the enum from description</returns>
        public static bool TryGetEnumFromDescriptionAttribute<TEnum>(string description, out TEnum result) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var parsed = false;
            result = default(TEnum);

            try
            {
                foreach (var o in from object o in Enum.GetValues(typeof(TEnum))
                                  let en = o as Enum
                                  let serviceDescription = en.GetDescription()
                                  where !string.IsNullOrEmpty(serviceDescription)
                                && serviceDescription.Equals(description, StringComparison.OrdinalIgnoreCase)
                                  select o)
                {
                    result = (TEnum)o;
                    parsed = true;
                }
            }
            catch
            {
                result = default(TEnum);
            }

            return parsed;
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <typeparam name="T">Type parameter</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="attributeProperty">The attribute property.</param>
        /// <returns>
        /// The value of the attributeProperty.
        /// </returns>
        public static string GetAttributeValue<T>(this Enum value, string attributeProperty) where T : Attribute
        {
            var attribs = value.GetAttributeValues<T>();
            var result = attribs.GetType().InvokeMember(attributeProperty, BindingFlags.GetProperty, null, attribs, null);
            return result as string;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="value">The enumeration value to get the description for.</param>
        /// <returns>The description text set with this attribute</returns>
        public static string GetDescription(this Enum value)
        {
            var attribs = value.GetAttributeValues<DescriptionAttribute>();

            // Return the first if there was a match.
            return attribs != null ? attribs.Description : string.Empty;
        }

        /// <summary>
        /// Method to get the Enumeration "T" based on the given Service Code
        /// </summary>
        /// <typeparam name="T">The type of object to return</typeparam>
        /// <param name="description">The description to convert.</param>
        /// <returns>
        /// An Enumeration value of Type "T"
        /// </returns>
        public static T GetEnumFromDescription<T>(this string description) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (string.IsNullOrEmpty(description))
            {
                // should be null reference exception
                throw new ArgumentException(string.Format("Description cannot be null"));
            }

            var typeOfEnum = typeof(T);
            var attributeEnum = Enum.GetValues(typeOfEnum)
                                .OfType<Enum>()
                                .FirstOrDefault(x =>
                                {
                                    var attributes = x.GetAttributeValues<DescriptionAttribute>();
                                    var attributeValue = attributes != null ? attributes.Description : string.Empty;
                                    return attributeValue.Equals(description, StringComparison.InvariantCultureIgnoreCase);
                                });

            if (attributeEnum != null)
            {
                return (T)Convert.ChangeType(attributeEnum, typeOfEnum);
            }

            throw new ArgumentException(string.Format("No description exists for type {0} corresponding to value of {1}.", typeOfEnum, description));
        }
    }
}
