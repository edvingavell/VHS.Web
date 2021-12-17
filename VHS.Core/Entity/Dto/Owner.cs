using System;

namespace VHS.Core.Entity.Dto
{
    public class Owner
    {
        public int OwnerStatus { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string? User { get; set; }
    }
}