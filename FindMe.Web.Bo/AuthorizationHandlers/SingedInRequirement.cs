namespace FindMe.Web.App
{
    public class SingedInRequirement : AccessRequirement
    {
        public SingedInRequirement()
            : base(AccessLevel.CookieSignedIn)
        {
        }
    }
}
