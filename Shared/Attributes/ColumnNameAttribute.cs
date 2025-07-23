using System;
namespace Shared.Attributes
{
    /// <summary>
    /// Adds the meta data of a column name to a property.
    /// This column maps to the csv that is seeded into the database.
    /// Allows for a custom CSV parser to be created.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the column this property maps to.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialises a new instance of the <see cref="ColumnNameAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the CSV column this property maps to.</param>
        public ColumnNameAttribute(string name)
        {
            Name = name;
        }

        #endregion
    }
}
