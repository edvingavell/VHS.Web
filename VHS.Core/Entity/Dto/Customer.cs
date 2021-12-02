using System.Collections.Generic;
using System;

namespace VHS.Core.Entity.Dto
{
    public class Customer
    {
        public List<Vehicle> Vehicles { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public User User { get; set; }
    }
}
