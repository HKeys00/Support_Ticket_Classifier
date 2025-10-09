using System.ComponentModel;
using System.Reflection;
using System;

namespace Shared.Helpers
{
    /// <summary>
    /// Helper methods to read enum values into strings.
    /// </summary>
    public static class EnumReader
    {
        /// <summary>
        /// Returns the descriptive string value of an enum if it exists.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="enumerationValue">The type of enum.</param>
        /// <returns>The description or the enum value as a stirng.</returns>
        public static string GetDescription<T>(this T enumerationValue) where T : Enum
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
    }
}
