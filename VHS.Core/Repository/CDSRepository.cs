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
            var accessToken = string.Empty;

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
    }
}
