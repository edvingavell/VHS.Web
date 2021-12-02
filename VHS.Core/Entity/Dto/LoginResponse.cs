using System;

namespace VHS.Core.Entity.Dto
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid CustomerId { get; set; }
    }
}
