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
            IUserRepository userRepository = new UserRepository();
            IBusRepository busRepository = new BusRepository();
            IScheduleRepository scheduleRepository = new ScheduleRepository();
            IInvoiceRepository invoiceRepository = new InvoiceRepository();
            
       

            MenuController uiCoordinator = new MenuController(userRepository, busRepository, scheduleRepository , invoiceRepository);

            uiCoordinator.RunMainMenu();
        }
    }
}