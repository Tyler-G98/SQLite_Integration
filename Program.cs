namespace SQLite_Integration;
using System.Data.SQLite;


internal class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=database.db";
        var connection = new SQLiteConnection(connectionString);

        connection.Open();

        string query;
        var command = new SQLiteCommand(null, null);
        try
        {
            query = "CREATE TABLE person (id INTEGER PRIMARY KEY, name TEXT NOT NULL, age INTEGER)";
            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.Message);
        }


        query = "INSERT INTO person (name, age) VALUES ('@name', @age)";
        command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@name", "John Doe");
        command.Parameters.AddWithValue("@age", 30);
        command.ExecuteNonQuery();

        query = "SELECT * FROM person";
        command = new SQLiteCommand(query, connection);
        var reader = command.ExecuteReader();
        while(reader.Read())
        {
            Console.WriteLine(reader["name"] + " " + reader["age"]);
        }

        query = "DELETE FROM person";
        command = new SQLiteCommand(query, connection);
        command.ExecuteNonQuery();

        query = "SELECT * FROM person";
        command = new SQLiteCommand(query, connection);
        command.ExecuteNonQuery();
        while (reader.Read())
        {
            Console.WriteLine(reader["name"] + " " + reader["age"]);
        }
    }
}


