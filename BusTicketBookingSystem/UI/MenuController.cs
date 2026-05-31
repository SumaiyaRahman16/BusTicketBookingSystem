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
         
            _busRepository.AddBus(new Bus("Dhaka-Metro-11", new EconomySeatingStrategy()));  // Bus 1
            _busRepository.AddBus(new Bus("915-VIP-BIZ", new BusinessSeatingStrategy()));     // Bus 2
            _busRepository.AddBus(new Bus("Sylhet-Exp-12", new BusinessSeatingStrategy()));   // Bus 3
            _busRepository.AddBus(new Bus("Cox-Night-77", new EconomySeatingStrategy()));     // Bus 4
            _busRepository.AddBus(new Bus("Ctg-Metro-99", new EconomySeatingStrategy()));     // Bus 5

     
            var busDhakaSylhet = _busRepository.GetBusByCoachNumber("Dhaka-Metro-11");
            var busDhakaCtg    = _busRepository.GetBusByCoachNumber("915-VIP-BIZ");
            var busSylhetDhaka = _busRepository.GetBusByCoachNumber("Sylhet-Exp-12");
            var busDhakaCox    = _busRepository.GetBusByCoachNumber("Cox-Night-77");
            var busCtgDhaka    = _busRepository.GetBusByCoachNumber("Ctg-Metro-99");

            _scheduleRepository.AddSchedule(busDhakaSylhet, "Dhaka", "Sylhet", DateTime.Today.AddHours(7), 650);         // Schedule 1
            _scheduleRepository.AddSchedule(busDhakaCtg,    "Dhaka", "Chittagong", DateTime.Today.AddHours(22), 1400);   // Schedule 2
            _scheduleRepository.AddSchedule(busSylhetDhaka, "Sylhet", "Dhaka", DateTime.Today.AddDays(1).AddHours(14), 1200); // Schedule 3
            _scheduleRepository.AddSchedule(busDhakaCox,    "Dhaka", "Cox's Bazar", DateTime.Today.AddHours(23), 900);       // Schedule 4
            _scheduleRepository.AddSchedule(busCtgDhaka,    "Chittagong", "Dhaka", DateTime.Today.AddDays(1).AddHours(8), 700); // Schedule 5 // Tomorrow 8:00 AM Economy Return
        }

        public void RunMainMenu()
        {

            Console.WriteLine("--- Bus Ticket Booking System ---");

            while (true)
            {
           

                Console.WriteLine("\n1. Create User");
                Console.WriteLine("2. Show Users");
                Console.WriteLine("3. Create Bus");
                Console.WriteLine("4. Show Buses");
                Console.WriteLine("5. Create Schedule");
                Console.WriteLine("6. Show Schedules");
                Console.WriteLine("7. Show Schedule Details");
                Console.WriteLine("8. Book Ticket");
                Console.WriteLine("9. Show Invoices of a user");
                Console.WriteLine("10. Pay Invoice");
                Console.WriteLine("11. Show Tickets of a User");
                Console.WriteLine("12. Exit");
        
                Console.Write("Select option: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": RegisterPassenger(); break;
                    case "2": ViewAllPassengers(); break;
                    case "3": CreateBusWorkflow(); break;
                    case "4": ViewBuses(); break;
                    case "5": CreateScheduleWorkflow(); break;
                    case "6": ViewSchedules(); break;
                    case "7": ViewScheduleDetailsGrid(); break;
                    case "8": BookTicketWorkflow(); break;
                    case "9": ShowInvoices(); break;
                    case "10": PayInvoiceWorkflow(); break;
                    case "11": Console.WriteLine("Feature active."); break;
                    case "12": return;
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
        private void CreateBusWorkflow()
        {
            Console.WriteLine("=== Create New Bus ===");
            Console.Write("Enter Coach Number (e.g., 915, Dhaka-Metro-12): ");
            string coachNumber = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(coachNumber))
            {
                Console.WriteLine("❌ Coach number cannot be empty.");
                return;
            }

            // Check if a coach with this exact number already exists
            if (_busRepository.GetBusByCoachNumber(coachNumber) != null)
            {
                Console.WriteLine($"❌ Error: Coach number '{coachNumber}' is already registered.");
                return;
            }

            Console.WriteLine("Select Seating Configuration Class:");
            Console.WriteLine("1. Economy Coach (40 Seats, 4 Columns Layout)");
            Console.WriteLine("2. Business Coach (28 Seats, 3 Columns Layout)");
            Console.Write("Selection (1-2): ");
            string classChoice = Console.ReadLine();

            ISeatingStrategy selectedStrategy;
            if (classChoice == "1")
            {
                selectedStrategy = new EconomySeatingStrategy();
            }
            else if (classChoice == "2")
            {
                selectedStrategy = new BusinessSeatingStrategy();
            }
            else
            {
                Console.WriteLine("❌ Invalid seating strategy selection.");
                return;
            }

            try
            {
                // Construct the new domain object using our Strategy Pattern choice
                Bus newBus = new Bus(coachNumber, selectedStrategy);
                _busRepository.AddBus(newBus);
        
                Console.WriteLine($"\n✅ Bus Successfully Added to Fleet!");
                Console.WriteLine($"Coach: {newBus.CoachNumber} | Class: {newBus.BusClass} | Capacity: {newBus.TotalSeats} seats");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to save bus: {ex.Message}");
            }
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
        sch.DisplaySeatingGrid();


    Console.Write("Enter Seat Choice (e.g., 1A, 2B, 3C): ");
    string seatInput = Console.ReadLine()?.Trim().ToUpper();

    if (string.IsNullOrEmpty(seatInput) || seatInput.Length < 2)
    {
        Console.WriteLine("❌ Invalid seat format choice.");
        return;
    }


    char seatLetter = seatInput[seatInput.Length - 1];
    string rowPart = seatInput.Substring(0, seatInput.Length - 1);

    if (!int.TryParse(rowPart, out int rowNum) || seatLetter < 'A' || seatLetter > 'D')
    {
        Console.WriteLine("❌ Invalid seat characters detected.");
        return;
    }


    int columnsPerRow = sch.AssignedBus.BusClass.Equals("Business", StringComparison.OrdinalIgnoreCase) ? 3 : 4;
    int colNum = seatLetter - 'A' + 1; 


    if (columnsPerRow == 3 && seatLetter == 'D')
    {
        Console.WriteLine("❌ Business class coaches do not contain D seats.");
        return;
    }


    int seatNum = (rowNum - 1) * columnsPerRow + colNum;


    if (seatNum < 1 || seatNum > sch.AssignedBus.TotalSeats)
    {
        Console.WriteLine($"❌ Seat {seatInput} does not exist on this coach.");
        return;
    }

    try {
        Ticket ticket = new Ticket(sch, passenger, seatNum);
        Invoice invoice = new Invoice.Builder()
            .ForTicket(ticket)
            .WithAmount(sch.BaseFare)
            .SetPaidStatus(false) 
            .Build();

        _invoiceRepository.AddInvoice(invoice);
        Console.WriteLine($"\n✅ Seat {seatInput} Secured! Ticket Issued.\n{invoice}");
    } 
    catch (Exception ex) { 
        Console.WriteLine($"❌ Refused: {ex.Message}"); 
    }
            PauseForUser();
        }

        private void ShowInvoices()
        {
            Console.WriteLine("=== Show Invoices of a User ===");
            Console.Write("Enter Passenger Email: ");
            string email = Console.ReadLine();

            var passenger = _userRepository.GetUserByEmail(email);
            if (passenger == null)
            {
                Console.WriteLine("❌ No passenger profile found with that email address.");
                return;
            }

            var userInvoices = _invoiceRepository.GetAllInvoices()
                .Where(inv => inv.Ticket.Passenger.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase))
                .ToList();

        
            Console.WriteLine($"\nInvoices for {passenger.FullName} ({email}):");
            if (!userInvoices.Any())
            {
                Console.WriteLine("No invoice history found for this user.");
            }
            else
            {
                foreach (var inv in userInvoices)
                {
                    Console.WriteLine(inv);
                }
            }
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