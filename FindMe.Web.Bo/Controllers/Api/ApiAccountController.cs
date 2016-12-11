using FindMe.Data;
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
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, null, logger, mailService)
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
                            msg = this.GetMessage("Msg_SgnInFld"),
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
        public async Task<IActionResult> ManageProfile([FromBody]dynamic profile = null)
        {
            object result = null;
            object error = null;

            try
            {
                if (profile != null)
                {
                    await _repo.Execute("UpdateTokenUserProfile", profile);
                }

                result = await _repo.Execute("GetTokenUserProfile");
            }
            catch (ExceptionID ex)
            {
                string msg = null;

                switch (ex.ErrorID)
                {
                    case MessageIdentifier.USERNAME_ALREADY_USED:
                        msg = this.GetMessage("Msg_UsrnmAlrdUsed");
                        break;

                    case MessageIdentifier.USER_EMAIL_ALREADY_USED:
                        msg = this.GetMessage("Msg_EmailAlrdUsed");
                        break;
                }

                return BadRequestEx(ex, msg);
            }
            catch (Exception ex)
            {
                return BadRequestEx(ex);
            }

            return Ok(new { result = result, error = error, date = DateTime.Now });
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
