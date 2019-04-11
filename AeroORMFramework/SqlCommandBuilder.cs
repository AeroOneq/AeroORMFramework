using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.SqlTypes;
using Newtonsoft.Json;

namespace AeroORMFramework
{
    static class SqlCommandBuilder
    {
        private static TypeConverter TypeConverter { get; } = new TypeConverter();
        #region CREATE TABLE COMMAND
        public static SqlCommand BuildCreateTableCommand<Entity>(List<PropertyInfo> propertiesList,
           SqlConnection connection)
        {
            return new SqlCommand
            {
                Connection = connection,
                CommandText = CreateTableCommandString<Entity>(propertiesList)
            };
        }

        /// <summary>
        /// Determines wether the column in the database can be null or not
        /// </summary> 
        /// Returns the "null" if the property can be null,
        /// "not null" otherwise
        /// </returns>
        private static string SetCanBeNullString(PropertyInfo property)
        {
            bool canBeNull = property.GetCustomAttribute<CanBeNullAttribute>().Value;
            if (canBeNull)
                return "null";
            return "not null";
        }
        /// <summary>
        /// If the programmer decided to set the data type himself, then we set this type,
        /// otherwise we take the default 
        /// </summary>
        /// <returns>
        /// The data-type
        /// </returns>
        private static string SetAzureSqlDataType(PropertyInfo property)
        {
            SetAzureSQLDataTypeAttribute setAzureSQLDataType =
                property.GetCustomAttribute<SetAzureSQLDataTypeAttribute>();
            JsonAttribute jsonAttribute = property.GetCustomAttribute<JsonAttribute>();
            //if the property is json-coded then it has a type "varchar(max)"
            if (jsonAttribute != null)
                return "nvarchar(max)";
            //if propgrammer decided to use default types, then the method returns the default types,
            //else the data-type selected by developer is implemented
            if (setAzureSQLDataType == null)
                return TypeConverter.GetAzureSQLType(property.PropertyType);
            return setAzureSQLDataType.SqlDataType;
        }
        /// <summary>
        /// Inserts the names of the columns, their data types and additional parameters
        /// into the string
        /// </summary>
        /// <returns>
        /// The string which descries the tables, and then put into the command text
        /// </returns>
        private static string InsertInitialColsInTheCommandText(List<PropertyInfo> propertiesList)
        {
            string insertionString = string.Empty;
            foreach (PropertyInfo property in propertiesList)
            {
                //get the null-state and data type
                string canBeNullString = SetCanBeNullString(property);
                string primaryKeyString = SetPrimaryKeyString(property);
                string azureSqlDataTypeString = SetAzureSqlDataType(property);
                insertionString += $"{property.Name} {azureSqlDataTypeString} " +
                    $"{canBeNullString}{primaryKeyString}, ";
            }
            return insertionString;
        }
        private static string SetPrimaryKeyString(PropertyInfo property)
        {
            string primaryKeyString = string.Empty;
            if (property.GetCustomAttribute<PrimaryKeyAttribute>() != null)
            {
                if (property.GetCustomAttribute<AutoincrementIDAttribute>() != null)
                    primaryKeyString += " IDENTITY (1,1)";
                primaryKeyString += " PRIMARY KEY";
            }
            return primaryKeyString;
        }
        private static string CreateTableCommandString<Type>(List<PropertyInfo> propertiesList)
        {
            string commandString = $"CREATE TABLE {typeof(Type).Name} (";
            commandString += InsertInitialColsInTheCommandText(propertiesList);
            //delete extra comma in the end of the command
            commandString.Remove(commandString.Length - 2, 2);
            commandString += ")";
            return commandString;
        }
        #endregion
        #region GET ALL RECORDS 
        public static SqlCommand BuildSelectAllCommand<Entity>(SqlConnection connection)
        {
            return new SqlCommand
            {
                CommandText = $"SELECT * FROM {typeof(Entity).Name}",
                Connection = connection
            };
        }
        #endregion
        #region INSERT RECORD COMAND
        public static SqlCommand BuildInsertCommand<Entity>(Entity entity,
            SqlConnection connection)
        {
            SqlCommand insertCommand = new SqlCommand
            {
                Connection = connection,
                CommandText = CreateInsertionCommandText(entity)
            };
            PropertyInfo[] propertiesArray = typeof(Entity).GetProperties();
            foreach (PropertyInfo property in propertiesArray)
            {
                if (property.GetCustomAttribute<PrimaryKeyAttribute>() == null)
                {
                    insertCommand.Parameters.AddWithValue($"@{property.Name}",
                        GetPropertyValueObjectRepresentation(entity, property));
                }
            }
            return insertCommand;
        }
        private static string CreateInsertionCommandText<Entity>(Entity entity)
        {
            PropertyInfo[] propertiesArray = typeof(Entity).GetProperties();

            string parametersNames = CreateParametersString(propertiesArray);
            string commandString = $"INSERT INTO {typeof(Entity).Name} {parametersNames} VALUES(";

            foreach (PropertyInfo propertyInfo in propertiesArray)
            {
                if (propertyInfo.GetCustomAttribute<AutoincrementIDAttribute>() == null)
                    commandString += $"@{propertyInfo.Name}, ";
            }
            //delete last space
            commandString = commandString.Remove(commandString.Length - 2, 2);
            commandString += ")";
            return commandString;
        }
        private static string CreateParametersString(PropertyInfo[] propertiesArray)
        {
            string parametersNames = "(";
            foreach (PropertyInfo property in propertiesArray)
            {
                if (property.GetCustomAttribute<AutoincrementIDAttribute>() == null)
                    parametersNames += $"{property.Name}, ";
            }
            //delete last space and coma
            parametersNames = parametersNames.Remove(parametersNames.Length - 2, 2);
            return parametersNames + ")";
        }
        private static object GetPropertyValueObjectRepresentation<Entity>(Entity entity,
            PropertyInfo property)
        {
            string valueString = string.Empty;
            if (property.GetCustomAttribute<JsonAttribute>() != null)
            {
                string jsonString = JsonConvert.SerializeObject(property.GetValue(entity));
                valueString += $"{jsonString}";
            }
            else
            {
                return property.GetValue(entity);
            }
            return valueString;
        }
        #endregion
        #region DELETE RECORD COMMAND
        public static SqlCommand BuildDeleteRecordCommand<Entity>(Entity entity,
            SqlConnection connection)
        {
            //get the primary property
            PropertyInfo primaryProperty = GetPrimaryProperty<Entity>();
            return new SqlCommand
            {
                Connection = connection,
                CommandText = $"DELETE FROM {typeof(Entity).Name} WHERE {primaryProperty.Name} " +
                $"like '{primaryProperty.GetValue(entity)}'"
            };
        }
        /// <summary>
        /// Returns a property which is marked with a primary key attribute
        /// </summary>
        private static PropertyInfo GetPrimaryProperty<Entity>()
        {
            PropertyInfo[] propertiesArray = typeof(Entity).GetProperties(BindingFlags.Public
                | BindingFlags.Instance);
            foreach (PropertyInfo property in propertiesArray)
            {
                if (property.GetCustomAttribute<PrimaryKeyAttribute>() != null)
                {
                    return property;
                }
            }
            return null;
        }
        #endregion
        #region DELETE TABLE COMMAND
        public static SqlCommand BuildDeleteTableCommand<Entity>(SqlConnection connection)
        {
            return new SqlCommand
            {
                Connection = connection,
                CommandText = $"DROP TABLE {typeof(Entity).Name}"
            };
        }
        #endregion
        #region UPDATE RECORD COMMAND
        public static SqlCommand BuildUpdateRecordComand<Entity>(Entity entity,
            SqlConnection connection)
        {
            List<PropertyInfo> propertiesList = typeof(Entity).GetProperties(BindingFlags.Public
                | BindingFlags.Instance).ToList();
            SqlCommand updateCommand = new SqlCommand
            {
                Connection = connection,
                CommandText = CreateUpdateRecordCommandText(propertiesList, entity)
            };
            foreach (PropertyInfo property in propertiesList)
            {
                if (property.GetCustomAttribute<PrimaryKeyAttribute>() == null)
                {
                    updateCommand.Parameters.AddWithValue($"@{property.Name}",
                        GetPropertyValueObjectRepresentation(entity, property));
                }
            }
            return updateCommand;
        }
        private static string CreateUpdateRecordCommandText<Entity>(List<PropertyInfo> propertiesList,
            Entity entity)
        {
            string updateRecordCommandText = $"UPDATE {typeof(Entity).Name} SET ";
            string commandEndingPredicate = string.Empty;
            foreach (PropertyInfo property in propertiesList)
            {
                if (property.GetCustomAttribute<PrimaryKeyAttribute>() != null)
                {
                    commandEndingPredicate += $"WHERE {property.Name} like " +
                        $"{GetPropertyValueObjectRepresentation(entity, property)}";
                }
                else
                {
                    updateRecordCommandText += $"{property.Name} = " +
                        $"@{property.Name}, ";
                }
            }
            //delete the last coma
            updateRecordCommandText = updateRecordCommandText.Remove(
                updateRecordCommandText.LastIndexOf(","), 1);
            updateRecordCommandText += commandEndingPredicate;
            return updateRecordCommandText;
        }
        #endregion
    }
}
