using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroORMFramework
{
    class SetTableNameAttribute : Attribute
    {
        public string TableName { get; }

        public SetTableNameAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
