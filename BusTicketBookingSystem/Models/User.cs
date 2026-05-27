using System;

namespace BusTicketBookingSystem.Models
{
    public class User
    {
 
        public string UserId { get; private set; } 
        public string FullName { get; private set; }
        public string MobileNumber { get; private set; }
        public string EmailAddress { get; private set; }
     


        public User(string userId ,string fullName, string mobileNumber, string emailAddress)
        {   
            if (string.IsNullOrWhiteSpace(fullName)) 
                throw new ArgumentException("Name cannot be empty");
            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new ArgumentException("You must enter an email address");
            if (string.IsNullOrWhiteSpace(mobileNumber))
                throw new ArgumentException("Mobile number cannot be empty");

    
            UserId = userId;
            
            FullName = fullName;
            MobileNumber = mobileNumber;
            EmailAddress = emailAddress;

        }
        

        public override string ToString()
        {
            return $"{UserId} | {FullName} | {MobileNumber} | {EmailAddress}";
        }
    }
}