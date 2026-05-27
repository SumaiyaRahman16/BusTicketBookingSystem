namespace BusTicketBookingSystem.Models.Bus.Strategies;

public class BusinessSeatingStrategy
{
    public string ClassificationName => "Business";

    public int GetCapacity()
    {
        return 28; 
    }
}