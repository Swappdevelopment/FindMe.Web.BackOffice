﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class BaseController : Controller
    {
        public const string TOKENS_KEY = "APP_TOKENS";
        public const string REFRESH_TOKEN_KEY = "REFRESH_TOKEN";
        public const string ACCESS_TOKEN_KEY = "ACCESS_TOKEN";
        public const string REFRESH_TOKEN_EXP_DATE_KEY = "REFRESH_TOKEN_EXP_DATE";
        public const string INVLD_PASSWORD_FORMAT_TOKEN_KEY = "INVLD_PASSWORD_FORMAT_TOKEN";
        public const string REMEMBER_USER = "REMEMBER_USER";
        public const string USER_FULL_NAME = "USER_FULL_NAME";



        protected IConfigurationRoot _config;
        protected WebDbRepository _repo;
        protected ILogger _logger;

        protected string _checkedAccessToken;


        public BaseController(
            IConfigurationRoot config
            , WebDbRepository repo
            , ILogger logger)
            : base()
        {
            _config = config;
            _repo = repo;
            _logger = logger;

            if (_repo != null)
            {
                _repo.ChangeAccessToken += Reader_ChangeAccessToken;
                _repo.RequestPropertiesInit += _repo_RequestPropertiesInit;
            }

            _checkedAccessToken = null;
        }


        private void Reader_ChangeAccessToken(object sender, Swapp.Data.ValuePairEventArgs<string, bool> e)
        {
            string refreshToken = this.GetCookieValue($"{TOKENS_KEY}:{REFRESH_TOKEN_KEY}");
            string rememberToken = this.GetCookieValue($"{TOKENS_KEY}:{REMEMBER_USER}");
            string fullName = this.GetCookieValue($"{TOKENS_KEY}:{USER_FULL_NAME}");

            this.AddSignedCookies(
                refreshToken,
                e.Value1,
                e.Value2,
                (!string.IsNullOrEmpty(rememberToken) && rememberToken.ToLower() == "true"),
                fullName);
        }

        private void _repo_RequestPropertiesInit(object sender, EventArgs e)
        {
            GetClientIpAddressV4ASync().ContinueWith(task =>
            {
                if (_repo != null)
                {
                    _repo.SetClientIpAddress(task.Result);
                }
            });

            _repo.SetTokenValues(
                        GetCookieValue($"{TOKENS_KEY}:{REFRESH_TOKEN_KEY}"),
                        GetCookieValue($"{TOKENS_KEY}:{ACCESS_TOKEN_KEY}"));
        }



        public string ClientIpAddress
        {
            get
            {
                if (this.HttpContext != null
                    && this.HttpContext.Connection != null)
                {
                    return this.HttpContext.Connection.RemoteIpAddress.ToString();
                }

                return null;
            }
        }


        public async Task<string> GetClientIpAddressV4ASync()
        {
            string result = this.ClientIpAddress;

            if (!string.IsNullOrEmpty(this.ClientIpAddress))
            {
                IPAddress ipAddress;

                if (IPAddress.TryParse(result, out ipAddress))
                {
                    if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        ipAddress = (await Dns.GetHostEntryAsync(ipAddress)).AddressList.FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    }

                    result = ipAddress == null ? result : ipAddress.ToString();
                }
            }

            return result;
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_repo != null)
            {
                _repo.ChangeAccessToken -= Reader_ChangeAccessToken;
            }
        }


        public void LogCritical(Exception ex, bool withStackTrace = true, string description = "")
        {
            if (_logger != null
                && _repo != null)
            {
                _logger.LogCriticalEx(ex, _repo, withStackTrace, description);
            }
        }
        public void LogError(Exception ex, bool withStackTrace = true, string description = "")
        {
            if (_logger != null
                && _repo != null)
            {
                _logger.LogErrorEx(ex, _repo, withStackTrace, description);
            }
        }






        protected void AddCookie(string key, string value, DateTime? expireryDate = null)
        {
            AddCookies(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>(key, value) }, expireryDate);
        }
        protected void AddCookies(KeyValuePair<string, string>[] cookies, DateTime? expireryDate = null)
        {
            if (Response == null
                || Response.Cookies == null) return;


            if (cookies == null
                || cookies.Length == 0) return;


            if (expireryDate == null)
            {
                foreach (var ck in cookies)
                {
                    Response.Cookies.Append(
                        ck.Key,
                        ck.Value);
                }
            }
            else
            {
                foreach (var ck in cookies)
                {
                    Response.Cookies.Append(
                        ck.Key,
                        ck.Value,
                        new CookieOptions()
                        {
                            Expires = DateTime.Now.AddDays(30)
                        });
                }
            }
        }

        protected void RemoveCookie(string key)
        {
            RemoveCookies(new string[] { key });
        }
        protected void RemoveCookies(string[] keys)
        {
            if (Response == null
                || Response.Cookies == null) return;


            if (keys == null
                || keys.Length == 0) return;


            foreach (var key in keys)
            {
                Response.Cookies.Append(
                    key,
                    "",
                    new CookieOptions()
                    {
                        Expires = DateTime.Now.AddDays(-10)
                    });
            }
        }

        protected string GetCookieValue(string key)
        {
            if (Request == null
                || Request.Cookies == null
                || string.IsNullOrEmpty(key)) return string.Empty;


            return Request.Cookies[key];
        }
        protected KeyValuePair<string, string>[] GetCookies(string[] keys)
        {

            if (Request == null
                || Request.Cookies == null
                || keys == null
                || keys.Length == 0) return new KeyValuePair<string, string>[0];


            return (from key in keys
                    select new KeyValuePair<string, string>(key, Request.Cookies[key])).ToArray();
        }


        protected void AddSignedCookies(
            string refreshTokenValue,
            string accessTokenValue,
            bool invalidPassword,
            bool remember,
            string fullName)
        {
            AddCookies(
                new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>($"{TOKENS_KEY}:{REFRESH_TOKEN_KEY}", refreshTokenValue),
                    new KeyValuePair<string, string>($"{TOKENS_KEY}:{ACCESS_TOKEN_KEY}", accessTokenValue),
                    new KeyValuePair<string, string>($"{TOKENS_KEY}:{INVLD_PASSWORD_FORMAT_TOKEN_KEY}", invalidPassword.ToString()),
                    new KeyValuePair<string, string>($"{TOKENS_KEY}:{REMEMBER_USER}", remember.ToString()),
                    new KeyValuePair<string, string>($"{TOKENS_KEY}:{USER_FULL_NAME}", fullName)
                },
                remember ? new DateTime?(DateTime.Now.AddDays(30)) : null);
        }

        protected void RemoveSignedCookies()
        {
            RemoveCookies(
                new string[]
                {
                    $"{TOKENS_KEY}:{REFRESH_TOKEN_KEY}",
                    $"{TOKENS_KEY}:{ACCESS_TOKEN_KEY}",
                    $"{TOKENS_KEY}:{INVLD_PASSWORD_FORMAT_TOKEN_KEY}",
                    $"{TOKENS_KEY}:{USER_FULL_NAME}"
                });
        }




        protected IActionResult CheckForRedirection(AccessLevel level)
        {
            switch (level)
            {
                case AccessLevel.CookieSignedIn:
                    _checkedAccessToken = this.GetCookieValue($"{TOKENS_KEY}:{ACCESS_TOKEN_KEY}");

                    if (string.IsNullOrEmpty(_checkedAccessToken))
                    {
                        return RedirectToAction("SignIn", "Account");
                    }
                    break;
            }


            return null;
        }
    }
}
