using Microsoft.AspNetCore.Mvc;
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
