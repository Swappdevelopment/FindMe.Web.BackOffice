using Microsoft.AspNetCore.Authorization;

namespace FindMe.Web.App
{
    public class SingedInAttribute : AuthorizeAttribute
    {
        public SingedInAttribute(string policy = "SignedIn")
            : base(policy)
        {
        }
    }
}
