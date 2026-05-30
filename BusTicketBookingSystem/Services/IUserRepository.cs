using System.Collections.Generic;
using BusTicketBookingSystem.Models;

namespace BusTicketBookingSystem.Services
{
    public interface IUserRepository
    {
        User AddUser(string fullName, string mobileNumber, string emailAddress);
        User GetUserById(string userId);
        User GetUserByEmail(string emailAddress);
        IEnumerable<User> GetAllUsers();
    }
}