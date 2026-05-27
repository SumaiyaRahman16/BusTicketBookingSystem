using System;
using BusTicketBookingSystem.Models;
using BusTicketBookingSystem.Models.Bus;
using BusTicketBookingSystem.Models.Bus.Strategies;

namespace BusTicketBookingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Real-Time Terminal Input Test ===\n");
           
            int userCounter = 1;
            try
            {
            
                Console.Write("Enter Full Name: ");
                string inputtedName = Console.ReadLine();

                Console.Write("Enter Mobile Number: ");
                string inputtedMobile = Console.ReadLine();

                Console.Write("Enter Email Address: ");
                string inputtedEmail = Console.ReadLine();

                string generatedId = $"USR-{userCounter++}";

                Console.WriteLine("\nProcessing registration data...");
                Console.WriteLine("--------------------------------------------");

 
                User newUser = new User( generatedId , inputtedName, inputtedMobile, inputtedEmail);
                

                Console.WriteLine("✅ User Instance Created Successfully!");
                Console.WriteLine($"System Output: {newUser}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Input Validation Failed: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}