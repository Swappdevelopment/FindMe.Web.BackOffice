using FindMe.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swapp.Data;
using System;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class ApiAccountController : BaseController
    {
        public ApiAccountController(IConfigurationRoot config, WebDbReader reader)
            : base(config, reader)
        {
        }


        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]dynamic signInVm)
        {
            string userName = signInVm.userName;
            string password = signInVm.password;
            bool remember = signInVm.remember;

            object result = null;
            object error = null;

            string refreshTokenValue = null;
            string accessTokenValue = null;

            try
            {
                result = await _reader.Execute("SignIn", userName, password, TokenClientType.WebApp);

                refreshTokenValue = result.GetPropVal<string>("r");
                accessTokenValue = result.GetPropVal<string>("a");
            }
            catch (ExceptionID ex)
            {
                switch (ex.ErrorID)
                {
                    case MessageIdentifier.SIGNIN_FAILED:
                        error = new
                        {
                            msg = "Sign In failed",
                            id = (int)ex.ErrorID
                        };
                        break;

                    default:
                        return BadRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(new { error = error });
        }



        public IActionResult SignUp()
        {
            return View();
        }
    }
}
