using AKP_TrackManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using IAuthorizationFilter = System.Web.Http.Filters.IAuthorizationFilter;

namespace AKP_TrackManager.Security
{
    public class AkpCustomAuthorizeAttribute : TypeFilterAttribute
    {
        public AkpCustomAuthorizeAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public bool AllowMultiple => throw new NotImplementedException();

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            throw new NotImplementedException();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
