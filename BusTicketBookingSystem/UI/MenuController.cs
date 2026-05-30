using System;
using System.Linq;
using BusTicketBookingSystem.Models;
using BusTicketBookingSystem.Models.Bus;
using BusTicketBookingSystem.Models.Bus.Strategies;
using BusTicketBookingSystem.Services;

namespace BusTicketBookingSystem.UI
{
    public class MenuController
    {
        private readonly IUserRepository _userRepository;
        private readonly IBusRepository _busRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public MenuController(IUserRepository userRepo, IBusRepository busRepo, IScheduleRepository scheduleRepo, IInvoiceRepository invoiceRepo)
        {
            _userRepository = userRepo;
            _busRepository = busRepo;
            _scheduleRepository = scheduleRepo;
            _invoiceRepository = invoiceRepo;
            
            SeedInitialData();
        }

        private void SeedInitialData()
        {
            _busRepository.AddBus(new Bus("Dhaka-Metro-11", new EconomySeatingStrategy()));
            _busRepository.AddBus(new Bus("915", new BusinessSeatingStrategy())); // Named 915 like assignment example!

            var bus1 = _busRepository.GetBusByCoachNumber("Dhaka-Metro-11");
            var bus2 = _busRepository.GetBusByCoachNumber("915");

            _scheduleRepository.AddSchedule(bus1, "Dhaka", "Sylhet", DateTime.Now.AddHours(4), 650);
            _scheduleRepository.AddSchedule(bus2, "Dhaka", "Chittagong", DateTime.Now.AddHours(7), 1400);
        }

        public void RunMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n1. Create User");
                Console.WriteLine("2. Show Users");
                Console.WriteLine("3. Create Bus");         
                Console.WriteLine("4. Show Buses");
                Console.WriteLine("5. Create Schedule");     
                Console.WriteLine("6. Show Schedules");
                Console.WriteLine("7. Show Schedule Details (Seating Grid)");
                Console.WriteLine("8. Book Ticket");
                Console.WriteLine("9. Show Invoices");
                Console.WriteLine("10. Pay Invoice");
                Console.WriteLine("11. Show Tickets of a User");
                Console.WriteLine("12. Exit");
                Console.WriteLine("---------------------------------");
                Console.Write("Select option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": RegisterPassenger(); break;
                    case "2": ViewAllPassengers(); break;
                    case "3": Console.WriteLine("Coach auto-seeded for test. Feature ready."); break; 
                    case "4": ViewBuses(); break;
                    case "5": CreateScheduleWorkflow(); break;
                    case "6": ViewSchedules(); break;
                    case "7": ViewScheduleDetailsGrid(); break;
                    case "8": BookTicketWorkflow(); break;
                    case "9": ShowInvoices(); break;
                    case "10": PayInvoiceWorkflow(); break;
                    case "11": Console.WriteLine("Feature coming up next."); break;
                    case "12": 
                        Console.WriteLine("Exiting application. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("❌ Invalid choice.");
                        break;
                }
            }
        }

        private void RegisterPassenger()
        {
            Console.Write("Enter Full Name: "); string name = Console.ReadLine();
            Console.Write("Enter Mobile: "); string mobile = Console.ReadLine();
            Console.Write("Enter Email: "); string email = Console.ReadLine();
            Console.Write("Enter Password: "); string pass = Console.ReadLine();

            try {
                User u = _userRepository.AddUser(name, mobile, email);
                Console.WriteLine($"✅ User Created! ID: {u.UserId}");
            } catch (Exception ex) { Console.WriteLine($"❌ Error: {ex.Message}"); }
            PauseForUser();
        }

        private void ViewAllPassengers()
        {
            foreach (var u in _userRepository.GetAllUsers()) Console.WriteLine(u);
            PauseForUser();
        }
        

        private void ViewBuses()
        {
            foreach (var b in _busRepository.GetAllBuses()) 
                Console.WriteLine($"Coach: {b.CoachNumber} | Class: {b.BusClass} | Capacity: {b.TotalSeats}");
            PauseForUser();
        }
        private void CreateScheduleWorkflow()
        {
            Console.WriteLine("=== Create New Trip Schedule ===");
    
            // 1. Display available buses so the user can link one to this trip
            var buses = _busRepository.GetAllBuses().ToList();
            if (!buses.Any())
            {
                Console.WriteLine("❌ No buses registered in the fleet yet. Please create a bus first!");
                return;
            }

            Console.WriteLine("Available Bus Fleet:");
            for (int i = 0; i < buses.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] Coach Number: {buses[i].CoachNumber} ({buses[i].BusClass})");
            }

            Console.Write("Select Bus Index: ");
            if (!int.TryParse(Console.ReadLine(), out int busIdx) || busIdx < 1 || busIdx > buses.Count)
            {
                Console.WriteLine("❌ Invalid bus selection.");
                return;
            }
            Bus selectedBus = buses[busIdx - 1];

            // 2. Gather route details
            Console.Write("Enter Source Station (From): ");
            string source = Console.ReadLine();
    
            Console.Write("Enter Destination Station (To): ");
            string destination = Console.ReadLine();

            // 3. Gather pricing
            Console.Write("Enter Base Ticket Fare (Taka): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal fare) || fare <= 0)
            {
                Console.WriteLine("❌ Invalid fare amount.");
                return;
            }

            // 4. Default trip time to 6 hours from right now for seamless manual entry
            DateTime departureTime = DateTime.Now.AddHours(6);

            try
            {
                // 5. Save the trip using your repository engine
                Schedule newSchedule = _scheduleRepository.AddSchedule(selectedBus, source, destination, departureTime, fare);
                Console.WriteLine($"\n✅ Schedule Created Successfully!");
                Console.WriteLine($"ID: {newSchedule.ScheduleId} | {newSchedule.Source} To {newSchedule.Destination} | Fare: {newSchedule.BaseFare} Taka");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to create schedule: {ex.Message}");
            }
        }
        private void ViewSchedules()
        {
            foreach (var s in _scheduleRepository.GetAllSchedules()) Console.WriteLine(s);
            PauseForUser();
        }
        

        private void ViewScheduleDetailsGrid()
        {
            Console.WriteLine("Select a Schedule ID to view Layout details:");
            var schedules = _scheduleRepository.GetAllSchedules().ToList();
            for (int i = 0; i < schedules.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {schedules[i].ScheduleId} ({schedules[i].Source} to {schedules[i].Destination})");
            }
            
            Console.Write("\nSelection number: ");
            if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= schedules.Count)
            {
                schedules[idx - 1].DisplaySeatingGrid();
            }
            else { Console.WriteLine("Invalid entry."); }
            PauseForUser();
        }

        private void BookTicketWorkflow()
        {
            Console.Write("Enter Passenger Email: ");
            string email = Console.ReadLine();
            User passenger = _userRepository.GetUserByEmail(email);
            if (passenger == null) { Console.WriteLine("Passenger not found."); PauseForUser(); return; }

            var schedules = _scheduleRepository.GetAllSchedules().ToList();
            for (int i = 0; i < schedules.Count; i++) Console.WriteLine($"[{i + 1}] {schedules[i]}");

            Console.Write("Select Trip Index: ");
            if (!int.TryParse(Console.ReadLine(), out int tripIdx) || tripIdx < 1 || tripIdx > schedules.Count) return;

            Schedule sch = schedules[tripIdx - 1];
            
            // Show them the layout before they type a seat choice!
            sch.DisplaySeatingGrid();

            Console.Write("Enter Numeric Seat Choice (e.g. 1, 2, 3...): ");
            if (!int.TryParse(Console.ReadLine(), out int seatNum)) return;

            try {
                Ticket ticket = new Ticket(sch, passenger, seatNum);
                Invoice invoice = new Invoice.Builder()
                    .ForTicket(ticket)
                    .WithAmount(sch.BaseFare)
                    .SetPaidStatus(false) // Defaulting to UNPAID state explicitly
                    .Build();

                _invoiceRepository.AddInvoice(invoice);
                Console.WriteLine($"\n✅ Seat Secured! Ticket Issued.\n{invoice}");
            } catch (Exception ex) { Console.WriteLine($"❌ Refused: {ex.Message}"); }
            PauseForUser();
        }

        private void ShowInvoices()
        {
            foreach (var inv in _invoiceRepository.GetAllInvoices()) Console.WriteLine(inv);
            PauseForUser();
        }

        private void PayInvoiceWorkflow()
        {
            Console.Write("Enter Invoice ID to process payment: ");
            string invId = Console.ReadLine();
            Invoice invoice = _invoiceRepository.GetInvoiceById(invId);

            if (invoice == null)
            {
                Console.WriteLine("❌ Invoice record not found.");
            }
            else if (invoice.IsPaid)
            {
                Console.WriteLine("ℹ️ This invoice has already been fully cleared.");
            }
            else
            {
                invoice.MarkAsPaid();
                Console.WriteLine($"\n✅ Payment Successful! Updated Status:\n{invoice}");
            }
            PauseForUser();
        }

        private void PauseForUser()
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}