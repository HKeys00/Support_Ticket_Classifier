using System;

namespace Shared.Attributes
{
    /// <summary>
    /// Attribute used to parse CSV data into the correct database objects
    /// Removes the need to add a column name attribute to all columns that simply start with the type.
    /// E.g Ticket Type can be parsed to just Type because it contains the object prefix.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ObjectPrefixAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the prefix for the object this attribute is assigned.
        /// </summary>
        public string Prefix { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialises a new instance of the <see cref="ObjectPrefixAttribute"/> class.
        /// </summary>
        /// <param name="prefix">The prefix. (Usually the class name)</param>
        public ObjectPrefixAttribute(string prefix)
        {
            Prefix = prefix; 
        }

        #endregion
    }
}
