using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VHS.Core;
using VHS.Core.Repository;

namespace VHS.Web.Filters
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private readonly CDSRepository cdsRepository;

        public ClaimRequirementFilter()
        {
            cdsRepository = new CDSRepository();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = Identity.CdsToken;
            var userId = Identity.CdsUserId;
            var hasClaim = cdsRepository.Validate(userId, token);
            if (!hasClaim)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
