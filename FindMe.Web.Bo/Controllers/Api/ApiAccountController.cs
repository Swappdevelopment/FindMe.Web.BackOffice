using FindMe.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swapp.Data;
using System;
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


        public IActionResult SignUp()
        {
            return View();
        }
    }
}
