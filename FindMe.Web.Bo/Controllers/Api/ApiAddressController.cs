using FindMe.Data;
using FindMe.Data.Models;
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
    public class ApiAddressController : BaseController
    {
        public ApiAddressController(
            IConfigurationRoot config,
            WebDbRepository repo,
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, null, logger, mailService)
        {
        }


        [HttpPost]
        public async Task<IActionResult> GetClients([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            Client[] clients;

            int count = 0;

            try
            {
                int limit = 0;
                int offset = 0;

                bool getTotalClients = false;

                if (param != null)
                {
                    limit = param.GetPropVal<int>("limit");
                    offset = param.GetPropVal<int>("offset");
                    getTotalClients = param.GetPropVal<bool>("getTotalClients");
                }

                if (getTotalClients)
                {
                    await Task.WhenAll(
                                Task.Run(async () =>
                                {
                                    count = await _repo.Execute<int>("GetClientsCount", "", "", SqlSearchMode.Equals, true, true, limit, offset);
                                }),
                                Task.Run(async () =>
                                {
                                    clients = await _repo.Execute<Client[]>("GetClients", "", "", SqlSearchMode.Equals, true, true, limit, offset);
                                    result = clients.Select(l => l.Simplify()).ToArray();
                                }));
                }
                else
                {
                    clients = await _repo.Execute<Client[]>("GetClients", "", "", SqlSearchMode.Equals, true, true, limit, offset);
                    result = clients.Select(l => l.Simplify()).ToArray();
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

            return Ok(new { result = result, count = count, error = error });
        }
    }
}
