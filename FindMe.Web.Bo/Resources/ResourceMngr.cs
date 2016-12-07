using FindMe.Data.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Globalization;
using Swapp.Data;
using System.Collections.Generic;

namespace FindMe.Web.App
{
    public class ResourceMngr : IDisposable
    {
        private const string DEFAULT_LANGUAGE = "en-US";
        private const string HEADER_LANGUAGE_KEY = "Accept-Language";



        private CultureInfo _culture;

        private Dictionary<string, string> _rscLbls;
        private Dictionary<string, string> _rscMsgs;

        public ResourceMngr(string cultureName)
            : this(new CultureInfo(cultureName))
        {
        }

        public ResourceMngr(CultureInfo culture)
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


        public static ResourceMngr Create(IHtmlHelper htmlHelper)
        {
            return Create(htmlHelper == null || htmlHelper.ViewContext == null ? null : htmlHelper.ViewContext.HttpContext);
        }
        public static ResourceMngr Create(Controller controller)
        {
            return Create(controller == null ? null : controller.HttpContext);
        }
        public static ResourceMngr Create(HttpContext context)
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

            return new ResourceMngr(lang);
        }
    }
}
