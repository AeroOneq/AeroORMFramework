using System;

namespace AeroORMFramework
{
    /// <summary>
    /// Attribute which is used to set the azure sql data type,
    /// if the programmer isnt sutisfied with the default choice
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SetAzureSQLDataTypeAttribute : Attribute
    {
        public string SqlDataType { get; }

        public SetAzureSQLDataTypeAttribute(string sqlDataType)
        {
            SqlDataType = sqlDataType; 
        }
    }
}
