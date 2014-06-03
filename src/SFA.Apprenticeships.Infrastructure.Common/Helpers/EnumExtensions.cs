using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using CuttingEdge.Conditions;

namespace SFA.Apprenticeships.Infrastructure.Common.Helpers
{
    /// <summary>
    /// Enum extensions and helpers
    /// </summary>
    public static class EnumExtensions
    {
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
            if (attribs.Length > 0)
            {
                var attrib = attribs[0] as T;
                return attrib;
            }

            return default(T);
        }

        /// <summary>
        /// Tries the get enum from description attribute.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="description">The description.</param>
        /// <param name="result">The result.</param>
        /// <returns>the enum from description</returns>
        public static bool TryGetEnumFromDescription<TEnum>(string description, out TEnum result) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var parsed = false;
            result = default(TEnum);

            try
            {
                foreach (var o in from object o in Enum.GetValues(typeof(TEnum))
                                  let en = o as Enum
                                  let serviceDescription = GetDescription(en)
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
            Condition.Requires(description, "description").IsNotNullOrEmpty();

            var typeOfEnum = typeof(T);
            var attributeEnum = Enum.GetValues(typeOfEnum)
                                .OfType<Enum>()
                                .FirstOrDefault(x =>
                                {
                                    var attributes = GetAttributeValues<DescriptionAttribute>(x);
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
