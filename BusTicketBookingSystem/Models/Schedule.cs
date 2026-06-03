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

            ScheduleId = "SchID" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
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
            return $"{ScheduleId} | {AssignedBus.BusId} | {Source} to {Destination} | Date: {DepartureTime:yyyy-MM-dd} | Time: {DepartureTime:hh:mm tt} | Fare: {BaseFare} Taka | Available Seats: {GetAvailableSeatCount()}/{AssignedBus.TotalSeats}";
        }

public void DisplaySeatingGrid(Services.IInvoiceRepository invoiceRepository)
{
    Console.WriteLine($"\n--- Schedule Details ---");
    Console.WriteLine($"Schedule ID: {ScheduleId}");
    Console.WriteLine($"{AssignedBus.BusId} | Coach Number: {AssignedBus.CoachNumber} | Type: {AssignedBus.BusClass}");
    Console.WriteLine($"From: {Source} To: {Destination}");
    Console.WriteLine($"Departure Time: {DepartureTime:hh:mm} | Taka: {BaseFare}");
    Console.WriteLine($"Total Seats: {AssignedBus.TotalSeats}");
   
    Console.WriteLine("\nSeat Layout ('X' = Not Available, Label = Available):)");
    
    int columnsPerRow = AssignedBus.BusClass.Equals("Business", StringComparison.OrdinalIgnoreCase) ? 3 : 4;
    int totalSeats = AssignedBus.TotalSeats;
    int totalRows = (int)Math.Ceiling((double)totalSeats / columnsPerRow);

    // fetching  all paid invoices for  specific schedule route
    var paidSeatsForThisSchedule = invoiceRepository.GetAllInvoices()
        .Where(inv => inv != null && 
                      inv.Ticket != null && 
                      inv.Ticket.SelectedSchedule.ScheduleId == this.ScheduleId && 
                      inv.IsPaid)
        .Select(inv => inv.Ticket.SeatNumber)
        .ToHashSet(); // HashSet makes lookups incredibly fast O(1)

    int seatCounter = 1;
    for (int row = 1; row <= totalRows; row++)
    {
        string rowText = "";
        for (int col = 0; col < columnsPerRow; col++)
        {
            if (seatCounter > totalSeats) break;

            char seatLetter = (char)('A' + col);
            
            //  CRITICAL CHANGE: The seat shows "X" ONLY if it exists in our paid collection
            bool isPaidAndConfirmed = paidSeatsForThisSchedule.Contains(seatCounter);

            string seatToken = isPaidAndConfirmed ? "X" : $"{row}{seatLetter}";
            rowText += $"[{seatToken,-3}]";

            if (columnsPerRow == 3 && col == 0) rowText += "     "; 
            else if (columnsPerRow == 4 && col == 1) rowText += "     ";
            else rowText += " ";

            seatCounter++;
        }
        Console.WriteLine($"{rowText}Row {row}");
    }
    Console.WriteLine();
}
    }
}