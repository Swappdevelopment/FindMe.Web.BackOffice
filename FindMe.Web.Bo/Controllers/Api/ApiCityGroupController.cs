using FindMe.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Swapp.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class ApiCityGroupController : BaseController
    {
        public ApiCityGroupController(
            IConfigurationRoot config,
            WebDbRepository repo,
            IHostingEnvironment env,
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, env, logger, mailService)
        {
        }


        [HttpPost]
        public async Task<IActionResult> GetCityGroups([FromBody]JObject param)
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

                bool getTotalCityGroups = false;

                if (param != null)
                {
                    limit = param.GetPropVal<int>("limit");
                    offset = param.GetPropVal<int>("offset");

                    name = param.GetPropVal<string>("name");

                    getTotalCityGroups = param.GetPropVal<bool>("getTotalCityGroups");
                }


                func = async () =>
                {
                    result = (await _repo.Execute<CityGroup[]>("GetCityGroups", 0, "", name, false, limit, offset)).Select(l => l.Simplify()).ToArray();
                };

                if (getTotalCityGroups)
                {
                    await Task.WhenAll(
                                Task.Run(async () =>
                                {
                                    count = await _repo.Execute<int>("GetCityGroupsCount", 0, "", name, false);
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
        public async Task<IActionResult> SaveCityGroups([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            CityGroup[] cityGroups;

            try
            {
                if (param != null)
                {
                    cityGroups = param.JGetPropVal<CityGroup[]>("cityGroups");

                    if (cityGroups != null
                        && cityGroups.Length > 0)
                    {
                        for (int i = 0; i < cityGroups.Length; i++)
                        {
                            cityGroups[i] = await _repo.Execute<CityGroup>("ManageCityGroup", cityGroups[i]);
                        }

                        result = cityGroups.Select(l=> l == null ? null : l.Simplify()).ToArray();
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
                cityGroups = null;
            }

            return Ok(new { result = result, error = error });
        }
    }
}
