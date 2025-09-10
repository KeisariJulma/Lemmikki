using System;
using Lemmikki;

namespace Lemmikki
{
    class Program
    {
        static void Main(string[] args)
        {
            LemikkiDB db = new LemikkiDB();
            Console.WriteLine("Welcome to Lemmikki Database!");
            while (true)
            {
                Console.WriteLine(
                    "Lisää uusi lemmikki: AddLemmikki\nPäivitä puhelinnumero: UpdatePhoneNumber\nEtsi lemmikki puhelinnumeron perusteella: FindByPhoneNumber\nExit: Exit");
            
                string input = Console.ReadLine();

                switch (input)
                {
                    case "AddLemmikki":
                        db.AddLemmikki();
                        break;
                    case "UpdatePhoneNumber":
                        db.UpdatePhoneNumber();
                        break;
                    case "FindPhoneNumber":
                        db.FindPhonenumber();
                        break;
                    case "Exit":
                        System.Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid command. Please try again.");
                        break;
                }
            }

        }
        
    }
    
}