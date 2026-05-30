using System;
using BusTicketBookingSystem.Models;
using BusTicketBookingSystem.Services;
using BusTicketBookingSystem.UI;

namespace BusTicketBookingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Initialize our application engine dependencies in system memory
            IUserRepository userRepository = new UserRepository();
            IBusRepository busRepository = new BusRepository();
            IScheduleRepository scheduleRepository = new ScheduleRepository();
            IInvoiceRepository invoiceRepository = new InvoiceRepository();
            
            // 2. Generate secured financial receipt defaulting to UNPAID
       

            // 2. Pass dependencies into the terminal ui management layer
            MenuController uiCoordinator = new MenuController(userRepository, busRepository, scheduleRepository , invoiceRepository);

            // 3. Fire up the continuous application execution loop
            uiCoordinator.RunMainMenu();
        }
    }
}