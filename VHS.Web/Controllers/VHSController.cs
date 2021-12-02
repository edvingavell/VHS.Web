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

namespace VHS.Web.Controllers
{
    [Route("api/VHS")]
    [ApiController]
    public class VHSController : ControllerBase
    {
        #region Private
        private readonly VehiclesRepository vehiclesRepository;
        private readonly CDSRepository cdsRepository;
        #endregion

        #region Public

        public VHSController()
        {
            vehiclesRepository = new VehiclesRepository();
            cdsRepository = new CDSRepository();
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
        public ActionResult<Guid> PostStatus(string regNumber, int batteryStatus, string carGPS, double tripMeter, int lockStatus, int alarmStatus, string tireLF, string tireLB, string tireRF, string tireRB)
        {
            bool correctTirePressures = Misc.CheckTirePressures(tireLF, tireLB, tireRF, tireRB);
            if (correctTirePressures)
            {
                var tirePressures = new List<double>() { Double.Parse(tireLF),
                    Double.Parse(tireLB), Double.Parse(tireRF), Double.Parse(tireRB) };

                SqlConnection myConnection = new SqlConnection(ExpressDb.ConnectionString);

                myConnection.Open();

                SqlCommand cmd = new SqlCommand("dbo.sPost_Status", myConnection) { CommandType = System.Data.CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = regNumber });
                cmd.Parameters.Add(new SqlParameter("@BatteryStatus", SqlDbType.Int) { Value = batteryStatus });
                cmd.Parameters.Add(new SqlParameter("@GPS", SqlDbType.NVarChar, 255) { Value = carGPS });
                cmd.Parameters.Add(new SqlParameter("@TripMeter", SqlDbType.Float) { Value = tripMeter });
                cmd.Parameters.Add(new SqlParameter("@LockStatus", SqlDbType.Int) { Value = lockStatus });
                cmd.Parameters.Add(new SqlParameter("@AlarmStatus", SqlDbType.Int) { Value = alarmStatus });
                cmd.Parameters.Add(new SqlParameter("@TirePressures", SqlDbType.NVarChar, 50) { 
                    Value = JsonConvert.SerializeObject(tirePressures) });
                cmd.Parameters.Add(new SqlParameter("@StatusId", SqlDbType.UniqueIdentifier)
                { Value = null, Direction = ParameterDirection.Output });

                cmd.ExecuteNonQuery();

                var userId = new Guid(cmd.Parameters[7].Value.ToString());

                myConnection.Close();

                return new OkObjectResult(userId);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpPost]
        [Route("/PostAlarm")]
        public ActionResult<string> PostAlarm(string regNumber, string gps)
        {
            // integration med CDS, skicka regNr till CDS och få tillbaka telefon nr som tillhör bilen.
            // skicka sms till bilen

            return null;
        }

        [HttpPost]
        [Route("/GetPosition")]
        public ActionResult<string> PostPosition(string regNumber)
        {
            //skickar sms till bilen
            //bilen postar upp sin status (position)
            var list = vehiclesRepository.GetStatus(regNumber);
            var gps = String.Empty;
            if (list.Count > 0)
            {
                gps = list[0].GPS;
            }
            if (!String.IsNullOrEmpty(gps))
            {
                return new OkObjectResult(gps);
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
        public ActionResult<LoginResponse> GetAccessToken(string userName = "edgave", string password = "IoT20!!!")
        {
            var loginResponse = cdsRepository.GetAccessToken(userName, password);

            if (loginResponse != null)
            {
                Identity.CdsToken = loginResponse.AccessToken;
                return new OkObjectResult(loginResponse);
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [HttpGet]
        [Route("/GetCustomer")]
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
        [Route("/Validate/")]
        public bool validate(Guid userId, string accessToken)
        {
            return cdsRepository.Validate(userId, accessToken);
        }
        #endregion
    }
}
