using System;

namespace VHS.Core.Entity
{
    public class VehiclePosition
    {
        public Guid? StatusId { get; set; }
        public string RegistrationNumber { get; set; }
        public double? PositionLatitude { get; set; }
        public double? PositionLongitude { get; set; }
        public DateTime DateOfCreation{ get; set; }
        public DateTime DateLastModified { get; set; }
        public double? PositionRadius { get; set; }
    }
}
