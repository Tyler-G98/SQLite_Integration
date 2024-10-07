using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace SQLite_Integration
{
    public class TableBuilder
    {
        #region Fields
        private string _tableName;
        private List<(string columnName, string columnType)> columns;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName"></param>
        public TableBuilder(string tableName)
        {
            _tableName = tableName;
            columns = new List<(string columnName, string columnType)>();
        }

        public void AddColumn(string columnName, string columnType)
        {
            columns.Add((columnName, columnType));
        }

        public string BuildCreateTableQuery()
        {
            if ((columns.Count == 0) || string.IsNullOrEmpty(_tableName))
            {
                throw new InvalidOperationException("Table name and columns must be set before building the query");
            }

            string query = $"CREATE TABLE {_tableName} (";
            for (int i = 0; i < columns.Count; i++)
            {
                query += $"{columns[i].columnName} {columns[i].columnType}";
                if (i < columns.Count - 1)
                {
                    query += ", ";
                }
            }

            query += ")";
            return query;
        }

        public void CreateTable(SQLiteConnection connection)
        {
            string query = BuildCreateTableQuery();
            var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }
    }
}
