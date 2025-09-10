using Dapper;
using Microsoft.Data.Sqlite;

namespace Lemmikki;

public class LemikkiDB
{
    public LemikkiDB()
    {
        EnsureDatabaseExists();
    }

    private void EnsureDatabaseExists()
    {
        using (SqliteConnection connection = new SqliteConnection("Data Source=lemmikki.db"))
        {
            connection.Open();
            string createTablesQuery = @"
                CREATE TABLE IF NOT EXISTS Omistaja (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nimi TEXT,
                    Puhelin TEXT
                );
                CREATE TABLE IF NOT EXISTS Lemmikki (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nimi TEXT,
                    Laji TEXT,
                    Omistajan_Id INTEGER,
                    FOREIGN KEY (Omistajan_Id) REFERENCES Omistaja(Id)
                );
            ";
            connection.Execute(createTablesQuery);
        }
    }
}