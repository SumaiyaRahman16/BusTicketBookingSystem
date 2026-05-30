namespace BusTicketBookingSystem.Models.Bus.Strategies
{
    public class EconomySeatingStrategy : ISeatingStrategy
    {

        public string ClassificationName => "Economy";

        public int GetCapacity()
        {
            return 40; 
        }
    }
}