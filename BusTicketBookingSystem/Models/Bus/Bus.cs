using BusTicketBookingSystem.Models.Bus.Strategies;

namespace BusTicketBookingSystem.Models.Bus;
    

public class Bus
{   
        
    public string BusId { get; private set; }
    public string CoachNumber { get; private set; }
    public string BusClass { get; private set; }
    public int TotalSeats { get; private set; }

    public Bus(string coachNumber, ISeatingStrategy seatingStrategy)
    {
        if (string.IsNullOrWhiteSpace(coachNumber))
            throw new ArgumentException("Coach number cannot be empty.");
        if (seatingStrategy == null)
            throw new ArgumentNullException(nameof(seatingStrategy), "Seating strategy must be provided.");

    
        BusId = "BUS-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            
        CoachNumber = coachNumber;

        BusClass = seatingStrategy.ClassificationName;
        TotalSeats = seatingStrategy.GetCapacity();
    }


    public override string ToString()
    {
        return $"{CoachNumber} | {BusClass} | Total Seats: {TotalSeats} (ID: {BusId})";
    }
}