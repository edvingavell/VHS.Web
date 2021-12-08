using System;

namespace VHS.Core.Entity
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Destination { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}