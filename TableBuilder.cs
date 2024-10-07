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

        public void CreateTable(SQLiteConnection connection)
        {
            try
            {
                if (TableExists(connection))
                {
                    Console.WriteLine($"Table '{_tableName}' already exists.");
                }
                else
                {
                    string query = BuildCreateTableQuery();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Table '{_tableName}' created successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the table: {ex.Message}");
            }
        }

        #region Private Methods
        private string BuildCreateTableQuery()
        {
            if (columns.Count == 0)
            {
                throw new InvalidOperationException("Table must have at least one column.");
            }

            string query = $"CREATE TABLE IF NOT EXISTS {_tableName} (";
            for (int i = 0; i < columns.Count; i++)
            {
                query += $"{columns[i].columnName} {columns[i].columnType}";
                if (i < columns.Count - 1)
                {
                    query += ", ";
                }
            }
            query += ");";
            return query;
        }

        private bool TableExists(SQLiteConnection connection)
        {
            string query = "SELECT name FROM sqlite_master WHERE type='table' AND name=@_tableName;";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@_tableName", _tableName);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }
        #endregion

    }
}
