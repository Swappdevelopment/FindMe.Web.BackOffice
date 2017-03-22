using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swapp.Data;
using System;
using System.IO;

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


        public static string GetCacheVersion(this IConfigurationRoot obj)
        {
            try
            {
                return obj["App:CacheVersion"];
            }
            catch
            {
                return "";
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
            bool withStackTrace = false,
            string description = "")
        {
            logger.LogEx(LogLevel.Critical, exception, repo, withStackTrace, description);
        }

        public static void LogErrorEx(
            this ILogger logger,
            Exception exception,
            WebDbRepository repo,
            bool withStackTrace = false,
            string description = "")
        {
            logger.LogEx(LogLevel.Error, exception, repo, withStackTrace, description);
        }

        public static void LogEx(
            this ILogger logger,
            LogLevel logLevel,
            Exception exception,
            WebDbRepository repo,
            bool withStackTrace = false,
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


        public static string GetCurrentLang(this IHtmlHelper htmlHelper)
        {
            return ResourceMngr.GetCurrentLang(htmlHelper == null || htmlHelper.ViewContext == null ? null : htmlHelper.ViewContext.HttpContext);
        }
        public static string GetCurrentLang(this HttpContext context)
        {
            return ResourceMngr.GetCurrentLang(context);
        }


        public static string GetLabel(this IHtmlHelper htmlHelper, string key)
        {
            return ResourceMngr.GetLabel(key, htmlHelper);
        }
        public static string GetMessage(this IHtmlHelper htmlHelper, string key)
        {
            return ResourceMngr.GetMessage(key, htmlHelper);
        }

        public static string GetLabel(this Controller controller, string key)
        {
            return ResourceMngr.GetLabel(key, controller);
        }
        public static string GetMessage(this Controller controller, string key)
        {
            return ResourceMngr.GetMessage(key, controller);
        }


        public static IHtmlContent ToJSonHtmlString(this IHtmlHelper htmlHelper, object value)
        {
            if (htmlHelper == null)
                throw new NullReferenceException();


            if (value == null)
                return new HtmlString("");

            if (!(value is string))
            {
                value = Helper.JSonSerializeObject(value);
            }

            string result = (string)value;

            if (result.Length > 1)
            {
                if (result.StartsWith("\""))
                {
                    result = result.Substring(1, result.Length - 1);
                }

                if (result.Length > 1)
                {
                    if (result.EndsWith("\""))
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                }

                //result = result.Replace("\"", "'");
            }


            return htmlHelper.Raw(result);
        }


        public static string GetResPath(this IUrlHelper url, string path, string rootPath)
        {
            if (string.IsNullOrEmpty(rootPath)) return path;


            if (path.StartsWith("~"))
            {
                if (url == null) return path;

                path = url.Content(path);
            }

            path = path.StartsWith("/") ? path : $"/{path}";

            string filePath = path.Replace("/", "\\");

            filePath = (rootPath.EndsWith("\\") ? rootPath.Substring(0, rootPath.Length - 1) : rootPath) + filePath;

            string version = "";

            if (File.Exists(filePath))
            {
                var fi = new FileInfo(filePath);

                version = $"?v={fi.LastWriteTimeUtc.ToFileTime()}";
            }

            return path + version;
        }
    }
}
