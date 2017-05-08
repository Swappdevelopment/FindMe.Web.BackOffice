using CoreFtp;
using CoreFtp.Enum;
using CoreFtp.Infrastructure;
using FindMe.Data;
using FindMe.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Swapp.Data;
using System;
using System.Collections.Generic;
using System.IO;
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


        [HttpGet]
        public async Task<IActionResult> GetAllAddressIDs()
        {
            long[] ids = null;

            try
            {
                ids = await _repo.Execute<long[]>("GetAllAddressIDs");
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

            return Ok(new { result = ids });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyAddress([FromBody]JObject param)
        {
            List<AddrException> addrExceptions = null;

            object[] result = null;

            JObject[] jobjs;

            object lockObj;

            try
            {
                if (param == null) throw new NullReferenceException(nameof(param));


                bool skipFilesVerification = param.GetPropVal<bool>("skipFilesVerification");
                jobjs = param.JGetPropVal<JObject[]>("addresses");

                if (jobjs == null
                    || jobjs.Length == 0)
                {
                    jobjs = new JObject[] { param };
                }

                string currentEnv = "production";

                if (_env.IsDevelopment())
                {
                    currentEnv = "development";
                }
                else if (_config.IsPublishEnvStaging())
                {
                    currentEnv = "staging";
                }

                UrlManager.SetupApplicationHost(_config[$"UrlConfigs:{currentEnv}:webSite"]);


                result = new object[0];

                lockObj = new object();


                string ftpHost = _config[$"FtpAccess:{currentEnv}:host"];
                string ftpUserName = _config[$"FtpAccess:{currentEnv}:user"];
                string ftppassword = _config[$"FtpAccess:{currentEnv}:password"];
                bool ftpIgnoreCertificateErrors = true;


                await Task.WhenAll(
                            from j in jobjs
                            select Helper.GetFunc<JObject, Task>(
                                async (jobj) =>
                                {
                                    long addrID = jobj.GetPropVal<long>("addrID");
                                    string addrUID = jobj.GetPropVal<string>("addrUID");

                                    Address addr = await _repo.Execute<Address>("GetAddress", addrID, addrUID, null, null, 0, 0, 0, true);

                                    if (addr == null) throw new NullReferenceException(nameof(addr));


                                    addrExceptions = new List<AddrException>();


                                    int slugCount = await _repo.Execute<int>("GetAddressCountFromSlug", addr.Slug);

                                    if (slugCount > 1)
                                    {
                                        addrExceptions.Add(new AddrException()
                                        {
                                            Status = AddressVerifiedStatus.MultipleSlug,
                                            Data = new { AddSlugsCount = (slugCount - 1) }
                                        });
                                    }


                                    if (!skipFilesVerification
                                        && addr.Files != null
                                        && addr.Files.Count > 0)
                                    {
                                        await Task.WhenAll(
                                            from f in addr.Files
                                            select Helper.GetFunc<AddressFile, Task>(async af =>
                                            {
                                                using (var ftpClient = new FtpClient(
                                                                    new FtpClientConfiguration()
                                                                    {
                                                                        Host = ftpHost,
                                                                        Username = ftpUserName,
                                                                        Password = ftppassword,
                                                                        EncryptionType = FtpEncryption.Implicit,
                                                                        IgnoreCertificateErrors = ftpIgnoreCertificateErrors
                                                                    }))
                                                {
                                                    await ftpClient.LoginAsync();

                                                    string afPath = af.GetFtpSource(addr.ClientUID, addr.UID, optimizedMedia: false);

                                                    if (await ftpClient.FileExistsASync(afPath))
                                                    {
                                                        switch (af.Type)
                                                        {
                                                            case AddressFileType.Images:
                                                            case AddressFileType.Logos:

                                                                afPath = af.GetFtpSource(addr.ClientUID, addr.UID, optimizedMedia: true);

                                                                if (!await ftpClient.FileExistsASync(afPath))
                                                                {
                                                                    addrExceptions?.Add(new AddrException()
                                                                    {
                                                                        Status = AddressVerifiedStatus.OptzFileMissing,
                                                                        Data = new { type = af.Type.ToString(), File = af.Simplify(false) }
                                                                    });
                                                                }

                                                                foreach (var size in UrlManager.GetThumnailSizes)
                                                                {
                                                                    string thumnailPath = af.GetFtpThumbnailSource(addr.ClientUID, addr.UID, size.Width, size.Height);

                                                                    if (!await ftpClient.FileExistsASync(thumnailPath))
                                                                    {
                                                                        addrExceptions?.Add(new AddrException()
                                                                        {
                                                                            Status = AddressVerifiedStatus.ThumbnailMissing,
                                                                            Data = new { type = af.Type.ToString(), file = af.Simplify(false), th = size.ToString() }
                                                                        });
                                                                    }
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        addrExceptions?.Add(new AddrException()
                                                        {
                                                            Status = AddressVerifiedStatus.FileMissing,
                                                            Data = new { type = af.Type.ToString(), File = af.Simplify(false) }
                                                        });
                                                    }
                                                }
                                            })(f));
                                    }


                                    if (addr != null
                                        && addrExceptions != null
                                        && addrExceptions.Count > 0)
                                    {
                                        lock (lockObj)
                                        {
                                            result = result.Concat(from ex in addrExceptions
                                                                   group ex by ex.Status into grp
                                                                   select new
                                                                   {
                                                                       ID = addr.ID,
                                                                       Name = addr.Name,
                                                                       Slug = addr.Slug,
                                                                       Status = grp.Key,
                                                                       Data = grp.Select(l => l.Data).ToArray()
                                                                   }).ToArray();
                                        }
                                    }
                                })(j));


                return Ok(new { exceptions = result });
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
                lockObj = null;

                result = null;

                if (addrExceptions != null)
                {
                    addrExceptions.Clear();
                    addrExceptions = null;
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> GenerateAddressFile([FromBody]JObject param)
        {
            Dictionary<string, object> errors = null;

            try
            {
                if (param == null) throw new NullReferenceException(nameof(param));


                string currentEnv = "production";

                if (_env.IsDevelopment())
                {
                    currentEnv = "development";
                }
                else if (_config.IsPublishEnvStaging())
                {
                    currentEnv = "staging";
                }

                string ftpHost = _config[$"FtpAccess:{currentEnv}:host"];
                string ftpUserName = _config[$"FtpAccess:{currentEnv}:user"];
                string ftppassword = _config[$"FtpAccess:{currentEnv}:password"];
                bool ftpIgnoreCertificateErrors = true;


                errors = new Dictionary<string, object>();

                await _repo.VerifyLoginToken();

                await Task.WhenAll(
                    from obj in param.JGetPropVal<JObject[]>("params")
                                                     .Select(j => new
                                                     {
                                                         id = j.GetPropVal<long>("addrFileID"),
                                                         jobj = j
                                                     })
                    group obj by obj.id into grp
                    select Helper.GetFunc<long, IEnumerable<JObject>, Task>(async (afID, jobjs) =>
                    {
                        var af = await _repo.Execute<AddressFile>("GetAddressFile", afID, null, true);

                        await Task.WhenAll(jobjs.Select(j =>
                            Helper.GetFunc<JObject, AddressFile, Task>(async (jobj, addrFile) =>
                            {
                                string uid = jobj.JGetPropVal<string>("uid");

                                int[] size;
                                byte[] bArr;

                                try
                                {
                                    if (addrFile == null) throw new NullReferenceException("AddressFile");


                                    using (var readFtpClient = new FtpClient(
                                        new FtpClientConfiguration()
                                        {
                                            Host = ftpHost,
                                            Username = ftpUserName,
                                            Password = ftppassword,
                                            EncryptionType = FtpEncryption.Implicit,
                                            IgnoreCertificateErrors = ftpIgnoreCertificateErrors
                                        }))
                                    {
                                        await readFtpClient.LoginAsync();

                                        using (var writeFtpClient = new FtpClient(
                                                                       new FtpClientConfiguration()
                                                                       {
                                                                           Host = ftpHost,
                                                                           Username = ftpUserName,
                                                                           Password = ftppassword,
                                                                           EncryptionType = FtpEncryption.Implicit,
                                                                           IgnoreCertificateErrors = ftpIgnoreCertificateErrors
                                                                       }))
                                        {
                                            string addrFileFormat = jobj.JGetPropVal<string>("addrFileFormat");
                                            string dimensions = jobj.JGetPropVal<string>("dimensions");

                                            if (string.IsNullOrEmpty(dimensions)) throw new NullReferenceException("Dimensions");


                                            await writeFtpClient.LoginAsync();

                                            string optzFileFtpPath = addrFile.GetFtpSource(addrFile.Address.Client.UID, addrFile.Address.UID, optimizedMedia: true);

                                            if (!await readFtpClient.FileExistsASync(optzFileFtpPath))
                                            {
                                                string filePath = addrFile.GetFtpSource(addrFile.Address.Client.UID, addrFile.Address.UID, optimizedMedia: false);

                                                using (var readStream = await readFtpClient.OpenFileReadStreamAsync(filePath))
                                                {
                                                    using (var optzStream = ImageManip.OptimizeImage(readStream, 0, 0, 0, resize: false))
                                                    {
                                                        using (var writeStream = await writeFtpClient.OpenFileWriteStreamAsync(optzFileFtpPath))
                                                        {
                                                            optzStream.CopyTo(writeStream);
                                                        }
                                                    }
                                                }
                                            }

                                            size = dimensions.Split('x').Select(l => int.Parse(l)).ToArray();

                                            if (size.Length != 2) throw new NullReferenceException("dimensions");


                                            string thFtpPath = addrFile.GetFtpThumbnailSource(addrFile.Address.Client.UID, addrFile.Address.UID, size[0], size[1]);

                                            using (var readStream = await readFtpClient.OpenFileReadStreamAsync(optzFileFtpPath))
                                            {
                                                using (var thStream = ImageManip.OptimizeImage(readStream, 0, size[0], size[1]))
                                                {
                                                    using (var writeStream = await writeFtpClient.OpenFileWriteStreamAsync(thFtpPath))
                                                    {
                                                        bArr = thStream.ToArray();

                                                        await writeStream.WriteAsync(bArr, 0, bArr.Length);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (errors != null)
                                    {
                                        errors.Add(uid, ex.Message);
                                    }
                                }
                                finally
                                {
                                    size = null;
                                }
                            })(j, af)));
                    })(grp.Key, grp.Select(l => l.jobj)));

                return Ok(new { errors = errors.Select(e => new { uid = e.Key, error = e.Value }).ToArray() });
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
                if (errors != null)
                {
                    errors.Clear();
                    errors = null;
                }
            }
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
                string slug = null;
                string type = null;

                bool getTotalAddresses = false;

                if (param != null)
                {
                    limit = param.GetPropVal<int>("limit");
                    offset = param.GetPropVal<int>("offset");
                    name = param.GetPropVal<string>("name");
                    type = param.GetPropVal<string>("type");

                    addressId = param.GetPropVal<long>("addressId");
                    categoryId = param.GetPropVal<long>("categoryId");
                    cityId = param.GetPropVal<long>("cityId");

                    getRefClients = param.GetPropVal<bool>("getRefClients");
                    getRefCatgs = param.GetPropVal<bool>("getRefCatgs");
                    getRefCities = param.GetPropVal<bool>("getRefCities");

                    getTotalAddresses = param.GetPropVal<bool>("getTotalAddresses");

                    switch (type ?? "")
                    {
                        case "slug":

                            slug = name;
                            name = null;
                            break;
                        case "id":

                            if (long.TryParse(name, out addressId))
                            {
                                name = null;
                            }
                            break;
                    }
                }

                taskArr = new List<Func<Task>>()
                {
                    async () =>
                    {
                        result = (await _repo.Execute<Address[]>("GetAddresses", addressId, "", name, slug, 0, categoryId, cityId, false, false, false, false, limit, offset))
                                                .Select(l => l.Simplify(withCollections: false)).ToArray();
                    }
                };

                if (getTotalAddresses)
                {
                    taskArr.Add(async () =>
                                 {
                                     count = await _repo.Execute<int>("GetAddressesCount", addressId, "", name, slug, 0, categoryId, cityId);
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
                    if (_env != null
                        && _config != null)
                    {
                        if (_env.IsDevelopment())
                        {
                            UrlManager.SetupApplicationHost(_config["UrlConfigs:development:webSite"]);
                        }
                        else if (_config.IsPublishEnvStaging())
                        {
                            UrlManager.SetupApplicationHost(_config["UrlConfigs:staging:webSite"]);
                        }
                        else
                        {
                            UrlManager.SetupApplicationHost(_config["UrlConfigs:production:webSite"]);
                        }
                    }

                    addrID = param.GetPropVal<long>("addrID");

                    result = await _repo.Execute<object>("GetAddressContent", addrID, null, false);
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

            object[] addresses;

            List<object> tempLst = null;

            Language[] langs;

            Func<JObject, Address> func;

            Func<AddressFile, string, string, AddressFile> funcSetUIDs;

            try
            {
                if (_env.IsDevelopment())
                {
                    UrlManager.SetupApplicationHost(_config["UrlConfigs:development:webSite"]);
                }
                else if (_config.IsPublishEnvStaging())
                {
                    UrlManager.SetupApplicationHost(_config["UrlConfigs:staging:webSite"]);
                }
                else
                {
                    UrlManager.SetupApplicationHost(_config["UrlConfigs:production:webSite"]);
                }

                if (param != null)
                {
                    langs = await _repo.Execute<Language[]>("GetLanguages", null, SqlSearchMode.Equals);

                    if (langs == null) throw new NullReferenceException(nameof(Language));


                    func = (jobj) =>
                    {
                        if (jobj == null) return null;


                        var addr = Helper.JSonCamelDeserializeObject<Address>(jobj);

                        addr.LangDescs = addr.LangDescs == null ? new ObjectCollection<Address_LangDesc>() : addr.LangDescs;

                        string desc = jobj.JGetPropVal<string>("desc_en");

                        var lang = langs.FirstOrDefault(l => l.Code != null && l.Code.ToLower().StartsWith("en"));

                        if (lang != null)
                        {
                            addr.LangDescs.Add(
                                new Address_LangDesc()
                                {
                                    Address_Id = addr.ID,
                                    Language_Id = lang.ID,
                                    Value = DescData.ToBlankDescDataJson(desc)
                                });
                        }

                        desc = jobj.JGetPropVal<string>("desc_fr");

                        lang = langs.FirstOrDefault(l => l.Code != null && l.Code.ToLower().StartsWith("fr"));

                        if (lang != null)
                        {
                            addr.LangDescs.Add(
                                new Address_LangDesc()
                                {
                                    Address_Id = addr.ID,
                                    Language_Id = lang.ID,
                                    Value = DescData.ToBlankDescDataJson(desc)
                                });
                        }


                        var jFiles = new List<JObject>();


                        var jObjects = jobj.JGetPropVal<JObject[]>("images");

                        if (jObjects != null)
                        {
                            jFiles.AddRange(jObjects);
                        }

                        jObjects = jobj.JGetPropVal<JObject[]>("logos");

                        if (jObjects != null)
                        {
                            jFiles.AddRange(jObjects);
                        }

                        jObjects = jobj.JGetPropVal<JObject[]>("documents");

                        if (jObjects != null)
                        {
                            jFiles.AddRange(jObjects);
                        }

                        if (jFiles.Count > 0)
                        {
                            addr.Files = addr.Files == null ? new ObjectCollection<AddressFile>() : addr.Files;

                            foreach (var jImg in jFiles
                                                    .Where(j =>
                                                        !j.JGetPropVal<bool>("waitingUpload")
                                                        || ((RecordState)j.JGetPropVal<int>("recordState")) == RecordState.Deleted))
                            {
                                addr.Files.Add(Helper.JSonCamelDeserializeObject<AddressFile>(jImg));
                            }
                        }

                        jFiles.Clear();
                        jFiles = null;

                        jObjects = jobj.JGetPropVal<JObject[]>("featureds");

                        if (jObjects != null)
                        {
                            addr.IsFeatureds = new ObjectCollection<AddressIsFeatured>(
                                jObjects.Select(l => Helper.JSonCamelDeserializeObject<AddressIsFeatured>(l)));
                        }


                        jObjects = jobj.JGetPropVal<JObject[]>("contacts");

                        if (jObjects != null)
                        {
                            addr.Contacts = new ObjectCollection<AddressContact>(
                                                 jObjects
                                                     .SelectMany(j => j.JGetPropVal<JObject[]>("contacts"))
                                                     .Select(j => Helper.JSonCamelDeserializeObject<AddressContact>(j)));
                        }


                        jObjects = jobj.JGetPropVal<JObject[]>("openHours");

                        if (jObjects != null)
                        {
                            addr.DaysOpen = new ObjectCollection<DayOpen>();

                            foreach (var jday in jObjects)
                            {
                                var jDayRows = jday.JGetPropVal<JObject[]>("openHours");

                                if (jDayRows != null
                                    && jDayRows.Length > 0)
                                {
                                    var dayEnum = (WeekDay)jday.JGetPropVal<int>("day");

                                    foreach (var dayRow in jDayRows)
                                    {
                                        addr.DaysOpen.Add(new DayOpen()
                                        {
                                            RecordState = (RecordState)dayRow.JGetPropVal<int>("recordState"),
                                            Address_Id = addr.ID,
                                            Day = dayEnum,
                                            ID = dayRow.JGetPropVal<long>("id"),
                                            HoursFrom = dayRow.JGetPropVal<short>("hrFrom"),
                                            HoursTo = dayRow.JGetPropVal<short>("hrTo"),
                                            MinutesFrom = dayRow.JGetPropVal<short>("minFrom"),
                                            MinutesTo = dayRow.JGetPropVal<short>("minTo"),
                                            Status = (ModelStatus)dayRow.JGetPropVal<int>("status"),
                                        });
                                    }
                                }
                            }
                        }

                        jObjects = null;

                        var tripAdWidget = jobj.JGetPropVal<AddressTripAdWidget>("tripAdWidget");

                        if (tripAdWidget != null)
                        {
                            addr.TripAdWidgets = new ObjectCollection<AddressTripAdWidget>()
                            {
                                tripAdWidget
                            };

                            tripAdWidget = null;
                        }

                        return addr;
                    };

                    addresses = param.JGetPropVal<JObject[]>("addresses");

                    if (addresses != null
                        && addresses.Length > 0)
                    {
                        addresses = addresses.Select(l => func(l as JObject)).ToArray();

                        tempLst = new List<object>();

                        funcSetUIDs = (af, clientUID, addrUID) =>
                        {
                            if (af != null)
                            {
                                af.Address = af.Address ?? new Address();
                                af.Address.UID = addrUID;

                                af.Address.Client = af.Address.Client ?? new Client();
                                af.Address.Client.UID = clientUID;
                            }


                            return af;
                        };

                        foreach (var addr in addresses.OfType<Address>())
                        {
                            object temp = await _repo.Execute("ManageAddressGetFullContent", addr);

                            tempLst.Add(temp);
                            temp = null;

                            if (addr.RecordState == RecordState.Deleted)
                            {
                                await DeleteAddrFiles(addr.ID, null);
                            }
                            else if (addr.Files != null
                               && addr.Files.Count > 0)
                            {
                                await DeleteAddrFiles(
                                            0,
                                            addr.Files
                                                .Where(l => l.RecordState == RecordState.Deleted)
                                                .Select(l => funcSetUIDs(l, addr.ClientUID, addr.UID))
                                                .ToArray());
                            }
                        }

                        result = tempLst.ToArray();
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
                addresses = null;

                if (tempLst != null)
                {
                    tempLst.Clear();
                    tempLst = null;
                }
            }

            return Ok(new { result = result, error = error });
        }


        [HttpPost]
        public async Task<IActionResult> UploadAddressFiles(string data = null)
        {
            Address[] addresses;

            AddressFile[] filesToSave;

            IFormFile formFile;

            try
            {
                if (Request != null
                    && Request.Form != null
                    && Request.Form.Keys != null
                    && Request.Form.Keys.Count > 0
                    && Request.Form != null
                    && Request.Form.Files != null
                    && Request.Form.Keys.Contains("address")
                    && Request.Form.Files.Count == Request.Form.Keys.Count(l => !l.StartsWith("address")))
                {
                    string currentEnv = "production";

                    if (_env.IsDevelopment())
                    {
                        currentEnv = "development";
                    }
                    else if (_config.IsPublishEnvStaging())
                    {
                        currentEnv = "staging";
                    }

                    UrlManager.SetupApplicationHost(_config[$"UrlConfigs:{currentEnv}:webSite"]);


                    string ftpHost = _config[$"FtpAccess:{currentEnv}:host"];
                    string ftpUserName = _config[$"FtpAccess:{currentEnv}:user"];
                    string ftppassword = _config[$"FtpAccess:{currentEnv}:password"];
                    bool ftpIgnoreCertificateErrors = true;


                    addresses = (from k in Request.Form.Keys
                                 where k != null
                                 && k.StartsWith("address")
                                 select Helper.JSonCamelDeserializeObject<Address>(Request.Form[k])).ToArray();

                    foreach (var addr in addresses.Where(a => a != null && !string.IsNullOrEmpty(a.UID)))
                    {
                        addr.Files = new ObjectCollection<AddressFile>(
                                        from k in Request.Form.Keys
                                        where k != null
                                        && k.StartsWith(addr.UID)
                                        select Helper.JSonDeserializeObject<AddressFile>(Request.Form[k][0])
                                            .SetFile(Request.Form.Files.FirstOrDefault(f => f.Name == k)));


                        addr.Files = new ObjectCollection<AddressFile>(await _repo.Execute<AddressFile[]>("ManageAddressFiles",
                                                                                    addr.UID,
                                                                                    false,
                                                                                    addr.Files,
                                                                                    null,
                                                                                    null,
                                                                                    true));

                        filesToSave = addr.Files.Where(l => l.HasFile()).ToArray();

                        if (filesToSave.Length > 0)
                        {
                            await Task.WhenAll(
                                from fs in filesToSave
                                select Helper.GetFunc<AddressFile, Task>(
                                    async af =>
                                    {
                                        try
                                        {
                                            using (var writeFtpClient = new FtpClient(
                                                    new FtpClientConfiguration()
                                                    {
                                                        Host = ftpHost,
                                                        Username = ftpUserName,
                                                        Password = ftppassword,
                                                        EncryptionType = FtpEncryption.Implicit,
                                                        IgnoreCertificateErrors = ftpIgnoreCertificateErrors
                                                    }))
                                            {
                                                await writeFtpClient.LoginAsync();




                                                formFile = af.GetFile<IFormFile>();

                                                if (formFile != null)
                                                {
                                                    string ftpFilePath;

                                                    using (var fileStream = formFile.OpenReadStream())
                                                    {
                                                        ftpFilePath = af.GetFtpSource(addr.ClientUID, addr.UID, optimizedMedia: false);

                                                        using (var writeStream = await writeFtpClient.OpenFileWriteStreamAsync(ftpFilePath))
                                                        {
                                                            await fileStream.CopyToAsync(writeStream);
                                                        }
                                                    }

                                                    switch (af.Type)
                                                    {
                                                        case AddressFileType.Images:
                                                        case AddressFileType.Logos:

                                                            using (var readFtpClient = new FtpClient(
                                                                                            new FtpClientConfiguration()
                                                                                            {
                                                                                                Host = ftpHost,
                                                                                                Username = ftpUserName,
                                                                                                Password = ftppassword,
                                                                                                EncryptionType = FtpEncryption.Implicit,
                                                                                                IgnoreCertificateErrors = ftpIgnoreCertificateErrors
                                                                                            }))
                                                            {
                                                                await readFtpClient.LoginAsync();


                                                                string optzFtpPath;

                                                                using (var fileFtpReadStream = await writeFtpClient.OpenFileReadStreamAsync(ftpFilePath))
                                                                {
                                                                    using (var optzStream = ImageManip.OptimizeImage(fileFtpReadStream, 75, 0, 0, resize: false, compress: true))
                                                                    {
                                                                        optzFtpPath = af.GetFtpSource(addr.ClientUID, addr.UID, optimizedMedia: true);

                                                                        using (var writeStream = await writeFtpClient.OpenFileWriteStreamAsync(optzFtpPath))
                                                                        {
                                                                            var bArr = optzStream.ToArray();
                                                                            await writeStream.WriteAsync(bArr, 0, bArr.Length);
                                                                            bArr = null;

                                                                        }

                                                                        long fileSize = await readFtpClient.GetFileSizeAsync(ftpFilePath);
                                                                        long optzFileSize = await readFtpClient.GetFileSizeAsync(optzFtpPath);

                                                                        if (optzFileSize > fileSize)
                                                                        {
                                                                            await writeFtpClient.DeleteFileAsync(optzFtpPath);

                                                                            using (var fileStream = formFile.OpenReadStream())
                                                                            {
                                                                                using (var writeStream = await writeFtpClient.OpenFileWriteStreamAsync(optzFtpPath))
                                                                                {
                                                                                    await fileStream.CopyToAsync(writeStream);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }


                                                                await Task.WhenAll(
                                                                            from sz in UrlManager.GetThumnailSizes
                                                                            select Helper.GetFunc<SwpSize, string, Task>(
                                                                                async (size, optzPath) =>
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        using (var thWriteFtpClient = new FtpClient(
                                                                                                                       new FtpClientConfiguration()
                                                                                                                       {
                                                                                                                           Host = ftpHost,
                                                                                                                           Username = ftpUserName,
                                                                                                                           Password = ftppassword,
                                                                                                                           EncryptionType = FtpEncryption.Implicit,
                                                                                                                           IgnoreCertificateErrors = ftpIgnoreCertificateErrors
                                                                                                                       }))
                                                                                        {
                                                                                            await thWriteFtpClient.LoginAsync();

                                                                                            string rszFtpPath = af.GetFtpThumbnailSource(addr.ClientUID, addr.UID, size.Width, size.Height);

                                                                                            using (var thReadFtpClient = new FtpClient(
                                                                                                                       new FtpClientConfiguration()
                                                                                                                       {
                                                                                                                           Host = ftpHost,
                                                                                                                           Username = ftpUserName,
                                                                                                                           Password = ftppassword,
                                                                                                                           EncryptionType = FtpEncryption.Implicit,
                                                                                                                           IgnoreCertificateErrors = ftpIgnoreCertificateErrors
                                                                                                                       }))
                                                                                            {
                                                                                                await thReadFtpClient.LoginAsync();

                                                                                                using (var readOptzStream = await thWriteFtpClient.OpenFileReadStreamAsync(optzPath))
                                                                                                {
                                                                                                    using (var rszStream = ImageManip.OptimizeImage(
                                                                                                                                    readOptzStream, 0, sz.Width, sz.Height,
                                                                                                                                    resize: true, compress: false))
                                                                                                    {
                                                                                                        using (var writeStream = await thWriteFtpClient.OpenFileWriteStreamAsync(rszFtpPath))
                                                                                                        {
                                                                                                            var bArr = rszStream.ToArray();
                                                                                                            await writeStream.WriteAsync(bArr, 0, bArr.Length);
                                                                                                            bArr = null;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        throw ex;
                                                                                    }
                                                                                })(sz, optzFtpPath));

                                                                break;
                                                            }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    })(fs));
                        }
                    }


                    return Ok(
                            new
                            {
                                result = addresses.Select(a => new
                                {
                                    id = a.ID,
                                    uid = a.UID,
                                    documents = a.Files
                                                    .Where(l => l.Type == AddressFileType.Documents)
                                                    .Select(l => l.SimplifyWithUrls(a.ClientUID, a.UID)).ToArray(),
                                    logos = a.Files
                                                .Where(l => l.Type == AddressFileType.Logos)
                                                .Select(l => l.SimplifyWithUrls(
                                                                    a.ClientUID,
                                                                    a.UID,
                                                                    thWidth: UrlManager.BO_LISTING_WIDTH,
                                                                    thHeight: UrlManager.BO_LISTING_HEIGHT)).ToArray(),
                                    images = a.Files
                                                .Where(l => l.Type == AddressFileType.Images)
                                                .Select(l => l.SimplifyWithUrls(
                                                                    a.ClientUID,
                                                                    a.UID,
                                                                    thWidth: UrlManager.BO_LISTING_WIDTH,
                                                                    thHeight: UrlManager.BO_LISTING_HEIGHT)).ToArray()
                                }).ToArray()
                            });
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                addresses = null;
                filesToSave = null;
            }
        }


        private async Task<Stream> DownloadOptzAddressFileASync(AddressFile af, string clientUID, string addrUID)
        {
            Stream result = null;
            string url;

            try
            {
                if (af == null) throw new NullReferenceException();


                url = UrlManager.GetToBeOptimizedUrl(clientUID, addrUID, af.UID, af.Format, af.Type);

                if (string.IsNullOrEmpty(url)) throw new NullReferenceException();


                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                    result = await httpClient.GetStreamAsync(new Uri(url, UriKind.Absolute));


                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                result = null;
            }
        }


        private async Task DeleteAddrFiles(long addrID, AddressFile[] addrFiles)
        {
            try
            {
                if (addrID > 0)
                {
                    if (addrID > 0)
                    {
                        addrFiles = await _repo.Execute<AddressFile[]>("GetAddressFiles", 0, null, addrID, null, 0, null, true, 0, 0);
                    }

                    if (addrFiles != null
                        && addrFiles.Length > 0)
                    {
                        string currentEnv = "production";

                        if (_env.IsDevelopment())
                        {
                            currentEnv = "development";
                        }
                        else if (_config.IsPublishEnvStaging())
                        {
                            currentEnv = "staging";
                        }

                        string ftpHost = _config[$"FtpAccess:{currentEnv}:host"];
                        string ftpUserName = _config[$"FtpAccess:{currentEnv}:user"];
                        string ftppassword = _config[$"FtpAccess:{currentEnv}:password"];
                        bool ftpIgnoreCertificateErrors = true;


                        await Task.WhenAll(
                            from f in addrFiles
                            select Helper.GetFunc<AddressFile, Task>(
                                async af =>
                                {
                                    if (af != null)
                                    {
                                        using (var readFtpClient = new FtpClient(
                                                                        new FtpClientConfiguration()
                                                                        {
                                                                            Host = ftpHost,
                                                                            Username = ftpUserName,
                                                                            Password = ftppassword,
                                                                            EncryptionType = FtpEncryption.Implicit,
                                                                            IgnoreCertificateErrors = ftpIgnoreCertificateErrors
                                                                        }))
                                        {
                                            await readFtpClient.LoginAsync();

                                            using (var writeFtpClient = new FtpClient(
                                                                           new FtpClientConfiguration()
                                                                           {
                                                                               Host = ftpHost,
                                                                               Username = ftpUserName,
                                                                               Password = ftppassword,
                                                                               EncryptionType = FtpEncryption.Implicit,
                                                                               IgnoreCertificateErrors = ftpIgnoreCertificateErrors
                                                                           }))
                                            {
                                                await writeFtpClient.LoginAsync();

                                                string path = af.GetFtpSource(af.Address.Client.UID, af.Address.UID, optimizedMedia: false);

                                                if (await readFtpClient.FileExistsASync(path))
                                                {
                                                    await writeFtpClient.DeleteFileAsync(path);
                                                }

                                                path = af.GetFtpSource(af.Address.Client.UID, af.Address.UID, optimizedMedia: true);

                                                if (await readFtpClient.FileExistsASync(path))
                                                {
                                                    await writeFtpClient.DeleteFileAsync(path);
                                                }
                                            }
                                        }

                                        await Task.WhenAll(
                                            from sz in UrlManager.GetThumnailSizes
                                            select Helper.GetFunc<AddressFile, SwpSize, Task>(
                                                async (addrFile, size) =>
                                                {
                                                    using (var readFtpClient = new FtpClient(
                                                                        new FtpClientConfiguration()
                                                                        {
                                                                            Host = ftpHost,
                                                                            Username = ftpUserName,
                                                                            Password = ftppassword,
                                                                            EncryptionType = FtpEncryption.Implicit,
                                                                            IgnoreCertificateErrors = ftpIgnoreCertificateErrors
                                                                        }))
                                                    {
                                                        await readFtpClient.LoginAsync();

                                                        using (var writeFtpClient = new FtpClient(
                                                                                       new FtpClientConfiguration()
                                                                                       {
                                                                                           Host = ftpHost,
                                                                                           Username = ftpUserName,
                                                                                           Password = ftppassword,
                                                                                           EncryptionType = FtpEncryption.Implicit,
                                                                                           IgnoreCertificateErrors = ftpIgnoreCertificateErrors
                                                                                       }))
                                                        {
                                                            await writeFtpClient.LoginAsync();

                                                            string thFtpPath = addrFile.GetFtpThumbnailSource(addrFile.Address.Client.UID, addrFile.Address.UID, size.Width, size.Height);

                                                            if (await readFtpClient.FileExistsASync(thFtpPath))
                                                            {
                                                                await writeFtpClient.DeleteFileAsync(thFtpPath);
                                                            }
                                                        }
                                                    }

                                                })(af, sz));
                                    }
                                })(f));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                addrFiles = null;
            }
        }
    }
}
