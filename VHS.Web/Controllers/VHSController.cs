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
            int batteryStatus, double tripMeter, int engineRunning, int lockStatus, int alarmStatus, double tireLF, double tireLB, double tireRF, double tireRB)
        {
            bool correctTirePressures = Misc.CheckTirePressures(tireLF, tireLB, tireRF, tireRB);
            if (correctTirePressures)
            {
                var tirePressures = new List<double>() { tireLF,
                    tireLB, tireRF, tireRB };
                Guid resultId = vehiclesRepository.PostStatus(regNumber, batteryStatus, tripMeter, engineRunning, lockStatus, alarmStatus, 
                    tirePressures, positionLatitude, positionLongitude);
                return new OkObjectResult(resultId);
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
            //Bilen postar upp sin status
            Guid resultId = vehiclesRepository.PostAlarm(regNumber, positionLatitude, positionLongitude);
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

            //För att kunna se fordonets position och söka efter resmål samt kunna
            //skicka adresser till bilen för att sedan få möjlighet att använda denna
            //data för bilens GPS/ Navigeringssystem

        }

        [HttpGet]
        [Route("/Address")]
        public ActionResult<Address> GetAddress(string regNumber, bool lastDestinationOnly = true)
        {
            IList<Address> list = new List<Address>();
                list = vehiclesRepository.GetAddress(regNumber, lastDestinationOnly);
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
        [Route("/Address")]
        public ActionResult<string> PostAddress(string regNumber, string destination)
        {
            Guid resultId = vehiclesRepository.PostAddress(regNumber, destination);
            if (resultId != Guid.Empty)
            {
                return new OkObjectResult(resultId);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpGet]
        [Route("/DrivingJournal")]
        public ActionResult<DrivingJournal> GetDrivingJournal(string regNumber, DateTime searchDate, DateTime startTime, DateTime endTime)
        {
            IList<DrivingJournal> list = new List<DrivingJournal>();
            if (searchDate.Year == 1 && startTime.Year == 1 && endTime.Year == 1)
            {
                list = vehiclesRepository.GetDrivingJournal(regNumber);
            }
            else if (searchDate.Year != 1)
            {
                list = vehiclesRepository.GetDrivingJournal(regNumber, searchDate);
            }
            else
            {
                list = vehiclesRepository.GetDrivingJournal(regNumber, startTime, endTime);
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

        [HttpPost]
        [Route("/DrivingJournal")]
        public ActionResult<Guid> PostDrivingJournal(string regNumber, string startTime, string stopTime, 
            double distanceInKilometers, double energyConsumptionInkWh, string typeOfTravel)
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
            Guid drivingJournalId = vehiclesRepository.PostDrivingJournal(regNumber, startTime1, stopTime1, distanceInKilometers, 
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
