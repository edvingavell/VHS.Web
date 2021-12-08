using Microsoft.AspNetCore.Mvc;
using System;
using VHS.Web.Filters;

namespace VHS.Web.Attributes
{
    public class VHSAuthorizeAttribute : TypeFilterAttribute
    {
        public VHSAuthorizeAttribute() : base(typeof(ClaimRequirementFilter))
        {

        }
    }
}
