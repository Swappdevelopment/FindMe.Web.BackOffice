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
    public class ApiDistrictController : BaseController
    {
        public ApiDistrictController(
            IConfigurationRoot config,
            WebDbRepository repo,
            IHostingEnvironment env,
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, env, logger, mailService)
        {
        }


        [HttpPost]
        public async Task<IActionResult> GetDistricts([FromBody]JObject param)
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

                bool getTotalDistricts = false;

                if (param != null)
                {
                    limit = param.GetPropVal<int>("limit");
                    offset = param.GetPropVal<int>("offset");

                    name = param.GetPropVal<string>("name");

                    getTotalDistricts = param.GetPropVal<bool>("getTotalDistricts");
                }


                func = async () =>
                {
                    result = (await _repo.Execute<CityDistrict[]>("GetCityDistricts", 0, "", name, false, limit, offset)).Select(l => l.Simplify()).ToArray();
                };

                if (getTotalDistricts)
                {
                    await Task.WhenAll(
                                Task.Run(async () =>
                                {
                                    count = await _repo.Execute<int>("GetCityDistrictsCount", 0, "", name, false);
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
        public async Task<IActionResult> SaveDistricts([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            CityDistrict[] districts;

            try
            {
                if (param != null)
                {
                    districts = param.JGetPropVal<CityDistrict[]>("districts");

                    if (districts != null
                        && districts.Length > 0)
                    {
                        for (int i = 0; i < districts.Length; i++)
                        {
                            districts[i] = await _repo.Execute<CityDistrict>("ManageCityDistrict", districts[i]);
                        }

                        result = districts.Select(l=> l == null ? null : l.Simplify()).ToArray();
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
                districts = null;
            }

            return Ok(new { result = result, error = error });
        }
    }
}
