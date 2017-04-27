using CoreFtp;
using CoreFtp.Enum;
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

            Address addr;

            object result = null;

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

                UrlManager.SetupApplicationHost(_config[$"UrlConfigs:{currentEnv}:webSite"]);


                long addrID = param.GetPropVal<long>("addrID");
                string addrUID = param.GetPropVal<string>("addrUID");

                addr = await _repo.Execute<Address>("GetAddress", addrID, addrUID, null, 0, 0, 0, true);

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


                if (addr.Files != null
                    && addr.Files.Count > 0)
                {
                    string ftpHost = _config[$"FtpAccess:{currentEnv}:host"];
                    string ftpUserName = _config[$"FtpAccess:{currentEnv}:user"];
                    string ftppassword = _config[$"FtpAccess:{currentEnv}:password"];
                    bool ftpIgnoreCertificateErrors = true;


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

                        foreach (var af in addr.Files)
                        {
                            string afPath = af.GetFtpSource(addr.ClientUID, addr.UID, optimizedMedia: true);

                            if (!await ftpClient.FileExistsASync(afPath))
                            {
                                addrExceptions.Add(new AddrException()
                                {
                                    Status = AddressVerifiedStatus.FileMissing,
                                    Data = new { type = af.Type.ToString(), File = af.Simplify(false) }
                                });
                            }
                            else
                            {
                                switch (af.Type)
                                {
                                    case AddressFileType.Images:
                                    case AddressFileType.Logos:

                                        ImageSharp.Image img;

                                        try
                                        {
                                            using (var afStream = await ftpClient.OpenFileReadStreamAsync(afPath))
                                            {
                                                img = ImageSharp.Image.Load(afStream);

                                                img = (img.Width > 0 && img.Height > 0) ? img : null;
                                            }
                                        }
                                        catch
                                        {
                                            img = null;
                                        }

                                        if (img == null)
                                        {
                                            addrExceptions.Add(new AddrException()
                                            {
                                                Status = AddressVerifiedStatus.InvalidFile,
                                                Data = new { type = af.Type.ToString(), File = af.Simplify(false) }
                                            });
                                        }
                                        else
                                        {
                                            foreach (var size in UrlManager.GetThumnailSizes)
                                            {
                                                string thumnailPath = af.GetFtpThumbnailSource(addr.ClientUID, addr.UID, size.Width, size.Height);

                                                if (!await ftpClient.FileExistsASync(thumnailPath))
                                                {
                                                    addrExceptions.Add(new AddrException()
                                                    {
                                                        Status = AddressVerifiedStatus.ThumbnailMissing,
                                                        Data = new { type = af.Type.ToString(), file = af.Simplify(false), th = size.ToString() }
                                                    });
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        using (var thStream = await ftpClient.OpenFileReadStreamAsync(thumnailPath))
                                                        {
                                                            img = ImageSharp.Image.Load(thStream);

                                                            img = (img.Width > 0 && img.Height > 0) ? img : null;
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        img = null;
                                                    }

                                                    if (img == null)
                                                    {
                                                        addrExceptions.Add(new AddrException()
                                                        {
                                                            Status = AddressVerifiedStatus.InvalidThumbnail,
                                                            Data = new { type = af.Type.ToString(), file = af.Simplify(false), th = size.ToString() }
                                                        });
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }

                    if (addr != null
                        && addrExceptions != null
                        && addrExceptions.Count > 0)
                    {
                        result = (from ex in addrExceptions
                                  group ex by ex.Status into grp
                                  select new
                                  {
                                      Name = addr.Name,
                                      Slug = addr.Slug,
                                      Status = grp.Key,
                                      Data = grp.Select(l => l.Data).ToArray()
                                  }).ToArray();
                    }
                    else
                    {
                        result = new object[0];
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
                addr = null;

                if (addrExceptions != null)
                {
                    addrExceptions.Clear();
                    addrExceptions = null;
                }
            }

            return Ok(new { exceptions = result });
        }

        [HttpPost]
        public async Task<IActionResult> GenerateFile([FromBody]JObject param)
        {
            AddressFile addrFile;

            int[] size;

            try
            {
                if (param == null) throw new NullReferenceException(nameof(param));


                long addrFileID = param.GetPropVal<long>("addrFileID");
                string dimensions = param.GetPropVal<string>("dimensions");
                bool optz = param.GetPropVal<bool>("optimizedMedia");

                if (addrFileID <= 0) throw new NullReferenceException("ID");


                addrFile = await _repo.Execute<AddressFile>("GetAddressFile", addrFileID, true);

                if (addrFile == null) throw new NullReferenceException(nameof(addrFile));


                string fileFtpPath;

                if (string.IsNullOrEmpty(dimensions))
                {
                    fileFtpPath = addrFile.GetFtpSource(addrFile.Address.Client.UID, addrFile.Address.UID, optimizedMedia: optz);
                }
                else
                {
                    size = dimensions.Split('x').Select(l => int.Parse(l)).ToArray();

                    if (size.Length != 2) throw new NullReferenceException("dimensions");


                    fileFtpPath = addrFile.GetFtpThumbnailSource(addrFile.Address.Client.UID, addrFile.Address.UID, size[0], size[1]);
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
                addrFile = null;
                size = null;
            }

            return Ok();
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

            Func<JObject, Address> func;

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
                    func = (jobj) =>
                    {
                        if (jobj == null) return null;


                        var addr = Helper.JSonCamelDeserializeObject<Address>(jobj);


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

                        foreach (var addr in addresses.OfType<Address>())
                        {
                            object temp = await _repo.Execute("ManageAddressGetFullContent", addr);

                            tempLst.Add(temp);
                            temp = null;

                            if (addr.Files != null
                                && addr.Files.Count > 0)
                            {
                                foreach (var af in addr.Files.Where(l => l.RecordState == RecordState.Deleted))
                                {
                                    await DeleteFtpFile(af, addr.ClientUID, addr.UID);
                                }
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

                                foreach (var af in filesToSave)
                                {
                                    formFile = af.GetFile<IFormFile>();

                                    if (formFile != null)
                                    {
                                        using (var readStream = formFile.OpenReadStream())
                                        {
                                            string ftpPath = af.GetFtpSource(addr.ClientUID, addr.UID);

                                            using (var writeStream = await ftpClient.OpenFileWriteStreamAsync(ftpPath))
                                            {
                                                await readStream.CopyToAsync(writeStream);
                                            }
                                        }

                                        switch (af.Type)
                                        {
                                            case AddressFileType.Images:
                                            case AddressFileType.Logos:

                                                using (var readStream = await DownloadOptzAddressFileASync(af, addr.ClientUID, addr.UID))
                                                {
                                                    string ftpPath = af.GetFtpSource(addr.ClientUID, addr.UID, optimizedMedia: true);

                                                    using (var writeStream = await ftpClient.OpenFileWriteStreamAsync(ftpPath))
                                                    {
                                                        await readStream.CopyToAsync(writeStream);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
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

        private async Task DeleteFtpFile(AddressFile file, string clientUID, string addrUID)
        {
            try
            {
                if (file == null) return;


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

                    string ftpPath = file.GetFtpSource(clientUID, addrUID);

                    try
                    {
                        await ftpClient.DeleteFileAsync(ftpPath);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
