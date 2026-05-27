using System;

namespace BusTicketBookingSystem.Models
{
    public class Ticket
    {
        public string TicketId { get; private set; }
        public Schedule SelectedSchedule { get; private set; }
        public User Passenger { get; private set; }
        public int SeatNumber { get; private set; }

        public Ticket(Schedule schedule, User passenger, int seatNumber)
        {
            SelectedSchedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
            Passenger = passenger ?? throw new ArgumentNullException(nameof(passenger));
            
         
            if (!schedule.IsSeatAvailable(seatNumber))
                throw new InvalidOperationException($"Seat {seatNumber} is not available on this schedule.");

            TicketId = "TCK-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            SeatNumber = seatNumber;


            schedule.ReserveSeat(seatNumber);
        }

        public override string ToString()
        {
            return $"Ticket: {TicketId} | Passenger: {Passenger.FullName} | Route: {SelectedSchedule.Source} -> {SelectedSchedule.Destination} | Seat: {SeatNumber}";
        }
    }
}