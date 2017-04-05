using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Swapp.Data;
using System;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class ApiCatgController : BaseController
    {
        public ApiCatgController(
            IConfigurationRoot config,
            WebDbRepository repo,
            IHostingEnvironment env,
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, env, logger, mailService)
        {
        }


        [HttpPost]
        public async Task<IActionResult> GetCatgs([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            int count = 0;

            Func<Task> funcGet;
            Func<Task> funcCount;

            try
            {
                int limit = 0;
                int offset = 0;

                long parentID = 0;

                bool onlyParents = false;

                string name = null;

                bool getTotalCatgs = false;

                if (param != null)
                {
                    limit = param.GetPropVal<int>("limit");
                    offset = param.GetPropVal<int>("offset");

                    name = param.GetPropVal<string>("name");

                    parentID = string.IsNullOrEmpty(name) ? param.GetPropVal<long>("parentID") : 0;

                    onlyParents = (parentID <= 0 && string.IsNullOrEmpty(name));

                    getTotalCatgs = param.GetPropVal<bool>("getTotalCatgs");
                }


                funcGet = async () =>
                {
                    result = await _repo.Execute<object>("GetPivotCategorys", 0, parentID, "", name, false, true, true, onlyParents, limit, offset);
                };

                if (getTotalCatgs)
                {
                    await _repo.VerifyLoginToken();

                    funcCount = async () =>
                    {
                        count = await _repo.Execute<int>("GetCategorysCount", 0, parentID, "", name, false, onlyParents);
                    };

                    await Task.WhenAll(
                                funcCount(),
                                funcGet());
                }
                else
                {
                    await funcGet();
                }

            }
            catch (ExceptionID ex)
            {
                switch (ex.ErrorID)
                {
                    default:
                        return BadRequestEx(ex);
                }
            }
            catch (Exception ex)
            {
                return BadRequestEx(ex);
            }
            finally
            {
                funcCount = null;
                funcGet = null;
            }

            return Ok(new { result = result, count = count, error = error });
        }

        [HttpPost]
        public async Task<IActionResult> SaveCatgs([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            object[] catgs;

            try
            {
                if (param != null)
                {
                    catgs = param.JGetPropVal<object[]>("catgs");

                    if (catgs != null
                        && catgs.Length > 0)
                    {
                        for (int i = 0; i < catgs.Length; i++)
                        {
                            catgs[i] = await _repo.Execute<object>("ManagePivotedCategory", catgs[i]);
                        }

                        result = catgs;
                    }
                }
            }
            catch (ExceptionID ex)
            {
                switch (ex.ErrorID)
                {
                    default:
                        return BadRequestEx(ex);
                }
            }
            catch (Exception ex)
            {
                return BadRequestEx(ex);
            }
            finally
            {
                catgs = null;
            }

            return Ok(new { result = result, error = error });
        }
    }
}
