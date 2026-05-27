using System;
using System.Collections.Generic;

namespace BusTicketBookingSystem.Models
{
    public class Schedule
    {
        public string ScheduleId { get; private set; }
        public Bus.Bus AssignedBus { get; private set; }
        public string Source { get; private set; }
        public string Destination { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public decimal BaseFare { get; private set; }


        private Dictionary<int, bool> _seatInventory;

        public Schedule(Bus.Bus assignedBus, string source, string destination, DateTime departureTime, decimal baseFare)
        {
            AssignedBus = assignedBus ?? throw new ArgumentNullException(nameof(assignedBus));
            if (string.IsNullOrWhiteSpace(source)) throw new ArgumentException("Source cannot be empty.");
            if (string.IsNullOrWhiteSpace(destination)) throw new ArgumentException("Destination cannot be empty.");
            if (baseFare <= 0) throw new ArgumentException("Fare must be positive.");

            ScheduleId = "SCH-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Source = source;
            Destination = destination;
            DepartureTime = departureTime;
            BaseFare = baseFare;

            InitializeSeats();
        }

        private void InitializeSeats()
        {
            _seatInventory = new Dictionary<int, bool>();
            for (int i = 1; i <= AssignedBus.TotalSeats; i++)
            {
                _seatInventory.Add(i, false); 
            }
        }

        public bool IsSeatAvailable(int seatNumber)
        {
            if (!_seatInventory.ContainsKey(seatNumber)) return false;
            return !_seatInventory[seatNumber]; 
        }

        public void ReserveSeat(int seatNumber)
        {
            if (!IsSeatAvailable(seatNumber))
                throw new InvalidOperationException($"Seat {seatNumber} is already occupied!");

            _seatInventory[seatNumber] = true;
        }

        public int GetAvailableSeatCount()
        {
            int count = 0;
            foreach (var isBooked in _seatInventory.Values)
            {
                if (!isBooked) count++;
            }
            return count;
        }

        public override string ToString()
        {
            return $"{ScheduleId} | {Source} to {Destination} | {DepartureTime:yyyy-MM-dd hh:mm tt} | Available Seats: {GetAvailableSeatCount()}/{AssignedBus.TotalSeats}";
        }
    }
}