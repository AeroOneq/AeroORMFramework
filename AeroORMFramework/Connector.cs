using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SqlClient;

namespace AeroORMFramework
{
    /// <summary>
    /// Main class, the object of which connects the entities of the programm
    /// with database
    /// </summary>
    public class Connector
    {
        #region Connection settings
        /// <summary>
        /// Typical azure connection string with server and database parameters
        /// </summary>
        public string ConnectionString { get; }
        private Database Database { get; }
        #endregion
        /// <summary>
        /// Initializes the connection to database with the given connection string
        /// </summary>
        public Connector(string connectionString)
        {
            ConnectionString = connectionString;
            Database = new Database(ConnectionString);
        }
        /// <summary>
        /// Initializes the connection to database with the given connection
        /// </summary>
        /// <param name="connection"></param>
        public Connector(SqlConnection connection)
        {
            ConnectionString = connection.ConnectionString;
            Database = new Database(ConnectionString);
        }

        #region Add Table
        /// <summary>
        /// Creates a table which represents the given entity
        /// </summary>
        /// <typeparam name="Entity">
        /// The typw which will be represented in the table
        /// </typeparam>
        public void AddTable<Entity>()
        {
            Database.CreateNewTable<Entity>(GetAllProperties(typeof(Entity)));
        }
        /// <summary>
        /// Gets all properties of a given type (if it has at least one)
        /// and returns the list of them
        /// </summary>
        /// <exception cref="NotAppropriateTypeException">
        /// Throws that exception when the type has no public instance property
        /// </exception>
        private List<PropertyInfo> GetAllProperties(Type type)
        {
            List<PropertyInfo> propertiesList = type.GetProperties(BindingFlags.Public |
                BindingFlags.Instance).ToList();
            CheckProperties(propertiesList);
            return propertiesList;
        }
        private void CheckProperties(List<PropertyInfo> propertiesList)
        {
            if (propertiesList.Count == 0)
            {
                throw new NotAppropriateTypeException("In the type there must" +
                    " be at least one property");
            }
        }
        #endregion
        #region Get all Records
        /// <summary>
        /// Gets all records from the table which represents the given type
        /// </summary>
        /// <returns>The list of objects</returns>
        public List<Entity> GetAllRecords<Entity>()
            where Entity : new()
        {
            return Database.GetAllRecords<Entity>();
        }
        #endregion
        #region Get records
        /// <summary>
        /// Gets all records where the value of a given column is equal to value
        /// </summary>
        /// <returns>The List of found entities</returns>
        public List<Entity> GetRecords<Entity>(string columnName, object value)
            where Entity : new()
        {
            return Database.GetRecords<Entity>(columnName, value);
        }
        #endregion
        #region Insert new Record
        /// <summary>
        /// Inserts the object recrod in the table which represents the type
        /// </summary>
        public void Insert<Entity>(Entity entity)
            where Entity : new()
        {
            Database.InsertRecord(entity);
        }
        #endregion
        #region Delete table
        /// <summary>
        /// Delete the table which represents the given type
        /// </summary>
        public void DeleteTable<Entity>()
            where Entity : new()
        {
            Database.DeleteTable<Entity>();
        }
        #endregion
        #region Delete record
        /// <summary>
        /// Deletes the record which represents the given object
        /// </summary>
        /// <typeparam name="Entity">The type of the object</typeparam>
        /// <param name="entity">The object which will be deleted</param>
        public void DeleteRecord<Entity>(Entity entity)
            where Entity : new()
        {
            Database.DeleteRecord(entity);
        }
        #endregion
        #region Find record
        /// <summary>
        /// Finds the record which satisfies the given parameters
        /// </summary>
        /// <typeparam name="Entity">
        /// The type of the object that will be returned
        /// </typeparam>
        /// <param name="columnName">
        /// The name of the property, on which the search wil be based
        /// </param>
        /// <param name="value">
        /// The value which you want to find
        /// </param>
        /// <returns>
        /// The object of the type given
        /// </returns>
        public Entity GetRecord<Entity>(string columnName, object value)
            where Entity : new()
        {
            return Database.FindRecord<Entity>(columnName, value);
        }
        #endregion
        #region Update record
        /// <summary>
        /// Updates the record (basing on the primary key property) with the
        /// given new record
        /// </summary>
        public void UpdateRecord<Entity>(Entity entity)
        {
            Database.UpdateRecord<Entity>(entity);
        }
        #endregion
    }
}
