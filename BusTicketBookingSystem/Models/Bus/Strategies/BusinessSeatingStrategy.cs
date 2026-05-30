namespace BusTicketBookingSystem.Models.Bus.Strategies
{
    public class BusinessSeatingStrategy : ISeatingStrategy
    {
      
        public string ClassificationName => "Business";

        public int GetCapacity()
        {
            return 28; 
        }
    }
}