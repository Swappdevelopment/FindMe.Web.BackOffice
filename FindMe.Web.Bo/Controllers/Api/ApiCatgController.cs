using FindMe.Data;
using FindMe.Data.Models;
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
    public class ApiCatgController : BaseController
    {
        public ApiCatgController(
            IConfigurationRoot config,
            WebDbRepository repo,
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, null, logger, mailService)
        {
        }


        [HttpPost]
        public async Task<IActionResult> GetCatgs([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            int count = 0;

            Func<Task> func;

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

                    parentID = param.GetPropVal<long>("parentID");

                    onlyParents = (parentID <= 0);

                    name = param.GetPropVal<string>("name");
                    getTotalCatgs = param.GetPropVal<bool>("getTotalCatgs");
                }


                func = async () =>
                {
                    result = await _repo.Execute<object>("GetPivotCategorys", 0, parentID, "", name, false, true, true, onlyParents, limit, offset);
                };

                if (getTotalCatgs)
                {
                    await Task.WhenAll(
                                Task.Run(async () =>
                                {
                                    count = await _repo.Execute<int>("GetCategorysCount", 0, parentID, "", name, false, onlyParents);
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

            return Ok(new { result = result, count = count, error = error });
        }

        [HttpPost]
        public async Task<IActionResult> SaveCatgs([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            Client[] clients;

            try
            {
                if (param != null)
                {
                    clients = param.JGetPropVal<Client[]>("clients");

                    if (clients != null
                        && clients.Length > 0)
                    {
                        for (int i = 0; i < clients.Length; i++)
                        {
                            clients[i] = await _repo.Execute<Client>("ManageClient", clients[i]);
                        }

                        result = clients.Select(l => l == null ? null : l.Simplify()).ToArray();
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
                clients = null;
            }

            return Ok(new { result = result, error = error });
        }
    }
}
