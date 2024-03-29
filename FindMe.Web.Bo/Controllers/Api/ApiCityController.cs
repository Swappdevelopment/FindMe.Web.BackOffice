﻿using FindMe.Data.Models;
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
    public class ApiCityController : BaseController
    {
        public ApiCityController(
            IConfigurationRoot config,
            WebDbRepository repo,
            IHostingEnvironment env,
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, env, logger, mailService)
        {
        }




        private async Task<object> GetCityDetails(
            int limit,
            int offset,
            string name,
            long regionId,
            long districtId,
            long cityGroupId,
            bool getRefRegions,
            bool getRefDistricts,
            bool getRefCityGroups,
            bool getTotal)
        {
            List<Func<Task>> lstFuncs = null;

            object[] collection = null;
            object[] regions = null;
            object[] districts = null;
            object[] cityGroups = null;

            int count = 0;

            try
            {
                lstFuncs = new List<Func<Task>>()
                {
                    async () =>
                    {
                        collection = (await _repo.Execute<CityDetail[]>("GetCityDetails", 0, "", name, regionId, districtId, cityGroupId, false, false, limit, offset))
                                        .Select(l => l.Simplify()).ToArray();
                    }
                };

                if (getTotal)
                {
                    lstFuncs.Add(async () =>
                                {
                                    count = await _repo.Execute<int>("GetCityDetailsCount", 0, "", name, regionId, districtId, cityGroupId, false);
                                });
                }

                if (getRefRegions)
                {
                    lstFuncs.Add(async () =>
                    {
                        regions = await _repo.Execute<object[]>("GetRefRegions");
                    });
                }

                if (getRefDistricts)
                {
                    lstFuncs.Add(async () =>
                    {
                        districts = await _repo.Execute<object[]>("GetRefDistricts");
                    });
                }

                if (getRefCityGroups)
                {
                    lstFuncs.Add(async () =>
                    {
                        cityGroups = await _repo.Execute<object[]>("GetRefCityGroups");
                    });
                }


                if (lstFuncs.Count > 1)
                {
                    await _repo.VerifyLoginToken();
                }

                await Task.WhenAll(lstFuncs.Select(l => l()).ToArray());

                return new
                {
                    result = collection,
                    regions = regions,
                    districts = districts,
                    cityGroups = cityGroups,
                    count = count
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (lstFuncs != null)
                {
                    lstFuncs.Clear();
                    lstFuncs = null;
                }

                collection = null;
            }
        }

        private async Task<object> GetRegions(int limit, int offset, string name, bool getTotal)
        {
            List<Func<Task>> lstFuncs = null;

            object[] collection = null;

            int count = 0;

            try
            {
                lstFuncs = new List<Func<Task>>()
                {
                    async () =>
                    {
                        collection =  await _repo.Execute<object[]>("GetPivotRegions", 0, "", name, false, true, limit, offset);
                    }
                };

                if (getTotal)
                {
                    lstFuncs.Add(async () =>
                                {
                                    count = await _repo.Execute<int>("GetRegionsCount", 0, "", name, false);
                                });
                }



                if (lstFuncs.Count > 1)
                {
                    await _repo.VerifyLoginToken();
                }

                await Task.WhenAll(lstFuncs.Select(l => l()).ToArray());

                return new
                {
                    result = collection,
                    count = count
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (lstFuncs != null)
                {
                    lstFuncs.Clear();
                    lstFuncs = null;
                }

                collection = null;
            }
        }

        private async Task<object> GetDistricts(int limit, int offset, string name, bool getTotal)
        {
            List<Func<Task>> lstFuncs = null;

            object[] collection = null;

            int count = 0;

            try
            {
                lstFuncs = new List<Func<Task>>()
                {
                    async () =>
                    {
                        collection = (await _repo.Execute<CityDistrict[]>("GetCityDistricts", 0, "", name, false, limit, offset)).Select(l => l.Simplify()).ToArray();
                    }
                };

                if (getTotal)
                {
                    lstFuncs.Add(async () =>
                                {
                                    count = await _repo.Execute<int>("GetCityDistrictsCount", 0, "", name, false);
                                });
                }



                if (lstFuncs.Count > 1)
                {
                    await _repo.VerifyLoginToken();
                }

                await Task.WhenAll(lstFuncs.Select(l => l()).ToArray());

                return new
                {
                    result = collection,
                    count = count
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (lstFuncs != null)
                {
                    lstFuncs.Clear();
                    lstFuncs = null;
                }

                collection = null;
            }
        }

        private async Task<object> GetCityGroups(int limit, int offset, string name, bool getTotal)
        {
            List<Func<Task>> lstFuncs = null;

            object[] collection = null;

            int count = 0;

            try
            {
                lstFuncs = new List<Func<Task>>()
                {
                    async () =>
                    {
                        collection = (await _repo.Execute<CityGroup[]>("GetCityGroups", 0, "", name, false, limit, offset)).Select(l => l.Simplify()).ToArray();
                    }
                };

                if (getTotal)
                {
                    lstFuncs.Add(async () =>
                                    {
                                        count = await _repo.Execute<int>("GetCityGroupsCount", 0, "", name, false);
                                    });
                }

                if (lstFuncs.Count > 1)
                {
                    await _repo.VerifyLoginToken();
                }

                await Task.WhenAll(lstFuncs.Select(l => l()).ToArray());

                return new
                {
                    result = collection,
                    count = count
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (lstFuncs != null)
                {
                    lstFuncs.Clear();
                    lstFuncs = null;
                }

                collection = null;
            }
        }




        [HttpPost]
        public async Task<IActionResult> GetAllCityStuff()
        {
            List<Func<Task>> lstFuncs = null;

            object country = null;

            object cityDetailsData = null;
            object regionsData = null;
            object districtsData = null;
            object cityGroupsData = null;

            try
            {
                country = await _repo.Execute<object>("GetDefaultCountry", true);

                int limit = BaseController.SEARCH_RESULT_ITEMS_PER_PAGE;
                int offset = 0;

                bool getTotals = true;

                lstFuncs = new List<Func<Task>>()
                {
                    async () =>
                    {
                        cityDetailsData = await GetCityDetails(limit, offset, null, 0, 0, 0, false, false, false, getTotals);
                    },
                    async () =>
                    {
                        regionsData = await GetRegions(limit, offset, null, getTotals);
                    },
                    async () =>
                    {
                        districtsData = await GetDistricts(limit, offset, null, getTotals);
                    },
                    async () =>
                    {
                        cityGroupsData = await GetCityGroups(limit, offset, null, getTotals);
                    }
                };

                await _repo.VerifyLoginToken();
                await Task.WhenAll(lstFuncs.Select(l => l()).ToArray());
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
                if (lstFuncs != null)
                {
                    lstFuncs.Clear();
                    lstFuncs = null;
                }
            }

            return Ok(
                    new
                    {
                        cityDetailsData = cityDetailsData,
                        regionsData = regionsData,
                        districtsData = districtsData,
                        cityGroupsData = cityGroupsData
                    });
        }


        [HttpPost]
        public async Task<IActionResult> GetCityDetails([FromBody]JObject param)
        {
            object result = null;
            object regions = null;
            object districts = null;
            object cityGroups = null;
            object error = null;

            int count = 0;

            try
            {
                int limit = 0;
                int offset = 0;

                string name = null;

                long regionId = 0;
                long districtId = 0;
                long cityGroupId = 0;

                bool getRefRegions = false;
                bool getRefDistricts = false;
                bool getRefCityGroups = false;

                bool getTotalCityDetails = false;

                if (param != null)
                {
                    limit = param.GetPropVal<int>("limit");
                    offset = param.GetPropVal<int>("offset");

                    regionId = param.GetPropVal<long>("regionId");
                    districtId = param.GetPropVal<long>("districtId");
                    cityGroupId = param.GetPropVal<long>("cityGroupId");

                    name = param.GetPropVal<string>("name");

                    getRefRegions = param.GetPropVal<bool>("getRefRegions");
                    getRefDistricts = param.GetPropVal<bool>("getRefDistricts");
                    getRefCityGroups = param.GetPropVal<bool>("getRefCityGroups");

                    getTotalCityDetails = param.GetPropVal<bool>("getTotalCityDetails");
                }


                result = await GetCityDetails(limit, offset, name, regionId, districtId, cityGroupId, getRefRegions, getRefDistricts, getRefCityGroups, getTotalCityDetails);


                if (result != null)
                {
                    count = result.GetPropVal<int>("count");
                    regions = result.GetPropVal("regions");
                    districts = result.GetPropVal("districts");
                    cityGroups = result.GetPropVal("cityGroups");
                    result = result.GetPropVal("result");
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

            return Ok(
                    new
                    {
                        result = result,
                        regions = regions,
                        districts = districts,
                        cityGroups = cityGroups,
                        count = count,
                        error = error
                    });
        }

        [HttpPost]
        public async Task<IActionResult> SaveCityDetails([FromBody]JObject param)
        {
            object result = null;
            object error = null;

            CityDetail[] cityDetails;

            try
            {
                if (param != null)
                {
                    cityDetails = param.JGetPropVal<CityDetail[]>("cityDetails");

                    if (cityDetails != null
                        && cityDetails.Length > 0)
                    {
                        for (int i = 0; i < cityDetails.Length; i++)
                        {
                            cityDetails[i] = await _repo.Execute<CityDetail>("ManageCityDetail", cityDetails[i]);
                        }

                        result = cityDetails.Select(l => l == null ? null : l.Simplify()).ToArray();
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
                cityDetails = null;
            }

            return Ok(new { result = result, error = error });
        }


        [HttpPost]
        public async Task<IActionResult> GetRegions([FromBody]JObject param)
        {
            object result = null;
            object country = null;
            object error = null;

            int count = 0;

            try
            {
                country = await _repo.Execute<object>("GetDefaultCountry", true);


                int limit = 0;
                int offset = 0;

                string name = null;

                bool getTotalRegions = false;

                if (param != null)
                {
                    limit = param.GetPropVal<int>("limit");
                    offset = param.GetPropVal<int>("offset");

                    name = param.GetPropVal<string>("name");

                    getTotalRegions = param.GetPropVal<bool>("getTotalRegions");
                }


                result = await GetRegions(limit, offset, name, getTotalRegions);

                if (result != null)
                {
                    count = result.GetPropVal<int>("count");
                    result = result.GetPropVal("result");
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


        [HttpPost]
        public async Task<IActionResult> GetDistricts([FromBody]JObject param)
        {
            object result = null;
            object country = null;
            object error = null;

            int count = 0;

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


                result = await GetDistricts(limit, offset, name, getTotalDistricts);

                if (result != null)
                {
                    count = result.GetPropVal<int>("count");
                    result = result.GetPropVal("result");
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

                        result = districts.Select(l => l == null ? null : l.Simplify()).ToArray();
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


        [HttpPost]
        public async Task<IActionResult> GetCityGroups([FromBody]JObject param)
        {
            object result = null;
            object country = null;
            object error = null;

            int count = 0;

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


                result = await GetCityGroups(limit, offset, name, getTotalCityGroups);

                if (result != null)
                {
                    count = result.GetPropVal<int>("count");
                    result = result.GetPropVal("result");
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

                        result = cityGroups.Select(l => l == null ? null : l.Simplify()).ToArray();
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
