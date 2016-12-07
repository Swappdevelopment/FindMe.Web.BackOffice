using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swapp.Data;
using System;

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


        public static ILoggerFactory AddDatabase(
            this ILoggerFactory factory,
            LogLevel minLevel)
        {
            return AddDatabase(
                factory,
                (_, logLevel) => logLevel >= minLevel);
        }
        public static ILoggerFactory AddDatabase(
            this ILoggerFactory factory,
            Func<string, LogLevel, bool> filter = null)
        {
            factory.AddProvider(new DatabaseLoggerProvider(filter));
            return factory;
        }

        public static void LogCriticalEx(
            this ILogger logger,
            Exception exception,
            WebDbRepository repo,
            bool withStackTrace = true,
            string description = "")
        {
            logger.LogEx(LogLevel.Critical, exception, repo, withStackTrace, description);
        }

        public static void LogErrorEx(
            this ILogger logger,
            Exception exception,
            WebDbRepository repo,
            bool withStackTrace = true,
            string description = "")
        {
            logger.LogEx(LogLevel.Error, exception, repo, withStackTrace, description);
        }

        public static void LogEx(
            this ILogger logger,
            LogLevel logLevel,
            Exception exception,
            WebDbRepository repo,
            bool withStackTrace = true,
            string description = "")
        {
            logger.Log(
                    logLevel,
                    new EventId((int)DateTime.Now.ToFileTimeUtc(), description),
                    new { withStackTrace = withStackTrace, repository = repo },
                    exception,
                    (o, ex) =>
                    {
                        return ex.MergeMsgInnerExMsgs();
                    });
        }



        public static bool UserHasSignedInCookie(this IHtmlHelper htmlHelper)
        {
            string fullName;
            return htmlHelper.UserHasSignedInCookie(out fullName);
        }
        public static bool UserHasSignedInCookie(this IHtmlHelper htmlHelper, out string fullName)
        {
            fullName = "";

            if (htmlHelper == null
                || htmlHelper.ViewContext == null
                || htmlHelper.ViewContext.HttpContext == null
                || htmlHelper.ViewContext.HttpContext.Request == null
                || htmlHelper.ViewContext.HttpContext.Request.Cookies == null) return false;


            fullName = htmlHelper.ViewContext.HttpContext.Request.Cookies[$"{BaseController.TOKENS_KEY}:{BaseController.USER_FULL_NAME}"];

            return !string.IsNullOrEmpty(htmlHelper.ViewContext.HttpContext.Request.Cookies[$"{BaseController.TOKENS_KEY}:{BaseController.ACCESS_TOKEN_KEY}"]);
        }
    }
}
