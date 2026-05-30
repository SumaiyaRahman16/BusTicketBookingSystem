using System;
using System.Collections.Generic;
using System.Linq;
using BusTicketBookingSystem.Models;

namespace BusTicketBookingSystem.Services
{
    public class UserRepository : IUserRepository
    {
    
        private readonly List<User> _users = new List<User>();
        
        private int _idCounter = 1;

        public User AddUser(string fullName, string mobileNumber, string emailAddress)
        {

            if (GetUserByEmail(emailAddress) != null)
                throw new InvalidOperationException("A user with this email address already exists.");


            string generatedId = $"USR-{_idCounter++}";


            User newUser = new User(generatedId, fullName, mobileNumber, emailAddress);
            
            _users.Add(newUser);
            
            return newUser;
        }

        public User GetUserById(string userId)
        {
            return _users.FirstOrDefault(u => u.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));
        }

        public User GetUserByEmail(string emailAddress)
        {
            return _users.FirstOrDefault(u => u.EmailAddress.Equals(emailAddress, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }
    }
}