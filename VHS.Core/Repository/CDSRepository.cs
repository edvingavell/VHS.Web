using Newtonsoft.Json;
using System;
using RestSharp;
using System.Web;
using VHS.Core.Entity.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace VHS.Core.Repository
{
    public class CDSRepository
    {
        private readonly IRestClient _restClient;

        public CDSRepository()
        {
            _restClient = new RestClient("https://kyhdev.hiqcloud.net");
        }

        public LoginResponse Authenticate(string userName, string password)
        {
            var request = new RestRequest($"api/cds/v1.0/user/authenticate?userName=" +
                    $"{HttpUtility.UrlEncode(userName)}&pwd={HttpUtility.UrlEncode(password)}", Method.GET);
            var response = Execute<LoginResponse>(request, false);
            return response;
        }

        public Customer GetCustomer(Guid customerId)
        {
            var request = new RestRequest($"cds/v1.0/customer/{customerId}", Method.GET);
            var response = Execute<Customer>(request);
            return response;
        }

        public bool ValidateOwnerOfCar(Guid customerId, string regNo)
        {
            var customer = GetCustomer(customerId);
            foreach (Vehicle x in customer.Vehicles)
            {
                if (x.RegNo == regNo.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        public IList<Vehicle> GetYourCars(Guid customerId)
        {
            var customer = GetCustomer(customerId);
            List<Vehicle> list = new List<Vehicle>();
            if (customer.Vehicles != null)
            {
                foreach (Vehicle x in customer.Vehicles)
                {
                    list.Add(x);
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public bool Validate(Guid userId, string accessToken)
        {
            var request = new RestRequest($"api/cds/v1.0/user/{ userId }/validate?token={ HttpUtility.UrlEncode(accessToken)}");
            return Execute<bool>(request);
        }

        private T Execute<T>(IRestRequest request, bool addToken=true)
        {
            if (addToken)
            {
                request.AddHeader("kyh-auth", Identity.CdsToken);
            }
            var response = _restClient.Execute(request);
            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<T>(response.Content);
                return (T)(object)result;
            }
            return default(T);
        }

        public IList<FullVehicle> GetCarsWithOutOwners()
        {
            IList<FullVehicle> cars = new List<FullVehicle>();
            var request = new RestRequest($"api/cds/v1.0/vehicle/list", Method.GET);
            var result = Execute<IList<FullVehicle>>(request);
            if (result != null)
            {
                foreach(var vehicle in result)
                {
                    if (vehicle.Owner == null || vehicle.Owner.OwnerStatus == 0)
                    {
                        cars.Add(vehicle);
                    }
                }
            }
            return cars;
        }

        public FullVehicle PostOwnershipOfCar(string vin)
        {
            var request = new RestRequest($"api/cds/v1.0/vehicle/owner/{HttpUtility.UrlEncode(vin)}/{HttpUtility.UrlEncode(Identity.CdsCustomerId.ToString())}", Method.POST);
            var response = Execute<FullVehicle>(request);
            return response;
        }
    }
}
