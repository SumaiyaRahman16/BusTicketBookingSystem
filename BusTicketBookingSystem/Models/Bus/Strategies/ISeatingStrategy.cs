namespace BusTicketBookingSystem.Models.Bus.Strategies;

public interface ISeatingStrategy
{
    string ClassificationName { get; }
    int GetCapacity();
}