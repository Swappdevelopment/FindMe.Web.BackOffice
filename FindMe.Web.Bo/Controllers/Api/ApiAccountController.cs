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
            ILogger<ApiAccountController> logger)
            : base(config, repo, null, logger)
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
        public async Task<IActionResult> GetProfile([FromBody]dynamic profile = null)
        {
            object result = null;
            object error = null;

            try
            {
                if (profile != null)
                {

                }

                result = await _repo.Execute("GetTokenUserProfile");
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
