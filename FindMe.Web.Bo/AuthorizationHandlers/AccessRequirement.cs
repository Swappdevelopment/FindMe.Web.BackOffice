using Microsoft.AspNetCore.Authorization;

namespace FindMe.Web.App
{
    public class AccessRequirement : IAuthorizationRequirement
    {
        public AccessRequirement(AccessLevel accessLevel)
        {
            this.AccessLevel = accessLevel;
        }

        public AccessLevel AccessLevel { get; private set; }
    }
}
