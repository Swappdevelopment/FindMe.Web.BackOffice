namespace FindMe.Web.App
{
    public enum AccessLevel : short
    {
        Anonymous = 0,
        CookieSignedIn = 10,
        DbSignedIn = 20,
        HasMenuAccess = 30
    }
}
