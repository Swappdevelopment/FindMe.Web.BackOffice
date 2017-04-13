using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Swapp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class ApiTagController : BaseController
    {
        public ApiTagController(
            IConfigurationRoot config,
            WebDbRepository repo,
            IHostingEnvironment env,
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, env, logger, mailService)
        {
        }


        [HttpPost]
        public async Task<IActionResult> GetTags([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            int count = 0;

            List<Func<Task>> tasks = null;

            try
            {
                int limit = 0;
                int offset = 0;

                string name = null;

                bool getTotalTags = false;

                if (param != null)
                {
                    limit = param.GetPropVal<int>("limit");
                    offset = param.GetPropVal<int>("offset");

                    name = param.GetPropVal<string>("name");

                    getTotalTags = param.GetPropVal<bool>("getTotalTags");
                }

                tasks = new List<Func<Task>>();

                tasks.Add(async () =>
                {
                    result = await _repo.Execute<object>("GetPivotTags", 0, "", name, true, limit, offset);
                });

                if (getTotalTags)
                {

                    tasks.Add(async () =>
                    {
                        count = await _repo.Execute<int>("GetTagsCount", 0, "", name);
                    });
                }

                if (tasks.Count > 1)
                {
                    await _repo.VerifyLoginToken();
                }

                await Task.WhenAll(tasks.Select(f => f()).ToArray());

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
                if (tasks != null)
                {
                    tasks.Clear();
                    tasks = null;
                }
            }

            return Ok(new { result = result, count = count, error = error });
        }

        [HttpPost]
        public async Task<IActionResult> SaveTags([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            object[] tags;

            try
            {
                if (param != null)
                {
                    tags = param.JGetPropVal<object[]>("tags");

                    if (tags != null
                        && tags.Length > 0)
                    {
                        for (int i = 0; i < tags.Length; i++)
                        {
                            tags[i] = await _repo.Execute<object>("ManagePivotedTag", tags[i]);
                        }

                        result = tags;
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
                tags = null;
            }

            return Ok(new { result = result, error = error });
        }
    }
}
