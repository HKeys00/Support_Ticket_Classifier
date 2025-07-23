using System;
namespace Shared.Attributes
{
    /// <summary>
    /// Adds the meta data that the attached property is a nested class.
    /// Used for parsing nested objects in CSV files.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NestedObjectAttribute : Attribute
    {
    }
}
