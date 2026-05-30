using System;
using System.Collections.Generic;
using BusTicketBookingSystem.Models;
using BusTicketBookingSystem.Models.Bus;

namespace BusTicketBookingSystem.Services
{
    public interface IScheduleRepository
    {
        Schedule AddSchedule(Bus bus, string source, string destination, DateTime departureTime, decimal baseFare);
        Schedule GetScheduleById(string scheduleId);
        IEnumerable<Schedule> GetAllSchedules();
    }
}