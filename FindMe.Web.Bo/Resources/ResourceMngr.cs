using FindMe.Data.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FindMe.Web.App
{
    public static class ResourceMngr
    {
        private static _ResourceMngr _rscMngr = null;

        private static _ResourceMngr InitMngr(HttpContext context)
        {
            if (_rscMngr == null)
            {
                _rscMngr = _ResourceMngr.Create(context);
            }

            return _rscMngr;
        }



        public static string GetLabel(string key, IHtmlHelper htmlHelper)
        {
            return GetLabel(key, htmlHelper == null || htmlHelper.ViewContext == null ? null : htmlHelper.ViewContext.HttpContext);
        }

        public static string GetLabel(string key, Controller controller)
        {
            return GetLabel(key, controller == null ? null : controller.HttpContext);
        }

        public static string GetLabel(string key, HttpContext context)
        {
            return InitMngr(context).GetLabel(key);
        }



        public static string GetMessage(string key, IHtmlHelper htmlHelper)
        {
            return GetMessage(key, htmlHelper == null || htmlHelper.ViewContext == null ? null : htmlHelper.ViewContext.HttpContext);
        }

        public static string GetMessage(string key, Controller controller)
        {
            return GetMessage(key, controller == null ? null : controller.HttpContext);
        }

        public static string GetMessage(string key, HttpContext context)
        {
            return InitMngr(context).GetMessage(key);
        }



        private class _ResourceMngr : IDisposable
        {
            private const string DEFAULT_LANGUAGE = "en-US";
            private const string HEADER_LANGUAGE_KEY = "Accept-Language";



            private CultureInfo _culture;

            private Dictionary<string, string> _rscLbls;
            private Dictionary<string, string> _rscMsgs;

            public _ResourceMngr(string cultureName)
                : this(new CultureInfo(cultureName))
            {
            }

            public _ResourceMngr(CultureInfo culture)
            {
                _culture = culture;

                _rscLbls = ResourcesRepository.GetResourceLabels(culture);
                _rscMsgs = ResourcesRepository.GetResourceMessages(culture);
            }


            public string GetLabel(string key)
            {
                return GetValue(_rscLbls, key);
            }

            public string GetMessage(string key)
            {
                return GetValue(_rscMsgs, key);
            }


            private string GetValue(Dictionary<string, string> dic, string key)
            {
                if (dic != null
                    && !string.IsNullOrEmpty(key))
                {
                    string value;

                    if (dic.TryGetValue(key, out value))
                    {
                        return value;
                    }
                }


                return key;
            }



            public void Dispose()
            {
                _culture = null;
            }

            public override string ToString()
            {
                return _culture == null ? base.ToString() : _culture.ToString();
            }


            public static _ResourceMngr Create(IHtmlHelper htmlHelper)
            {
                return Create(htmlHelper == null || htmlHelper.ViewContext == null ? null : htmlHelper.ViewContext.HttpContext);
            }
            public static _ResourceMngr Create(Controller controller)
            {
                return Create(controller == null ? null : controller.HttpContext);
            }
            public static _ResourceMngr Create(HttpContext context)
            {
                string lang = "";

                if (context != null
                    && context.Request != null
                    && context.Request.Headers != null)
                {
                    Microsoft.Extensions.Primitives.StringValues value;

                    if (context.Request.Headers.TryGetValue(HEADER_LANGUAGE_KEY, out value))
                    {
                        if (value.Count > 0
                            && !string.IsNullOrEmpty(value[0]))
                        {
                            var arr = value[0].Split(',', ';');

                            if (arr != null
                                && arr.Length > 0)
                            {
                                lang = arr[0].Trim();
                            }

                            arr = null;
                        }
                    }
                }

                if (string.IsNullOrEmpty(lang))
                {
                    lang = DEFAULT_LANGUAGE;
                }

                return new _ResourceMngr(lang);
            }
        }
    }
}
