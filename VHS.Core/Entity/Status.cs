using System;
using System.Collections.Generic;

namespace VHS.Core.Entity
{
    public class Status
    {
        public Guid StatusId { get; set; }
        public string RegistrationNumber { get; set; }
        public int BatteryStatus { get; set; }
        public string GPS { get; set; }
        public float TripMeter { get; set; }
        public int LockStatus { get; set; }
        public int AlarmStatus { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateLastModified { get; set; }
        public List<double> TirePressures { get; set; }
    }
}
