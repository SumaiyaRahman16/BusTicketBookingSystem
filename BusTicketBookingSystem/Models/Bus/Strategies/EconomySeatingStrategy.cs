namespace BusTicketBookingSystem.Models.Bus.Strategies;

public class EconomySeatingStrategy
{
    public string ClassificationName => "Economy";

    public int GetCapacity()
    {
        return 40; 
    }
}