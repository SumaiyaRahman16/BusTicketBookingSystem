using System;
using System.Collections.Generic;
using System.Linq;
using BusTicketBookingSystem.Models;
using BusTicketBookingSystem.Models.Bus;

namespace BusTicketBookingSystem.Services
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly List<Schedule> _schedules = new List<Schedule>();
        private int _idCounter = 1;

        public Schedule AddSchedule(Bus bus, string source, string destination, DateTime departureTime, decimal baseFare)
        {

            string generatedId = $"SchID-{_idCounter++}";

            Schedule newSchedule = new Schedule(bus, source, destination, departureTime, baseFare);
            
      
            typeof(Schedule).GetProperty("ScheduleId")?.SetValue(newSchedule, generatedId);

            _schedules.Add(newSchedule);
            return newSchedule;
        }

        public Schedule GetScheduleById(string scheduleId)
        {
            return _schedules.FirstOrDefault(s => s.ScheduleId.Equals(scheduleId, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Schedule> GetAllSchedules()
        {
            return _schedules;
        }
    }
}