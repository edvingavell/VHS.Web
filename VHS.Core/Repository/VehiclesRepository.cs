using HiQ.NetStandard.Util.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using VHS.Core.Entity;

namespace VHS.Core.Repository
{
    public class VehiclesRepository
    {
        public IList<Status> GetStatus(string regNo)
        {
            IList<Status> userList = new List<Status>();
            SqlDbAccess DbAccess = new SqlDbAccess(ExpressDb.ConnectionString);

            var parameters = new SqlParameters();
            parameters.AddVarChar("@RegistrationNumber", 50, regNo);

            var dr = DbAccess.ExecuteReader("dbo.sStatus_GetByRegNo", ref parameters, CommandType.StoredProcedure);

            while (dr.Read())
            {
                userList.Add(new Status()
                {
                    StatusId = dr.GetGuid(0),
                    RegistrationNumber = regNo,
                    BatteryStatus = dr.GetInt32(2),
                    TripMeter = (float)dr.GetDouble(3),
                    LockStatus = dr.GetInt32(4),
                    AlarmStatus = dr.GetInt32(5),
                    DateOfCreation = dr.GetDateTime(6),
                    DateLastModified = dr.GetDateTime(7),
                    TirePressures = JsonConvert.DeserializeObject<List<double>>(dr.GetString(8)),
                    PositionLatitude = !dr.IsDBNull(9) ? dr.GetDouble(9) : null,
                    PositionLongitude = !dr.IsDBNull(10) ? dr.GetDouble(10) : null,
                    PositionRadius = !dr.IsDBNull(11) ? dr.GetDouble(11) : null
                });
            }

            DbAccess.DisposeReader(ref dr);

            return userList;
        }

        public IList<DrivingJournal> GetDrivingJournal(string regNumber)
        {
            IList<DrivingJournal> drivingJournalList = new List<DrivingJournal>();
            SqlDbAccess DbAccess = new SqlDbAccess(ExpressDb.ConnectionString);

            var parameters = new SqlParameters();
            parameters.AddVarChar("@RegistrationNumber", 50, regNumber);

            var dr = DbAccess.ExecuteReader("dbo.sDrivingJournal_GetByRegNo", ref parameters, CommandType.StoredProcedure);

            while (dr.Read())
            {
                drivingJournalList.Add(new DrivingJournal()
                {
                    DrivingJournalId = dr.GetGuid(0),
                    RegistrationNumber = dr.GetString(1),
                    StartTime = dr.GetDateTime(2),
                    StopTime = dr.GetDateTime(3),
                    DistanceInKm = dr.GetDouble(4),
                    EnergyConsumptionInkWh = dr.GetDouble(5),
                    AverageConsumptionInkWhPer100km = dr.GetDouble(6),
                    AverageSpeedInKmPerHour = dr.GetDouble(7),
                    TypeOfTravel = dr.GetString(8),
                    DateOfCreation = dr.GetDateTime(9),
                    DateLastModified = dr.GetDateTime(10)
                });
            }

            DbAccess.DisposeReader(ref dr);

            return drivingJournalList;
        }
    }
}
