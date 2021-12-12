using Microsoft.AspNetCore.Mvc;
using VHS.Web.Filters;

namespace VHS.Web.Attributes
{
    public class VHSOwnershipAttribute : TypeFilterAttribute
    {
        public VHSOwnershipAttribute() : base(typeof(ClaimOwnershipOfCarFilter))
        {
        }
    }
}
