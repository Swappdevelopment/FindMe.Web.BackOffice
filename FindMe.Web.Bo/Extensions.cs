using Microsoft.Extensions.Configuration;

namespace FindMe.Web.App
{
    public static class Extensions
    {
        public static bool IsMigration(this IConfigurationRoot obj)
        {
            try
            {
                return (obj["Migration:On"].ToLower() == "true");
            }
            catch
            {
                return false;
            }
        }
    }
}
