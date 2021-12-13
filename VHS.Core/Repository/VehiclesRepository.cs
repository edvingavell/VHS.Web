using HiQ.NetStandard.Util.Data;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using VHS.Core.Entity;

namespace VHS.Core.Repository
{
    public class VehiclesRepository
    {
        private static bool ValidateRegNo(string regNo)
        {
            var result = true;
            foreach (char c in regNo)
            {
                if (!Char.IsLetterOrDigit(c) || Char.IsLower(c))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private static bool ValidateIntAsBool(int x)
        {
            var result = false;
            if (x == 1 || x == 0)
            {
                result = true;
            }
            return result;
        }

        public IList<Status> GetStatus(string regNo)
        {
            if (ValidateRegNo(regNo)) { 
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
                        EngineRunning = dr.GetInt32(4),
                        LockStatus = dr.GetInt32(5),
                        AlarmStatus = dr.GetInt32(6),
                        DateOfCreation = dr.GetDateTime(7),
                        DateLastModified = dr.GetDateTime(8),
                        TirePressures = JsonConvert.DeserializeObject<List<double>>(dr.GetString(9)),
                        PositionLatitude = !dr.IsDBNull(9) ? dr.GetDouble(10) : null,
                        PositionLongitude = !dr.IsDBNull(10) ? dr.GetDouble(11) : null,
                        PositionRadius = !dr.IsDBNull(11) ? dr.GetDouble(12) : null
                    });
                }
                DbAccess.DisposeReader(ref dr);
                return userList;
            }
            else
            {
                return null;
            }
        }

        public IList<Status> PostStatus(string regNo, int batteryStatus, double tripMeter, int engineRunning, 
            int lockStatus, int alarmStatus, List<double> tirePressures, double positionLatitude, double positionLongitude)
        {
            if (ValidateRegNo(regNo) && ValidateIntAsBool(batteryStatus) && ValidateIntAsBool(engineRunning) 
                && ValidateIntAsBool(lockStatus) && ValidateIntAsBool(alarmStatus))
            {
                SqlConnection myConnection = new SqlConnection(ExpressDb.ConnectionString);
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("dbo.sStatus_Post", myConnection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@StatusId", SqlDbType.UniqueIdentifier)
                { Value = null, Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNo });
                cmd.Parameters.Add(new SqlParameter("@BatteryStatus", SqlDbType.Int) { Value = batteryStatus });
                cmd.Parameters.Add(new SqlParameter("@TripMeter", SqlDbType.Float) { Value = tripMeter });
                cmd.Parameters.Add(new SqlParameter("@EngineRunning", SqlDbType.Int) { Value = engineRunning });
                cmd.Parameters.Add(new SqlParameter("@LockStatus", SqlDbType.Int) { Value = lockStatus });
                cmd.Parameters.Add(new SqlParameter("@AlarmStatus", SqlDbType.Int) { Value = alarmStatus });
                cmd.Parameters.Add(new SqlParameter("@TirePressures", SqlDbType.NVarChar, 50)
                {
                    Value = JsonConvert.SerializeObject(tirePressures)
                });
                cmd.ExecuteNonQuery();
                var userId = new Guid(cmd.Parameters[0].Value.ToString());
                SqlCommand cmd2 = new SqlCommand("dbo.sVehiclePosition_Post", myConnection) { CommandType = CommandType.StoredProcedure };
                cmd2.Parameters.Add(new SqlParameter("@VehiclePositionId", SqlDbType.UniqueIdentifier)
                { Value = userId });
                cmd2.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNo });
                cmd2.Parameters.Add(new SqlParameter("@PositionLatitude", SqlDbType.Float) { Value = positionLatitude });
                cmd2.Parameters.Add(new SqlParameter("@PositionLongitude", SqlDbType.Float) { Value = positionLongitude });
                cmd2.Parameters.Add(new SqlParameter("@PositionRadius", SqlDbType.Float) { Value = 200 });
                cmd2.ExecuteNonQuery();
                myConnection.Close();
                return GetStatus(regNo);
            }
            else
            {
                return null;
            }
        }

        public IList<Address> GetAddress(string regNo, bool lastDestinationOnly)
        {
            if (ValidateRegNo(regNo))
            {
                IList<Address> addressList = new List<Address>();
                SqlDbAccess DbAccess = new SqlDbAccess(ExpressDb.ConnectionString);
                var parameters = new SqlParameters();
                parameters.AddVarChar("@RegistrationNumber", 50, regNo);
                SqlDataReader dr;
                if (lastDestinationOnly)
                {
                    dr = DbAccess.ExecuteReader("dbo.sAddress_GetLastByRegNo", ref parameters, CommandType.StoredProcedure);
                }
                else
                {
                    dr = DbAccess.ExecuteReader("dbo.sAddress_GetAllByRegNo", ref parameters, CommandType.StoredProcedure);
                }
                while (dr.Read())
                {
                    addressList.Add(new Address()
                    {
                        AddressId = dr.GetGuid(0),
                        RegistrationNumber = dr.GetString(1),
                        Destination = dr.GetString(2),
                        DateOfCreation = dr.GetDateTime(3)
                    });
                }
                DbAccess.DisposeReader(ref dr);
                return addressList;
            }
            else
            {
                return null;
            }
        }

        public Address PostAddress(string regNo, string destination)
        {
            if (ValidateRegNo(regNo)) 
            {
                SqlConnection myConnection = new SqlConnection(ExpressDb.ConnectionString);
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("dbo.sAddress_Post", myConnection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@AddressId", SqlDbType.UniqueIdentifier)
                { Value = null, Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNo });
                cmd.Parameters.Add(new SqlParameter("@Destination", SqlDbType.NVarChar, 255) { Value = destination });
                cmd.Parameters.Add(new SqlParameter("@DateOfCreation", SqlDbType.DateTime)
                { Value = null, Direction = ParameterDirection.Output });
                cmd.ExecuteNonQuery();
                var addressId = new Guid(cmd.Parameters[0].Value.ToString());
                DateTime addressDateOfCreation = DateTime.Parse(cmd.Parameters[3].Value.ToString());
                myConnection.Close();
                var address = new Address() { AddressId = addressId, RegistrationNumber = regNo, Destination = destination, DateOfCreation = addressDateOfCreation };
                return address;
            }
            else
            {
                return null;
            }
        }

        public IList<DrivingJournal> GetDrivingJournal(string regNo)
        {
            if (ValidateRegNo(regNo))
            {
                IList<DrivingJournal> drivingJournalList = new List<DrivingJournal>();
                SqlDbAccess DbAccess = new SqlDbAccess(ExpressDb.ConnectionString);
                var parameters = new SqlParameters();
                parameters.AddVarChar("@RegistrationNumber", 50, regNo);
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
            else
            {
                return null;
            }
        }

        public Guid? DeleteDrivingJournal(Guid id)
        {
            var list = GetDrivingJournal(id);
            var targetId = new Guid();
            foreach (var item in list)
            {
                targetId = item.DrivingJournalId;
            }
            if (id.Equals(targetId))
            {
                SqlConnection myConnection = new SqlConnection(ExpressDb.ConnectionString);
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("dbo.sDrivingJournal_DeleteByGuid", myConnection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@DrivingJournalId", SqlDbType.UniqueIdentifier) { Value = id });
                cmd.ExecuteNonQuery();
                myConnection.Close();
                return id;
            }
            else
            {
                return null;
            }
        }

        public IList<DrivingJournal> GetDrivingJournal(Guid id)
        {
            IList<DrivingJournal> drivingJournalList = new List<DrivingJournal>();
            SqlDbAccess DbAccess = new SqlDbAccess(ExpressDb.ConnectionString);
            var parameters = new SqlParameters();
            parameters.Add(new SqlParameter("@DrivingJournalId", SqlDbType.UniqueIdentifier)
            { Value = id });
            var dr = DbAccess.ExecuteReader("dbo.sDrivingJournal_GetByGuid", ref parameters, CommandType.StoredProcedure);
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

        public IList<DrivingJournal> GetDrivingJournal(string regNo, DateTime searchDate)
        {
            if (ValidateRegNo(regNo))
            {
                IList<DrivingJournal> drivingJournalList = new List<DrivingJournal>();
                SqlDbAccess DbAccess = new SqlDbAccess(ExpressDb.ConnectionString);
                var parameters = new SqlParameters();
                parameters.AddVarChar("@RegistrationNumber", 50, regNo);
                parameters.AddDateTime("SearchDate", searchDate);
                var dr = DbAccess.ExecuteReader("dbo.sDrivingJournal_GetByRegNoAndDate", ref parameters, CommandType.StoredProcedure);
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
            else
            {
                return null;
            }
        }

        public IList<DrivingJournal> GetDrivingJournal(string regNo, DateTime startDate, DateTime endDate)
        {
            if (ValidateRegNo(regNo))
            {
                var start = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
                var end = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
                IList<DrivingJournal> drivingJournalList = new List<DrivingJournal>();
                SqlDbAccess DbAccess = new SqlDbAccess(ExpressDb.ConnectionString);
                var parameters = new SqlParameters();
                parameters.AddVarChar("@RegistrationNumber", 50, regNo);
                parameters.AddDateTime("@Start", start);
                parameters.AddDateTime("@End", end);
                var dr = DbAccess.ExecuteReader("dbo.sDrivingJournal_GetByRegNoAndBetweenDates", ref parameters, CommandType.StoredProcedure);
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
            else
            {
                return null;
            }
        }

        public IList<DrivingJournal> PostDrivingJournal(string regNo, DateTime startTime1, DateTime stopTime1, 
            double distanceInKilometers, double energyConsumptionInkWh, string typeOfTravel)
        {
            if (ValidateRegNo(regNo))
            {
                double timeInHours = (stopTime1 - startTime1).TotalHours;
                double averageSpeedInKilometersPerHour = distanceInKilometers / timeInHours;
                double averageConsumptionInkWhPer100km = energyConsumptionInkWh / (distanceInKilometers / 100);
                SqlConnection myConnection = new SqlConnection(ExpressDb.ConnectionString);
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("dbo.sDrivingJournal_Post", myConnection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@DrivingJournalId", SqlDbType.UniqueIdentifier)
                { Value = null, Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNo });
                cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime) { Value = startTime1 });
                cmd.Parameters.Add(new SqlParameter("@StopTime", SqlDbType.DateTime) { Value = stopTime1 });
                cmd.Parameters.Add(new SqlParameter("@DistanceInKm", SqlDbType.Float) { Value = distanceInKilometers });
                cmd.Parameters.Add(new SqlParameter("@EnergyConsumptionInkWh", SqlDbType.Float) { Value = energyConsumptionInkWh });
                cmd.Parameters.Add(new SqlParameter("@AverageConsumptionInkWhPer100km", SqlDbType.Float) { Value = averageConsumptionInkWhPer100km });
                cmd.Parameters.Add(new SqlParameter("@AverageSpeedInKmPerHour", SqlDbType.Float) { Value = averageSpeedInKilometersPerHour });
                cmd.Parameters.Add(new SqlParameter("@TypeOfTravel", SqlDbType.NVarChar, 50) { Value = typeOfTravel });
                cmd.ExecuteNonQuery();
                var drivingJournalId = new Guid(cmd.Parameters[0].Value.ToString());
                myConnection.Close();
                return GetDrivingJournal(drivingJournalId);
            }
            else
            {
                return null;
            }
        }

        public IList<DrivingJournal> PatchDrivingJournal(Guid id, string typeOfTravel)
        {
            SqlConnection myConnection = new SqlConnection(ExpressDb.ConnectionString);
            myConnection.Open();
            SqlCommand cmd = new SqlCommand("dbo.sDrivingJournal_PatchTravelTypeByGuid", myConnection) 
                { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@DrivingJournalId", SqlDbType.UniqueIdentifier)
            { Value = id });
            cmd.Parameters.Add(new SqlParameter("@TypeOfTravel", SqlDbType.NVarChar, 50) { Value = typeOfTravel });
            cmd.ExecuteNonQuery();
            myConnection.Close();

            IList<DrivingJournal> drivingJournalList = GetDrivingJournal(id);
            return drivingJournalList;
        }

        public Guid? PostAlarm(string regNo, double positionLatitude, double positionLongitude)
        {
            if (ValidateRegNo(regNo))
            {
                SqlConnection myConnection = new SqlConnection(ExpressDb.ConnectionString);
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("dbo.sAlarm_Post", myConnection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@AlarmId", SqlDbType.UniqueIdentifier)
                { Value = null, Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNo });
                cmd.Parameters.Add(new SqlParameter("@PositionLatitude", SqlDbType.Float) { Value = positionLatitude });
                cmd.Parameters.Add(new SqlParameter("@PositionLongitude", SqlDbType.Float) { Value = positionLongitude });
                cmd.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand("dbo.sVehiclePosition_GetDistance", myConnection) { CommandType = CommandType.StoredProcedure };
                cmd2.Parameters.Add(new SqlParameter("@VehiclePositionId", SqlDbType.UniqueIdentifier)
                { Value = null, Direction = ParameterDirection.Output });
                cmd2.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNo });
                cmd2.Parameters.Add(new SqlParameter("@PositionLatitude", SqlDbType.Float) { Value = positionLatitude });
                cmd2.Parameters.Add(new SqlParameter("@PositionLongitude", SqlDbType.Float) { Value = positionLongitude });
                cmd2.ExecuteNonQuery();
                Guid resultId = new Guid();
                if (cmd2.Parameters.Count > 0 && !String.IsNullOrEmpty(cmd2.Parameters[0].Value.ToString()))
                {
                    resultId = new Guid(cmd2.Parameters[0].Value.ToString());
                }
                myConnection.Close();
                return resultId;
            }
            else
            {
                return null;
            }
        }
    }
}
