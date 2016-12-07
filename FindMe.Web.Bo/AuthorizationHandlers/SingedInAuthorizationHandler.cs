using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class SingedInAuthorizationHandler : AuthorizationHandler<SingedInRequirement>, IAuthorizationRequirement
    {
        public override Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (context.HasSucceeded)
            {
                
            }

            return base.HandleAsync(context);
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SingedInRequirement requirement)
        {
            if(true)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
