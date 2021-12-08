using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using VHS.Core.Entity;
using Newtonsoft.Json;
using VHS.Core;
using VHS.Core.Repository;
using VHS.Core.Entity.Dto;
using VHS.Web.Attributes;
using System.Globalization;

namespace VHS.Web.Controllers
{
    [VHSAuthorize]
    [Route("api/VHS")]
    [ApiController]
    public class VHSController : ControllerBase
    {
        #region Private
        private readonly VehiclesRepository vehiclesRepository;
        #endregion

        #region Public
        public VHSController()
        {
            vehiclesRepository = new VehiclesRepository();
        }

        [HttpGet]
        [Route("/Status/{regNo}")]
        public ActionResult<IList<Status>> GetStatus(string regNo)
        {
            var userList = vehiclesRepository.GetStatus(regNo);
            if (userList.Count != 0)
            {
                return new OkObjectResult(userList);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpPost]
        [Route("/Status")]
        public ActionResult<Guid> PostStatus(string regNumber, double positionLatitude, double positionLongitude, 
            int batteryStatus, double tripMeter, int lockStatus, int alarmStatus, double tireLF, double tireLB, double tireRF, double tireRB)
        {
            bool correctTirePressures = Misc.CheckTirePressures(tireLF, tireLB, tireRF, tireRB);
            if (correctTirePressures)
            {
                var tirePressures = new List<double>() { tireLF,
                    tireLB, tireRF, tireRB };

                SqlConnection myConnection = new SqlConnection(ExpressDb.ConnectionString);

                myConnection.Open();

                SqlCommand cmd = new SqlCommand("dbo.sStatus_Post", myConnection) { CommandType = System.Data.CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@StatusId", SqlDbType.UniqueIdentifier)
                    { Value = null, Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNumber });
                cmd.Parameters.Add(new SqlParameter("@BatteryStatus", SqlDbType.Int) { Value = batteryStatus });
                cmd.Parameters.Add(new SqlParameter("@TripMeter", SqlDbType.Float) { Value = tripMeter });
                cmd.Parameters.Add(new SqlParameter("@LockStatus", SqlDbType.Int) { Value = lockStatus });
                cmd.Parameters.Add(new SqlParameter("@AlarmStatus", SqlDbType.Int) { Value = alarmStatus });
                cmd.Parameters.Add(new SqlParameter("@TirePressures", SqlDbType.NVarChar, 50) { 
                    Value = JsonConvert.SerializeObject(tirePressures) });
                
                cmd.ExecuteNonQuery();

                var userId = new Guid(cmd.Parameters[0].Value.ToString());

                SqlCommand cmd2 = new SqlCommand("dbo.sVehiclePosition_Post", myConnection) { CommandType = System.Data.CommandType.StoredProcedure };
                cmd2.Parameters.Add(new SqlParameter("@VehiclePositionId", SqlDbType.UniqueIdentifier)
                { Value = userId});
                cmd2.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNumber });
                cmd2.Parameters.Add(new SqlParameter("@PositionLatitude", SqlDbType.Float) { Value = positionLatitude });
                cmd2.Parameters.Add(new SqlParameter("@PositionLongitude", SqlDbType.Float) { Value = positionLongitude });
                cmd2.Parameters.Add(new SqlParameter("@PositionRadius", SqlDbType.Float) { Value = 200 });

                cmd2.ExecuteNonQuery();

                myConnection.Close();

                return new OkObjectResult(userId);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpPost]
        [Route("/Alarm")]
        public ActionResult<Guid> PostAlarm(string regNumber = "KKK111", double positionLatitude= 57.708870, double positionLongitude= 11.974560)
        {
            SqlConnection myConnection = new SqlConnection(ExpressDb.ConnectionString);

            myConnection.Open();

            SqlCommand cmd = new SqlCommand("dbo.sAlarm_Post", myConnection) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@AlarmId", SqlDbType.UniqueIdentifier)
            { Value = null, Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNumber });
            cmd.Parameters.Add(new SqlParameter("@PositionLatitude", SqlDbType.Float) { Value = positionLatitude });
            cmd.Parameters.Add(new SqlParameter("@PositionLongitude", SqlDbType.Float) { Value = positionLongitude });
            cmd.ExecuteNonQuery();

            var alarmId = new Guid(cmd.Parameters[0].Value.ToString());

            SqlCommand cmd2 = new SqlCommand("dbo.sVehiclePosition_GetDistance", myConnection) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd2.Parameters.Add(new SqlParameter("@VehiclePositionId", SqlDbType.UniqueIdentifier)
            { Value = null, Direction = ParameterDirection.Output });
            cmd2.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNumber });
            cmd2.Parameters.Add(new SqlParameter("@PositionLatitude", SqlDbType.Float) { Value = positionLatitude });
            cmd2.Parameters.Add(new SqlParameter("@PositionLongitude", SqlDbType.Float) { Value = positionLongitude });
            cmd2.ExecuteNonQuery();

            Guid resultId = new Guid();
            if (cmd2.Parameters.Count > 0 && !String.IsNullOrEmpty(cmd2.Parameters[0].Value.ToString())) {
                resultId = new Guid(cmd2.Parameters[0].Value.ToString());
            }
            myConnection.Close();

            if (resultId != Guid.Empty)
            {
                return new OkObjectResult(resultId);
            }
            else
            {
                return new NotFoundResult();
            }

            // integration med CDS, skicka regNr till CDS och få tillbaka telefon nr som tillhör bilen. -- FINNS EJ I DAGSLÄGET
            // skicka sms till bilen
            
        }

        [HttpGet]
        [Route("/Position")]
        public ActionResult<string> GetPosition(string regNumber)
        {
            //skickar sms till bilen
            //bilen postar upp sin status (position)
            var list = vehiclesRepository.GetStatus(regNumber);
            if (list.Count > 0 && list[0].PositionLatitude != null && list[0].PositionLongitude != null)
            {
                var position = new List<double>() { list[0].PositionLatitude.Value,
                   list[0].PositionLongitude.Value };
                return new OkObjectResult(JsonConvert.SerializeObject(position));
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpGet]
        [Route("/DrivingJournal")]
        public ActionResult<DrivingJournal> GetDrivingJournal(string regNumber)
        {
            var list = vehiclesRepository.GetDrivingJournal(regNumber);
            if (list.Count > 0)
            {
                return new OkObjectResult(list);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpPost]
        [Route("/DrivingJournal")]
        public ActionResult<Guid> PostDrivingJournal(string regNumber, string startTime, string stopTime, 
            double distanceInKilometers, double energyConsumptionInkWh, double averageConsumptionInkWhPer100km, string typeOfTravel)
        {
            DateTime startTime1;
            DateTime stopTime1;
            if (String.IsNullOrEmpty(startTime) || String.IsNullOrEmpty(stopTime))
            {
                startTime1 = DateTime.Now;
                stopTime1 = DateTime.Now;
            }
            else
            {
                startTime1 = DateTime.Parse(startTime);
                stopTime1 = DateTime.Parse(stopTime);
            }
            double timeInHours = (stopTime1 - startTime1).TotalHours;
            double averageSpeedInKilometersPerHour = distanceInKilometers / timeInHours;
            SqlConnection myConnection = new SqlConnection(ExpressDb.ConnectionString);
            myConnection.Open();
            SqlCommand cmd = new SqlCommand("dbo.sDrivingJournal_Post", myConnection) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@DrivingJournalId", SqlDbType.UniqueIdentifier)
            { Value = null, Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNumber });
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

            if (drivingJournalId != Guid.Empty)
            {
                return new OkObjectResult(drivingJournalId);
            }
            else
            {
                return new NotFoundResult();
            }
        }
        #endregion
    }

    [Route("api/CDS")]
    public class CDSController : ControllerBase
    {
        private readonly CDSRepository cdsRepository;

        public CDSController()
        {
            cdsRepository = new CDSRepository();
        }

        #region Public
        [HttpGet]
        [Route("/Auth")]
        public ActionResult<LoginResponse> Authenticate(string userName = "edgave", string password = "IoT20!!!")
        {
            var loginResponse = cdsRepository.Authenticate(userName, password);

            if (loginResponse != null)
            {
                Identity.CdsToken = loginResponse.AccessToken;
                Identity.CdsUserId = loginResponse.Id;
                return new OkObjectResult(loginResponse);
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [HttpGet]
        [Route("/Customer")]
        public ActionResult<Customer> GetCustomer(Guid customerId)
        {
            var customer = cdsRepository.GetCustomer(customerId);
            if (customer != null)
            {
                return new OkObjectResult(customer);
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [HttpGet]
        [Route("/Validate")]
        public bool GetValidate(Guid userId, string accessToken)
        {
            return cdsRepository.Validate(userId, accessToken);
        }
        #endregion
    }
}
