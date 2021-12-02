using HiQ.NetStandard.Util.Data;
using Newtonsoft.Json;
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
                    GPS = dr.GetString(3),
                    TripMeter = (float)dr.GetDouble(4),
                    LockStatus = dr.GetInt32(5),
                    AlarmStatus = dr.GetInt32(6),
                    DateOfCreation = dr.GetDateTime(7),
                    DateLastModified = dr.GetDateTime(8),
                    TirePressures = JsonConvert.DeserializeObject<List<double>>(dr.GetString(9))
                });
            }

            DbAccess.DisposeReader(ref dr);

            return userList;
        }
    }
}
