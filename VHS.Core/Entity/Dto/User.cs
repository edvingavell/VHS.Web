using System;

namespace VHS.Core.Entity.Dto
{
    public class User
    {
        public object AccessToken { get; set; }
        public string CustomerId { get; set; }
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
}
