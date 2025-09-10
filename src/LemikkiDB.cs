using System;
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
    
    
    
    public void UpdatePhoneNumber(int ownerId, string newPhoneNumber)
    {
        using (SqliteConnection connection = new SqliteConnection("Data Source=lemmikki.db"))
        {
            connection.Open();
            string updateQuery = "UPDATE Omistaja SET Puhelin = @NewPhoneNumber WHERE Id = @OwnerId";
            connection.Execute(updateQuery, new { NewPhoneNumber = newPhoneNumber, OwnerId = ownerId });
        }
    }

    public void AddLemmikki(string name, string type, int ownerId)
    {
        using (SqliteConnection connection = new SqliteConnection("Data Source=lemmikki.db"))
        {
            connection.Open();
            string insertQuery = @"IMSERT INTO Lemmikki (Nimi, Laji, Omistajan_Id) VALUES (@Name, @Type, @OwnerId)";
            connection.Execute(insertQuery, new { Name = name, Type = type, OwnerId = ownerId });
        }
    }
    
    public void AddLemmikki()
    {
        Console.WriteLine("Omistajan nimi, puhelinnumero ja lemmikki nimi ja tyyppi:");
        string name = Console.ReadLine();
        string[] parts = name.Split(' ');
        using (SqliteConnection connection = new SqliteConnection("Data Source=lemmikki.db"))
        {
            connection.Open();
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
        string name = Console.ReadLine();
        using (SqliteConnection connection = new SqliteConnection("Data Source=lemmikki.db"))
        {
            connection.Open();
            string query = @"
                SELECT Omistaja.Puhelin
                FROM Omistaja
                JOIN Lemmikki ON Omistaja.Id = Lemmikki.Omistajan_Id
                WHERE Lemmikki.Nimi = @PetName
            ";
            var phoneNumber = connection.QueryFirstOrDefault<string>(query, new { PetName = name });
            if (phoneNumber == null)
            {
                throw new Exception("Puhelinnumeroa ei löytynyt");
                return;
            }

            Console.WriteLine(phoneNumber);

        }
    }
}