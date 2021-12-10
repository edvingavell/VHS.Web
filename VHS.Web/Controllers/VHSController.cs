using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VHS.Core.Entity;
using Newtonsoft.Json;
using VHS.Core;
using VHS.Core.Repository;
using VHS.Core.Entity.Dto;
using VHS.Web.Attributes;

namespace VHS.Web.Controllers
{
    [VHSAuthorize]
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
            if (cdsRepository.ValidateOwnerOfCar(regNo, Identity.CdsCustomerId))
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
            else
            {
                return new UnauthorizedResult();
            }
        }

        [HttpPost]
        [Route("/Status/{regNo}")]
        public ActionResult<Guid> PostStatus(string regNo, double positionLatitude, double positionLongitude,
            int batteryStatus, double tripMeter, int engineRunning, int lockStatus, int alarmStatus, 
            double tireLF, double tireLB, double tireRF, double tireRB)
        {
            if (cdsRepository.ValidateOwnerOfCar(regNo, Identity.CdsCustomerId))
            {
                bool correctTirePressures = Misc.CheckTirePressures(tireLF, tireLB, tireRF, tireRB);
                if (correctTirePressures)
                {
                    var tirePressures = new List<double>() { tireLF,
                    tireLB, tireRF, tireRB };
                    Guid resultId = vehiclesRepository.PostStatus(regNo, batteryStatus, tripMeter, engineRunning, lockStatus, alarmStatus,
                        tirePressures, positionLatitude, positionLongitude);
                    return new OkObjectResult(resultId);
                }
                else
                {
                    return new NotFoundResult();
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [HttpPost]
        [Route("/Alarm/{regNo}")]
        public ActionResult<Guid> PostAlarm(string regNo, double positionLatitude, double positionLongitude)
        {
            if (cdsRepository.ValidateOwnerOfCar(regNo, Identity.CdsCustomerId))
            {
                //Bilen postar upp sin status
                Guid resultId = vehiclesRepository.PostAlarm(regNo, positionLatitude, positionLongitude);
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
            else
            {
                return new UnauthorizedResult();
            }
        }

        [HttpGet]
        [Route("/Position/{regNo}")]
        public ActionResult<string> GetPosition(string regNo)
        {
            if (cdsRepository.ValidateOwnerOfCar(regNo, Identity.CdsCustomerId))
            {

                //skickar sms till bilen
                //bilen postar upp sin status (position)
                var list = vehiclesRepository.GetStatus(regNo);
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
            else
            {
                return new UnauthorizedResult();
            }

            //För att kunna se fordonets position och söka efter resmål samt kunna
            //skicka adresser till bilen för att sedan få möjlighet att använda denna
            //data för bilens GPS/ Navigeringssystem

        }

        [HttpGet]
        [Route("/Address/{regNo}")]
        public ActionResult<Address> GetAddress(string regNo, bool lastDestinationOnly = true)
        {
            if (cdsRepository.ValidateOwnerOfCar(regNo, Identity.CdsCustomerId))
            {
                IList<Address> list = new List<Address>();
                list = vehiclesRepository.GetAddress(regNo, lastDestinationOnly);
                if (list.Count > 0)
                {
                    return new OkObjectResult(list);
                }
                else
                {
                    return new NotFoundResult();
                }
            }
            else
            {
                return new UnauthorizedResult();

            }
        }

        [HttpPost]
        [Route("/Address/{regNo}")]
        public ActionResult<string> PostAddress(string regNo, string destination)
        {
            if (cdsRepository.ValidateOwnerOfCar(regNo, Identity.CdsCustomerId))
            {
                Guid resultId = vehiclesRepository.PostAddress(regNo, destination);
                if (resultId != Guid.Empty)
                {
                    return new OkObjectResult(resultId);
                }
                else
                {
                    return new NotFoundResult();
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [HttpGet]
        [Route("/DrivingJournal/{regNo}")]
        public ActionResult<DrivingJournal> GetDrivingJournal(string regNo, DateTime searchDate, DateTime startTime, DateTime endTime)
        {
            if (cdsRepository.ValidateOwnerOfCar(regNo, Identity.CdsCustomerId))
            {
                IList<DrivingJournal> list = new List<DrivingJournal>();
                if (searchDate.Year == 1 && startTime.Year == 1 && endTime.Year == 1)
                {
                    list = vehiclesRepository.GetDrivingJournal(regNo);
                }
                else if (searchDate.Year != 1)
                {
                    list = vehiclesRepository.GetDrivingJournal(regNo, searchDate);
                }
                else
                {
                    list = vehiclesRepository.GetDrivingJournal(regNo, startTime, endTime);
                }
                if (list.Count > 0)
                {
                    return new OkObjectResult(list);
                }
                else
                {
                    return new NotFoundResult();
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [HttpPost]
        [Route("/DrivingJournal/{regNo}")]
        public ActionResult<Guid> PostDrivingJournal(string regNo, string startTime, string stopTime,
            double distanceInKilometers, double energyConsumptionInkWh, string typeOfTravel )
        {
            if (cdsRepository.ValidateOwnerOfCar(regNo, Identity.CdsCustomerId))
            {
                DateTime startTime1;
                DateTime stopTime1;
                if (String.IsNullOrEmpty(startTime) || String.IsNullOrEmpty(stopTime))
                {
                    startTime1 = DateTime.Now;
                    stopTime1 = DateTime.Now.AddHours(5);
                }
                else
                {
                    startTime1 = DateTime.Parse(startTime);
                    stopTime1 = DateTime.Parse(stopTime);
                }
                Guid drivingJournalId = vehiclesRepository.PostDrivingJournal(regNo, startTime1, stopTime1, distanceInKilometers,
                    energyConsumptionInkWh, typeOfTravel);
                if (drivingJournalId != Guid.Empty)
                {
                    return new OkObjectResult(drivingJournalId);
                }
                else
                {
                    return new NotFoundResult();
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [HttpPatch]
        [Route("/DrivingJournal/id")]
        public ActionResult<DrivingJournal> PatchedDrivingJournal(Guid id, string typeOfTravel)
        {
           //Behövs ingen kontroll av ägande pga det kontrolleras när man hämtar ut alla drivingjournals.
            IList<DrivingJournal> drivingJournalList = vehiclesRepository.PatchDrivingJournal(id, typeOfTravel);
            if (drivingJournalList.Count > 0)
            {
                return new OkObjectResult(drivingJournalList);
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
#region Private
        private readonly CDSRepository cdsRepository;
#endregion

#region Public
        public CDSController()
        {
            cdsRepository = new CDSRepository();
        }

        [HttpGet]
        [Route("/Auth/{userName}/{pwd}")]
        public ActionResult<LoginResponse> Authenticate(string userName = "edgave", string pwd = "IoT20!!!")
        {
            var loginResponse = cdsRepository.Authenticate(userName, pwd);
            if (loginResponse != null)
            {
                Identity.CdsToken = loginResponse.AccessToken;
                Identity.CdsUserId = loginResponse.Id;
                Identity.CdsCustomerId = loginResponse.CustomerId;

                return new OkObjectResult(loginResponse);
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [HttpGet]
        [Route("/Customer/{customerId}")]
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
        [Route("/Validate/{userId}/{accessToken}")]
        public bool GetValidate(Guid userId, string accessToken)
        {
            return cdsRepository.Validate(userId, accessToken);
        }


        //patch//update på driving journal där man ändrar typeOftravel
        //utveckla search metoden till att gå på typeOfTravel om man vill
        //delete??

#endregion
    }
}
