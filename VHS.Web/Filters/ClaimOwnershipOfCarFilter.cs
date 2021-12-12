using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VHS.Core;
using VHS.Core.Repository;

namespace VHS.Web.Filters
{
    public class ClaimOwnershipOfCarFilter : IActionFilter
    {
        private readonly CDSRepository cdsRepository;

        public ClaimOwnershipOfCarFilter()
        {
            cdsRepository = new CDSRepository();
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            var customerId = Identity.CdsCustomerId;
            string regNo = context.ActionArguments["regNo"].ToString();
            var hasOwnership = cdsRepository.ValidateOwnerOfCar(customerId, regNo);
            if (!hasOwnership)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
