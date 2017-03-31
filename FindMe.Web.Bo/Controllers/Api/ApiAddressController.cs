using FindMe.Data;
using FindMe.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Swapp.Data;
using System;
using System.Collections.Generic;
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
            IHostingEnvironment env,
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, env, logger, mailService)
        {
        }


        [HttpPost]
        public async Task<IActionResult> GetClients([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            List<Func<Task>> taskArr = null;

            int count = 0;

            try
            {
                int limit = 0;
                int offset = 0;

                string allNames = null;

                bool getTotalClients = false;

                if (param != null)
                {
                    limit = param.GetPropVal<int>("limit");
                    offset = param.GetPropVal<int>("offset");
                    allNames = param.GetPropVal<string>("allNames");
                    getTotalClients = param.GetPropVal<bool>("getTotalClients");
                }

                taskArr = new List<Func<Task>>()
                {
                    async () =>
                    {
                        result = (await _repo.Execute<Client[]>("GetClients", 0, "", "", SqlSearchMode.Equals, allNames, true, true, limit, offset))
                                             .Select(l => l.Simplify()).ToArray();
                    }
                };

                if (getTotalClients)
                {
                    taskArr.Add(async () =>
                                {
                                    count = await _repo.Execute<int>("GetClientsCount", 0, "", "", SqlSearchMode.Equals, allNames, true, true, limit, offset);
                                });
                }

                if (taskArr.Count > 1)
                {
                    await _repo.VerifyLoginToken();
                }

                await Task.WhenAll(taskArr.Select(l => l()));
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
                if (taskArr != null)
                {
                    taskArr.Clear();
                    taskArr = null;
                }
            }

            return Ok(new { result = result, count = count, error = error });
        }

        [HttpPost]
        public async Task<IActionResult> SaveClients([FromBody]JObject param)
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




        [HttpPost]
        public async Task<IActionResult> GetAddresses([FromBody]JObject param)
        {
            object result = null;
            object clients = null;
            object categorys = null;
            object cityDetails = null;
            object error = null;

            List<Func<Task>> taskArr = null;

            int count = 0;

            try
            {
                int limit = 0;
                int offset = 0;

                long addressId = 0;
                long categoryId = 0;
                long cityId = 0;

                bool getRefClients = false;
                bool getRefCatgs = false;
                bool getRefCities = false;

                string name = null;

                bool getTotalAddresses = false;

                if (param != null)
                {
                    limit = param.GetPropVal<int>("limit");
                    offset = param.GetPropVal<int>("offset");
                    name = param.GetPropVal<string>("name");

                    addressId = param.GetPropVal<long>("addressId");
                    categoryId = param.GetPropVal<long>("categoryId");
                    cityId = param.GetPropVal<long>("cityId");

                    getRefClients = param.GetPropVal<bool>("getRefClients");
                    getRefCatgs = param.GetPropVal<bool>("getRefCatgs");
                    getRefCities = param.GetPropVal<bool>("getRefCities");

                    getTotalAddresses = param.GetPropVal<bool>("getTotalAddresses");
                }

                taskArr = new List<Func<Task>>()
                {
                    async () =>
                    {
                        result = (await _repo.Execute<Address[]>("GetAddresses", 0, "", name, addressId, categoryId, cityId, false, false, false, false, limit, offset))
                                                .Select(l => l.Simplify(withCollections: false)).ToArray();
                    }
                };

                if (getTotalAddresses)
                {
                    taskArr.Add(async () =>
                                 {
                                     count = await _repo.Execute<int>("GetAddressesCount", 0, "", name, addressId, categoryId, cityId);
                                 });
                }

                if (getRefClients)
                {
                    taskArr.Add(async () =>
                                 {
                                     clients = await _repo.Execute<object[]>("GetRefClients");
                                 });
                }

                if (getRefCatgs)
                {
                    taskArr.Add(async () =>
                                 {
                                     categorys = await _repo.Execute<object[]>("GetRefOrderedCategorys");
                                 });
                }

                if (getRefCities)
                {
                    taskArr.Add(async () =>
                                 {
                                     cityDetails = await _repo.Execute<object[]>("GetRefCityDetails");
                                 });
                }

                if (taskArr.Count > 1)
                {
                    await _repo.VerifyLoginToken();
                }

                await Task.WhenAll(taskArr.Select(l => l()).ToArray());
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
                if (taskArr != null)
                {
                    taskArr.Clear();
                    taskArr = null;
                }
            }

            return Ok(new { result = result, count = count, clients = clients, categorys = categorys, cityDetails = cityDetails, error = error });
        }


        [HttpPost]
        public async Task<IActionResult> GetAddressContent([FromBody]JObject param)
        {
            object result = null;

            try
            {
                long addrID = 0;

                if (param != null)
                {
                    addrID = param.GetPropVal<long>("addrID");

                    result = await _repo.Execute<object>("GetAddressContent", addrID);
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

            return Ok(new { result = result });
        }

        [HttpPost]
        public async Task<IActionResult> SaveAddresses([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            Address[] addresss;

            try
            {
                if (param != null)
                {
                    addresss = param.JGetPropVal<Address[]>("addresss");

                    if (addresss != null
                        && addresss.Length > 0)
                    {
                        for (int i = 0; i < addresss.Length; i++)
                        {
                            addresss[i] = await _repo.Execute<Address>("ManageAddress", addresss[i]);
                        }

                        result = addresss.Select(l => l == null ? null : l.Simplify()).ToArray();
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
                addresss = null;
            }

            return Ok(new { result = result, error = error });
        }
    }
}
