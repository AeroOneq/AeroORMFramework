using System;
using System.Collections.Generic;
using AeroORMFramework.CustomTypes;
using Newtonsoft.Json;

namespace AeroORMFramework
{
    internal class TypeConverter
    {
        /// <summary>
        /// Dictionary which matches the types from C# to types of Azure SQL
        /// </summary>
        private Dictionary<Type, string> TypesDictionary { get; set; } = new Dictionary<Type, string>
        {
            {typeof(string), "nvarchar(max)" },
            //DateTime
            {typeof(DateTime), "datetime" },
            //Bool
            {typeof(bool), "bit" },
            //Image types
            {typeof(byte[]), "varbinary(max)" },
            //Value types
            {typeof(byte), "tinyint" },
            {typeof(sbyte), "smallint" },
            {typeof(short), "smallint" },
            {typeof(ushort), "int" },
            {typeof(int), "int" },
            {typeof(uint), "bigint" },
            {typeof(long), "bigint" },
            {typeof(ulong), "bigint" },
            {typeof(double), "float(53)" },
            {typeof(float), "float(53)" },
            {typeof(decimal), "decimal(38, 38)" },
            //Common types
            {typeof(Email), "nvarchar(255)" },
            {typeof(Name), "nvarchar(255)" },
            {typeof(Surname), "nvarchar(255)" },
            //Json type (Json class can be initialized with any class)
            {typeof(Json<object>), "nvarchar(max)" }
        };
        /// <summary>
        /// With the help of TypesDictionary matches the type in cSharp to type in AzureSQL
        /// </summary>
        /// String representation of a SQL type which mathces the CsharpType
        /// Empty string in case of exception
        /// </returns>
        public string GetAzureSQLType(Type cSharpType)
        {
            try
            {
                return TypesDictionary[cSharpType];
            }
            catch (KeyNotFoundException ex)
            {
                ExceptionHandler.Handle(ex);
                return "varchar(max)";
            }
            catch (ArgumentNullException ex)
            {
                ExceptionHandler.Handle(ex);
                return string.Empty;
            }
        }
    }
}
