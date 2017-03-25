using FindMe.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Swapp.Data;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class ApiAccountController : BaseController
    {
        public ApiAccountController(
            IConfigurationRoot config,
            WebDbRepository repo,
            IHostingEnvironment env,
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, env, logger, mailService)
        {
        }


        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]dynamic signInVm)
        {
            object result = null;
            object error = null;

            string refreshTokenValue = null;
            string accessTokenValue = null;
            bool invalidPassword = false;
            string fullName = null;

            try
            {
                string userName = signInVm.userName;
                string password = signInVm.password;
                bool remember = signInVm.remember;


                result = await _repo.Execute("SignIn", userName, password, TokenClientType.WebApp);

                refreshTokenValue = result.GetPropVal<string>("r");
                accessTokenValue = result.GetPropVal<string>("a");
                invalidPassword = result.GetPropVal<bool>("i");
                fullName = result.GetPropVal<string>("fn");

                AddSignedCookies(refreshTokenValue, accessTokenValue, invalidPassword, remember, fullName);
            }
            catch (ExceptionID ex)
            {
                switch (ex.ErrorID)
                {
                    case MessageIdentifier.SIGNIN_FAILED:
                        error = new
                        {
                            msg = this.GetMessage("msg_SgnInFld"),
                            id = (int)ex.ErrorID
                        };
                        break;

                    default:
                        return BadRequestEx(ex);
                }
            }
            catch (Exception ex)
            {
                return BadRequestEx(ex);
            }

            return Ok(new { error = error });
        }


        [HttpPost]
        public async Task<IActionResult> ManageProfile([FromBody]dynamic param = null)
        {
            object result = null;
            object error = null;

            string fullName = null;

            string action = null;

            bool? skipGet = false;

            dynamic profile = null;

            string tokenValue = null;

            try
            {
                profile = param.profile;
                action = param.action;
                skipGet = param.skipGet;

                switch (action == null ? "" : action.ToLower())
                {
                    case "cancelemailconfirmation":
                        await _repo.Execute("CancelUserEmailConfirmation");
                        break;

                    case "resendemailconfirmation":

                        tokenValue = profile.emailToValToken.ToString();

                        string link = Url.Action("ValidateEmail", "Account", null, this.Request.Scheme);
                        link += (link.EndsWith("/") ? "" : "/") + tokenValue;


                        string message = this.GetMessage("msg_ValEmail") + link;


                        await Task.WhenAll(
                                    _repo.Execute("SetEmailValidationTokenStatus", tokenValue, EmailSatus.Sending),
                                    _mailService.SendEmailAsync(profile.emailToVal.ToString(), this.GetLabel("lbl_FndMeBoValEmail"), message));

                        await _repo.Execute("SetEmailValidationTokenStatus", tokenValue, EmailSatus.Sent);

                        break;

                    default:
                        action = null;
                        if (profile != null)
                        {
                            fullName = await _repo.Execute<string>("UpdateTokenUserProfile", profile);
                        }
                        break;
                }

                if (skipGet != true)
                {
                    result = await _repo.Execute("GetTokenUserProfile");
                }

                if (profile != null
                    && string.IsNullOrEmpty(action))
                {
                    string str = this.GetCookieValue($"{TOKENS_KEY}:{REFRESH_TOKEN_EXP_DATE_KEY}");

                    DateTime? expDate = string.IsNullOrEmpty(str) ? null : new DateTime?(str.FromISOFormat());

                    this.AddCookie($"{TOKENS_KEY}:{USER_FULL_NAME}", fullName, expDate);
                }
            }
            catch (ExceptionID ex)
            {
                switch (ex.ErrorID)
                {
                    case MessageIdentifier.USERNAME_ALREADY_USED:
                        return BadRequest(this.GetMessage("msg_UsrnmAlrdUsed"));

                    case MessageIdentifier.USER_EMAIL_ALREADY_USED:
                        return BadRequest(this.GetMessage("msg_EmailAlrdUsed"));
                }

                return BadRequestEx(ex);
            }
            catch (Exception ex)
            {
                return BadRequestEx(ex);
            }

            return Ok(new { result = result, error = error, date = DateTime.Now, fn = fullName });
        }


        [HttpPost]
        public async Task<IActionResult> IsEmailValidationTokenConfirmed([FromBody]dynamic param = null)
        {
            bool result = false;

            try
            {
                string tokenValue = param.tokenValue;

                if (!string.IsNullOrEmpty(tokenValue))
                {
                    result = await _repo.Execute<bool>("IsEmailValidationTokenConfirmed", tokenValue);
                }
            }
            catch (Exception ex)
            {
                return BadRequestEx(ex);
            }

            return Ok(new { result = result });
        }


        private async Task<string> GetCountriesDotIso()
        {
            HttpClient hClient = null;

            string sNames, sIso3s, sCapitals, sPhones, sCurrencies;

            JObject names, iso3s, capitals, phones, currencies;

            try
            {
                hClient = new HttpClient();

                sNames = await hClient.GetStringAsync("http://country.io/names.json");
                sIso3s = await hClient.GetStringAsync("http://country.io/iso3.json");
                sCapitals = await hClient.GetStringAsync("http://country.io/capital.json");
                sPhones = await hClient.GetStringAsync("http://country.io/phone.json");
                sCurrencies = await hClient.GetStringAsync("http://country.io/currency.json");

                names = Helper.JSonDeserializeObject<JObject>(sNames);
                iso3s = Helper.JSonDeserializeObject<JObject>(sIso3s);
                capitals = Helper.JSonDeserializeObject<JObject>(sCapitals);
                phones = Helper.JSonDeserializeObject<JObject>(sPhones);
                currencies = Helper.JSonDeserializeObject<JObject>(sCurrencies);

                var results = (from jp in names.OfType<JProperty>()
                               select new
                               {
                                   code = jp.Name,
                                   name = names.JGetPropVal<string>(jp.Name),
                                   iso3 = iso3s.JGetPropVal<string>(jp.Name),
                                   capital = capitals.JGetPropVal<string>(jp.Name),
                                   phoneCode = phones.JGetPropVal<string>(jp.Name),
                                   currency = currencies.JGetPropVal<string>(jp.Name),
                               });


                return Helper.JSonSerializeObject(results);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (hClient != null)
                {
                    hClient.Dispose();
                    hClient = null;
                }
            }
        }


        public IActionResult SignUp()
        {
            return View();
        }
    }
}
