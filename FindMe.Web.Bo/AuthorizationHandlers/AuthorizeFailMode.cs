namespace FindMe.Web.App
{
    public enum AuthorizeFailMode : short
    {
        None = 0
            ,
        Redirect = 10
            ,
        ChangeControllerPropertyValue = 20
    }
}
