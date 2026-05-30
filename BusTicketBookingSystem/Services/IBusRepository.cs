using System.Collections.Generic;
using BusTicketBookingSystem.Models.Bus;

namespace BusTicketBookingSystem.Services
{
    public interface IBusRepository
    {
        void AddBus(Bus bus);
        Bus GetBusByCoachNumber(string coachNumber);
        IEnumerable<Bus> GetAllBuses();
    }
}