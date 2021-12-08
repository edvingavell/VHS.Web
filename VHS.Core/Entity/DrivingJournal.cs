using System;
using System.Collections.Generic;

namespace VHS.Core.Entity
{
    public class DrivingJournal
    {
        public Guid DrivingJournalId { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        public double DistanceInKm { get; set; }
        public double EnergyConsumptionInkWh { get; set; }
        public double AverageConsumptionInkWhPer100km { get; set; }
        public double AverageSpeedInKmPerHour { get; set; }
        public string TypeOfTravel { get; set; }
        public DateTime DateOfCreation { get; set; }      
        public DateTime DateLastModified { get; set; }
    }
}
