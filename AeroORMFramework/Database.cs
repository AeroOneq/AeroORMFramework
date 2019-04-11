using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SqlClient;
using AeroORMFramework.CustomTypes;
using Newtonsoft.Json;
namespace AeroORMFramework
{
    internal class Database
    {
        private string ConnectionString { get; }

        public Database(string connectionString) => ConnectionString = connectionString;

        private SqlConnection CreateNewConnection()
        {
            return new SqlConnection
            {
                ConnectionString = ConnectionString
            };
        }

        #region Create new table
        /// <summary>
        /// Creates new table in the database which represents the given type entity
        /// </summary>
        /// <param name="propertiesList">
        /// List of properties which are defined in the type
        /// </param>
        internal void CreateNewTable<Entity>(List<PropertyInfo> propertiesList)
        {
            SqlConnection connection = CreateNewConnection();
            try
            {
                connection.Open();
                SqlCommand createTableCommand = SqlCommandBuilder.BuildCreateTableCommand
                    <Entity>(propertiesList, connection);
                createTableCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion
        #region Delete table
        public void DeleteTable<Entity>()
        {
            SqlConnection connection = CreateNewConnection();
            connection.Open();
            SqlCommand deleteTableCommand = SqlCommandBuilder.BuildDeleteTableCommand<Entity>(connection);
            deleteTableCommand.ExecuteNonQuery();
        }
        #endregion
        #region Get all records
        internal List<Entity> GetAllRecords<Entity>()
            where Entity : new()
        {
            SqlConnection connection = CreateNewConnection();
            connection.Open();
            SqlCommand getAllRecordsCommand = SqlCommandBuilder.BuildSelectAllCommand<Entity>(connection);
            SqlDataReader sqlDataReader = getAllRecordsCommand.ExecuteReader();
            return CreateEntitiesList<Entity>(sqlDataReader);
        }
        /// <summary>
        /// Creates the list of entities (with help of already executed sqlDataReader)
        /// of a given type and sets the properties of every instance
        /// </summary>
        private List<Entity> CreateEntitiesList<Entity>(SqlDataReader sqlDataReader)
            where Entity : new()
        {
            List<Entity> entitiesList = new List<Entity>();
            PropertyInfo[] propertiesArray = typeof(Entity).GetProperties(BindingFlags.Public
                | BindingFlags.Instance);
            while (sqlDataReader.Read())
            {
                Entity entity = new Entity();
                for (int i = 0; i < propertiesArray.Length; i++)
                {
                    object columnData = sqlDataReader.GetValue(i);
                    if (propertiesArray[i].GetCustomAttribute<JsonAttribute>() != null)
                    {
                        columnData = JsonConvert.DeserializeObject((string)columnData,
                            propertiesArray[i].PropertyType);
                        propertiesArray[i].SetValue(entity, columnData, null);
                    }
                    else
                        propertiesArray[i].SetValue(entity, columnData);
                }
                entitiesList.Add(entity);
            }
            return entitiesList;
        }
        #endregion
        #region Insert record
        public void InsertRecord<Entity>(Entity entity)
            where Entity : new()
        {
            SqlConnection connection = CreateNewConnection();
            try
            {
                connection.Open();
                SqlCommand insertionCommand = SqlCommandBuilder.BuildInsertCommand(entity,
                    connection);
                insertionCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion
        #region Delete record
        public void DeleteRecord<Entity>(Entity entity)
        {
            SqlConnection connection = CreateNewConnection();
            try
            {
                connection.Open();
                SqlCommand deleteRecordCommand = SqlCommandBuilder.BuildDeleteRecordCommand(entity, connection);
                deleteRecordCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion
        #region Find record
        /// <summary>
        /// Finds the record in the table which satisfies the given parameters
        /// </summary>
        /// <param name="columnName">The name of the property, based on which the search is going</param>
        /// <param name="value">the value of the property</param>
        /// <returns>The instance of the Entity if the object was found, the empty object otherwise</returns>
        public Entity FindRecord<Entity>(string columnName, object value)
            where Entity : new()
        {
            SqlConnection connection = CreateNewConnection();
            try
            {
                connection.Open();
                PropertyInfo findingProperty = GetFindingProperty<Entity>(columnName);
                List<Entity> allRecordsList = GetAllRecords<Entity>();
                return GetTheRecordInTheListOfEntities(allRecordsList, findingProperty, value);
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// Returns the Property Info object which indicates which 
        /// property will be used in the search
        /// </summary>
        /// <param name="columnName"> The string name of the property</param>
        private PropertyInfo GetFindingProperty<Entity>(string columnName)
        {
            PropertyInfo[] propertyInfosArray = typeof(Entity).GetProperties(BindingFlags.Public
                | BindingFlags.Instance);
            foreach (PropertyInfo property in propertyInfosArray)
            {
                if (property.Name == columnName)
                {
                    return property;
                }
            }
            return null;
        }
        /// <summary>
        /// Finds and returns the entity in the List of entities
        /// </summary>
        private Entity GetTheRecordInTheListOfEntities<Entity>(List<Entity> allRecordsList,
            PropertyInfo findingProperty, object value) where Entity : new()
        {
            foreach (Entity entity in allRecordsList)
            {
                if (findingProperty.GetValue(entity).Equals(value))
                {
                    return entity;
                }
            }
            return new Entity();
        }
        #endregion
        #region Get records
        public List<Entity> GetRecords<Entity>(string columnName, object value)
            where Entity : new()
        {
            SqlConnection connection = CreateNewConnection();
            try
            {
                connection.Open();
                PropertyInfo findingProperty = GetFindingProperty<Entity>(columnName);
                SqlCommand getRecordsCommand = CreateGetRecordsCommand<Entity>(findingProperty,
                    value, connection);
                SqlDataReader sqlDataReader = getRecordsCommand.ExecuteReader();
                return CreateEntitiesList<Entity>(sqlDataReader);
            }
            finally
            {
                connection.Close();
            }
        }
        private SqlCommand CreateGetRecordsCommand<Entity>(PropertyInfo property, object value,
            SqlConnection connection)
        {
            SqlCommand getRecordsCommand = new SqlCommand
            {
                Connection = connection,
                CommandText = $"SELECT * FROM {typeof(Entity).Name} WHERE {property.Name} like @value"
            };
            getRecordsCommand.Parameters.AddWithValue("@value", value);
            return getRecordsCommand;
        }
        #endregion
        #region Update record
        public void UpdateRecord<Entity>(Entity entity)
        {
            SqlConnection connection = CreateNewConnection();
            try
            {
                connection.Open();
                SqlCommand updateRecordCommand = SqlCommandBuilder.
                    BuildUpdateRecordComand(entity, connection);
                updateRecordCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion
    }
}
