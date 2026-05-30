using System;
using System.Collections.Generic;
using System.Linq;
using BusTicketBookingSystem.Models.Bus;

namespace BusTicketBookingSystem.Services
{
    public class BusRepository : IBusRepository
    {
        private readonly List<Bus> _buses = new List<Bus>();

        public void AddBus(Bus bus)
        {
            if (bus == null) 
                throw new ArgumentNullException(nameof(bus));
                
            if (GetBusByCoachNumber(bus.CoachNumber) != null)
                throw new InvalidOperationException($"Coach number {bus.CoachNumber} is already registered in the system.");

            _buses.Add(bus);
        }

        public Bus GetBusByCoachNumber(string coachNumber)
        {
            return _buses.FirstOrDefault(b => b.CoachNumber.Equals(coachNumber, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return _buses;
        }
    }
}