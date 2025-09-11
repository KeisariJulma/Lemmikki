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
    
    
    
    public void UpdatePhoneNumber()
    {
        Console.WriteLine("Omistajan id ja uusi puhelinnumero:");
        string input = Console.ReadLine();
        string[] parts = input.Split(' ');

        if (parts.Length < 2)
        {
            Console.WriteLine("Anna kaikki kaksi tietoa välilyönnillä erotettuna.");
            return;
        }

        using (SqliteConnection connection = new SqliteConnection("Data Source=lemmikki.db"))
        {
            string updateQuery = "UPDATE Omistaja SET Puhelin = @parts[1] WHERE Id = @parts[0]";
            connection.Execute(updateQuery, new { NewPhoneNumber = parts[1], OwnerId = parts[0] });
        }
    }

    
    
    public void AddLemmikki()
    {
        Console.WriteLine("Omistajan nimi, puhelinnumero ja lemmikki nimi ja tyyppi:");
        string Input = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(Input)) { return; }
        string[] parts = Input.Split(' ');
        
        if (parts.Length < 4)
        {
            Console.WriteLine("Anna kaikki neljä tietoa välilyönnillä erotettuna.");
            return;
        }
        
        using (SqliteConnection connection = new SqliteConnection("Data Source=lemmikki.db"))
        {
            string insertOwnerQuery = @"INSERT INTO Omistaja (Nimi, Puhelin) VALUES (@Name, @Phone)";
            connection.Execute(insertOwnerQuery, new { Name = parts[0], Phone = parts[1] });
            long ownerId = connection.ExecuteScalar<long>("SELECT last_insert_rowid();");
            string insertPetQuery = @"INSERT INTO Lemmikki (Nimi, Laji, Omistajan_Id) VALUES (@Name, @Type, @OwnerId)";
            connection.Execute(insertPetQuery, new { Name = parts[2], Type = parts[3], OwnerId = ownerId });
        }
    }

    public void FindPhonenumber()
    {
        Console.WriteLine("Lemmikkin nimi");
        string  input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace( input)) { return; }
        
        using (SqliteConnection connection = new SqliteConnection("Data Source=lemmikki.db"))
        {
            string query = @"
                SELECT Omistaja.Puhelin
                FROM Omistaja
                JOIN Lemmikki ON Omistaja.Id = Lemmikki.Omistajan_Id
                WHERE Lemmikki.Nimi = @PetName
            ";
            var phoneNumber = connection.QueryFirstOrDefault<string>(query, new { PetName =  input });
            if (phoneNumber == null)
            {
                Console.WriteLine("Nimi ei löytynyt");
                return;
            }

            Console.WriteLine(phoneNumber);

        }
    }
}