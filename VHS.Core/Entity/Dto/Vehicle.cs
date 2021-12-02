using System;

namespace VHS.Core.Entity.Dto
{
    public class Vehicle
    {
        public int OwnerStatus { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Vin { get; set; }
        public string RegNo { get; set; }
    }
}
