using System;
using Lemmikki;

namespace Lemmikki
{
    class Program
    {
        static void Main(string[] args)
        {
            LemikkiDB db = new LemikkiDB();
            Console.WriteLine("Tervetuloa lemmikki tietokantaan.");
            while (true)
            {
                Console.WriteLine(
                    "Lisää uusi lemmikki: (L)\nPäivitä puhelinnumero: (P)\nEtsi lemmikkin perusteella puhelinnumero : (E)\nExit: Exit");
            
                string input = Console.ReadLine();

                switch (input)
                {
                    case "L":
                        db.AddLemmikki();
                        break;
                    case "P":
                        db.UpdatePhoneNumber();
                        break;
                    case "E":
                        db.FindPhonenumber();
                        break;
                    case "Exit":
                        System.Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Väärä komento. Yritä uudestaan.");
                        break;
                }
            }

        }
        
    }
    
}