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
    public class ApiRegionController : BaseController
    {
        public ApiRegionController(
            IConfigurationRoot config,
            WebDbRepository repo,
            IHostingEnvironment env,
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, env, logger, mailService)
        {
        }


        [HttpPost]
        public async Task<IActionResult> GetRegions([FromBody]JObject param)
        {
            object result = null;
            object country = null;
            object error = null;

            int count = 0;

            Func<Task> func;

            try
            {
                country = await _repo.Execute<object>("GetDefaultCountry", true);


                int limit = 0;
                int offset = 0;

                string name = null;

                bool getTotalCatgs = false;

                if (param != null)
                {
                    limit = param.GetPropVal<int>("limit");
                    offset = param.GetPropVal<int>("offset");

                    name = param.GetPropVal<string>("name");

                    getTotalCatgs = param.GetPropVal<bool>("getTotalCatgs");
                }


                func = async () =>
                {
                    result = await _repo.Execute<object>("GetPivotRegions", 0, "", name, false, true, limit, offset);
                };

                if (getTotalCatgs)
                {
                    await Task.WhenAll(
                                Task.Run(async () =>
                                {
                                    count = await _repo.Execute<int>("GetRegionsCount", 0, "", name, false);
                                }),
                                func());
                }
                else
                {
                    await func();
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
                func = null;
            }

            return Ok(new { result = result, count = count, defCountry = country, error = error });
        }

        [HttpPost]
        public async Task<IActionResult> SaveRegions([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            object[] regions;

            try
            {
                if (param != null)
                {
                    regions = param.JGetPropVal<object[]>("regions");

                    if (regions != null
                        && regions.Length > 0)
                    {
                        for (int i = 0; i < regions.Length; i++)
                        {
                            regions[i] = await _repo.Execute<object>("ManagePivotedRegion", regions[i]);
                        }

                        result = regions;
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
                regions = null;
            }

            return Ok(new { result = result, error = error });
        }
    }
}
