using FindMe.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swapp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class ApiBulkImportController : BaseController
    {
        public ApiBulkImportController(
            IConfigurationRoot config,
            WebDbRepository repo,
            IHostingEnvironment env,
            ILogger<ApiAccountController> logger,
            IMailService mailService)
            : base(config, repo, env, logger, mailService)
        {
        }


        private Encoding GetEncoding(Stream stream)
        {
            // Read the BOM
            var bom = new char[4];
            using (var file = new StreamReader(stream))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }


        private T GetColumnValue<T>(bool isRequired, string columnName, object value, object ifNull = null)
        {
            bool isNull = false;

            try
            {
                isNull = false;

                string strValue = value == null ? null : value.ToString().Trim();

                if (typeof(T) == typeof(string))
                {
                    value = strValue;
                }
                else if (typeof(T) == typeof(bool))
                {
                    try
                    {
                        value = string.IsNullOrEmpty(strValue) ? default(bool) : bool.Parse(strValue);
                    }
                    catch
                    {
                        int temp = string.IsNullOrEmpty(strValue) ? default(int) : int.Parse(strValue);
                        value = temp != 0;
                    }
                }
                else if (typeof(T) == typeof(double))
                {
                    value = string.IsNullOrEmpty(strValue) ? default(double) : double.Parse(strValue);
                }

                if (string.IsNullOrEmpty(strValue)
                    && ifNull != null && ifNull.GetType() == typeof(T))
                {
                    value = ifNull;

                    strValue = value.ToString();
                }

                if (string.IsNullOrEmpty(strValue) && isRequired == true)
                {
                    isNull = true;
                    throw new Exception();
                }

                return (T)value;
            }
            catch (Exception ex)
            {
                if (isNull)
                {
                    throw new Exception($"The {columnName} is required but is empty", ex);
                }
                else
                {
                    throw new Exception($"Error wile converting value for column {columnName}", ex);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadCSV()
        {
            string errorMessage = null;

            var addresses = new List<AddressCSV>();
            var lines = new List<string[]>();

            IFormFile fileAddrCSV;
            IFormFile fileTimeCSV;

            string[] header = null;

            DaysCsv[] daysCsv;

            PropertyInfo[] props;

            List<CategoryCSV> csvCategories = null;
            List<TagCSV> csvTags = null;

            CategoryCSV[] processedCsvCatgs = null;
            TagCSV[] processedCsvTags = null;

            try
            {
                fileAddrCSV = Request.Form.Files.FirstOrDefault(l => l.Name == "ADDR_CSV");
                fileTimeCSV = Request.Form.Files.FirstOrDefault(l => l.Name == "TIME_CSV");

                if (fileAddrCSV == null)
                    throw new NullReferenceException("ADDR_CSV");


                props = typeof(AddressCSV).GetProperties();

                using (var reader = new StreamReader(fileAddrCSV.OpenReadStream(), Encoding.UTF7))
                {

                    header = reader.ReadLine().Split(';');

                    if (header.Length != (props.Length + AddressCSV.ADDITIONAL_PROPERTIES_COUNT_COMPARE))
                    {
                        errorMessage = $"There are {header.Length} column instead of {props.Length + AddressCSV.ADDITIONAL_PROPERTIES_COUNT_COMPARE}.";
                    }

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        string data = "";
                        string[] item = null;

                        while (reader.Peek() >= 0)
                        {
                            data += reader.ReadLine();

                            item = data.Split(';');

                            if (item.Length == header.Length)
                            {
                                lines.Add(item);
                                data = "";
                            }
                            else
                            {
                                data += "\r\n";
                            }
                        }
                    }
                }


                if (string.IsNullOrEmpty(errorMessage))
                {
                    csvCategories = new List<CategoryCSV>();
                    csvTags = new List<TagCSV>();

                    foreach (var item in lines)
                    {
                        var addr = new AddressCSV();

                        try
                        {
                            addr.AddressUUID = GetColumnValue<string>(false, "Address UUID", item[0]);
                            addr.AddressName = GetColumnValue<string>(true, "Address Name", item[1]);
                            addr.ClientName = GetColumnValue<string>(false, "Client Name", item[2], addr.AddressName);
                            addr.CategoryName = GetColumnValue<string>(true, "Category Name", item[3]);
                            addr.AddressSlug = GetColumnValue<string>(true, "Address Slug", item[4], CsvTools.Slugify(addr.AddressName));
                            addr.CityName = GetColumnValue<string>(true, "City Name", item[5]);
                            addr.Latitude = GetColumnValue<double>(false, "Latitude", item[6]);
                            addr.Longitude = GetColumnValue<double>(false, "Longitude", item[7]);
                            addr.Passport = GetColumnValue<bool>(false, "Passport", item[8]);
                            addr.RecByFb = GetColumnValue<bool>(false, "Recommended By Facebook", item[9]);
                            addr.DescriptionEN = GetColumnValue<string>(false, "Description EN", item[10]);
                            addr.DescriptionFR = GetColumnValue<string>(false, "Description FR", item[11]);
                            addr.PhysicalAddr1 = GetColumnValue<string>(false, "PhysicalAddr 1", item[12]);
                            addr.PhysicalAddr2 = GetColumnValue<string>(false, "PhysicalAddr 2", item[13]);
                            addr.PhysicalAddr3 = GetColumnValue<string>(false, "PhysicalAddr 3", item[14]);
                            addr.PhysicalAddr4 = GetColumnValue<string>(false, "PhysicalAddr 4", item[15]);
                            addr.PhysicalAddr5 = GetColumnValue<string>(false, "PhysicalAddr 5", item[16]);
                            addr.PhysicalAddr6 = GetColumnValue<string>(false, "PhysicalAddr 6", item[17]);
                            addr.PhoneNumber1 = GetColumnValue<string>(false, "PhoneNumber 1", item[18]);
                            addr.PhoneNumber2 = GetColumnValue<string>(false, "PhoneNumber 2", item[19]);
                            addr.PhoneNumber3 = GetColumnValue<string>(false, "PhoneNumber 3", item[20]);
                            addr.MobileNumber1 = GetColumnValue<string>(false, "MobileNumber 1", item[21]);
                            addr.MobileNumber2 = GetColumnValue<string>(false, "MobileNumber 2", item[22]);
                            addr.MobileNumber3 = GetColumnValue<string>(false, "MobileNumber 3", item[23]);
                            addr.FaxNumber1 = GetColumnValue<string>(false, "Fax Number 1", item[24]);
                            addr.FaxNumber2 = GetColumnValue<string>(false, "Fax Number 2", item[25]);
                            addr.FaxNumber3 = GetColumnValue<string>(false, "Fax Number 3", item[26]);
                            addr.FbPageUrl = GetColumnValue<string>(false, "Facebook Page Url", item[27]);
                            addr.WebsiteUrl1 = GetColumnValue<string>(false, "Website Url 1", item[28]);
                            addr.WebsiteUrl2 = GetColumnValue<string>(false, "Website Url 2", item[29]);
                            addr.WebsiteUrl3 = GetColumnValue<string>(false, "Website Url 3", item[30]);
                            addr.Email1 = GetColumnValue<string>(false, "Email 1", item[31]);
                            addr.Email2 = GetColumnValue<string>(false, "Email 2", item[32]);
                            addr.Email3 = GetColumnValue<string>(false, "Email 3", item[33]);
                            addr.AddressLogoUrl = GetColumnValue<string>(false, "Address Logo Url", item[34]);
                            addr.AddressImgUrl1 = GetColumnValue<string>(false, "Address Imageg Url 1", item[35]);
                            addr.AddressImgUrl2 = GetColumnValue<string>(false, "Address Imageg Url 2", item[36]);
                            addr.AddressImgUrl3 = GetColumnValue<string>(false, "Address Imageg Url 3", item[37]);
                            addr.AddressImgUrl4 = GetColumnValue<string>(false, "Address Imageg Url 4", item[38]);
                            addr.AddressImgUrl5 = GetColumnValue<string>(false, "Address Imageg Url 5", item[39]);
                            addr.AddressImgUrl6 = GetColumnValue<string>(false, "Address Imageg Url 6", item[40]);
                            addr.AddressImgUrl7 = GetColumnValue<string>(false, "Address Imageg Url 7", item[41]);
                            addr.AddressImgUrl8 = GetColumnValue<string>(false, "Address Imageg Url 8", item[42]);
                            addr.AddressImgUrl9 = GetColumnValue<string>(false, "Address Imageg Url 9", item[43]);
                            addr.AddressImgUrl10 = GetColumnValue<string>(false, "Address Imageg Url 10", item[44]);
                            addr.AddressImgUrl11 = GetColumnValue<string>(false, "Address Imageg Url 11", item[45]);
                            addr.AddressImgUrl12 = GetColumnValue<string>(false, "Address Imageg Url 12", item[46]);
                            addr.AddressImgUrl13 = GetColumnValue<string>(false, "Address Imageg Url 13", item[47]);
                            addr.AddressImgUrl14 = GetColumnValue<string>(false, "Address Imageg Url 14", item[48]);
                            addr.AddressImgUrl15 = GetColumnValue<string>(false, "Address Imageg Url 15", item[49]);
                            addr.AddressImgUrl16 = GetColumnValue<string>(false, "Address Imageg Url 16", item[50]);
                            addr.AddressImgUrl17 = GetColumnValue<string>(false, "Address Imageg Url 17", item[51]);
                            addr.AddressImgUrl18 = GetColumnValue<string>(false, "Address Imageg Url 18", item[52]);
                            addr.AddressImgUrl19 = GetColumnValue<string>(false, "Address Imageg Url 19", item[53]);
                            addr.AddressImgUrl20 = GetColumnValue<string>(false, "Address Imageg Url 20", item[54]);
                            addr.AddressDocUrl1 = GetColumnValue<string>(false, "Address Document Url 1", item[55]);
                            addr.AddressDocUrl2 = GetColumnValue<string>(false, "Address Document Url 2", item[56]);
                            addr.AddressDocUrl3 = GetColumnValue<string>(false, "Address Document Url 3", item[57]);
                            addr.AddressDocUrl4 = GetColumnValue<string>(false, "Address Document Url 1", item[58]);
                            addr.AddressDocUrl5 = GetColumnValue<string>(false, "Address Document Url 1", item[59]);
                            addr.AddressDocUrl6 = GetColumnValue<string>(false, "Address Document Url 1", item[60]);
                            addr.AddressDocUrl7 = GetColumnValue<string>(false, "Address Document Url 1", item[61]);
                            addr.AddressDocUrl8 = GetColumnValue<string>(false, "Address Document Url 1", item[62]);
                            addr.AddressDocUrl9 = GetColumnValue<string>(false, "Address Document Url 1", item[63]);
                            addr.AddressDocUrl10 = GetColumnValue<string>(false, "Address Document Url 1", item[64]);
                            addr.AddressVideoUrl1 = GetColumnValue<string>(false, "Address Video Url 1", item[65]);
                            addr.AddressVideoUrl2 = GetColumnValue<string>(false, "Address Video Url 2", item[66]);
                            addr.AddressVideoUrl3 = GetColumnValue<string>(false, "Address Video Url 3", item[67]);
                            addr.AddressVideoUrl4 = GetColumnValue<string>(false, "Address Video Url 4", item[68]);
                            addr.AddressVideoUrl5 = GetColumnValue<string>(false, "Address Video Url 5", item[69]);
                            addr.AddressVideoUrl6 = GetColumnValue<string>(false, "Address Video Url 6", item[70]);
                            addr.AddressVideoUrl7 = GetColumnValue<string>(false, "Address Video Url 7", item[71]);
                            addr.AddressVideoUrl8 = GetColumnValue<string>(false, "Address Video Url 8", item[72]);
                            addr.AddressVideoUrl9 = GetColumnValue<string>(false, "Address Video Url 19", item[73]);
                            addr.AddressVideoUrl10 = GetColumnValue<string>(false, "Address Video Url 10", item[74]);
                            addr.AddressTags = GetColumnValue<string>(false, "Address Tags", item[75]);

                            if (_env.IsDevelopment()
                                && string.IsNullOrEmpty(addr.AddressTags))
                            {
                                addr.AddressTags = addr.CategoryName + "|" + CsvTools.Slugify(addr.AddressName).Replace("-", "|");
                            }

                        }
                        catch (Exception ex)
                        {
                            addr = addr == null ? new AddressCSV() : addr;
                            addr.ErrorMessage = $"Error caught for Row {addr.AddressUUID}:\r\n{ex.Message}";
                        }

                        addresses.Add(addr.AutoCorrectProperties(csvCategories, csvTags));
                    }

                    processedCsvCatgs = await _repo.Execute<CategoryCSV[]>(
                                                "ValidateCsvCategories",
                                                Helper.JSonCamelSerializeObject(
                                                            csvCategories.Select(l => l.Simplify()).ToArray()));

                    processedCsvTags = await _repo.Execute<TagCSV[]>(
                                                "ValidateCsvTags",
                                                Helper.JSonCamelSerializeObject(
                                                            csvTags.Select(l => l.Simplify()).ToArray()));

                    daysCsv = this.GetCsvDays(fileTimeCSV);

                    if (daysCsv != null && daysCsv.Length > 0)
                    {
                        foreach (var addr in addresses)
                        {
                            addr._DaysCsv = (from d in daysCsv.Where(l => l.AddressUUID == addr.AddressUUID).SelectMany(l => l.Pivot()).OrderBy(l => l.Seqn)
                                             group d by d.Name into g
                                             select new
                                             {
                                                 name = g.Key,
                                                 values = g.OrderBy(l => l.HourFrom).ThenBy(l => l.HourTo).Select(l => l.Simplify()).ToArray()
                                             }).ToArray();
                        }
                    }
                }

                return Ok(
                    new
                    {
                        addresses = addresses.ToArray(),
                        error = errorMessage,
                        processedCsvCatgs = processedCsvCatgs == null ? null : processedCsvCatgs.Select(l => l.Simplify()).ToArray(),
                        processedCsvTags = processedCsvTags == null ? null : processedCsvTags.Select(l => l.Simplify()).ToArray()
                    });
            }
            catch (Exception ex)
            {
                return BadRequestEx(ex);
            }
            finally
            {
                errorMessage = null;

                fileAddrCSV = null;
                fileTimeCSV = null;

                header = null;
                props = null;

                daysCsv = null;

                if (csvCategories != null)
                {
                    csvCategories.Clear();
                    csvCategories = null;
                }

                if (addresses != null)
                {
                    addresses.Clear();
                    addresses = null;
                }

                if (lines != null)
                {
                    lines.Clear();
                    lines = null;
                }
            }
        }


        private DaysCsv[] GetCsvDays(IFormFile file)
        {
            if (file == null || file.Length <= 0) return new DaysCsv[0];

            PropertyInfo[] props;

            var days = new List<DaysCsv>();
            var lines = new List<string[]>();

            string[] header = null;

            DaysCsv d;

            try
            {
                props = typeof(DaysCsv).GetProperties();

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    header = reader.ReadLine().Split(';');

                    if (header.Length != (props.Length + DaysCsv.ADDITIONAL_PROPERTIES_COUNT_COMPARE))
                    {
                        throw new Exception($"There are {header.Length} column instead of {props.Length + DaysCsv.ADDITIONAL_PROPERTIES_COUNT_COMPARE}.");
                    }

                    string data = "";
                    string[] item = null;

                    while (reader.Peek() >= 0)
                    {
                        data += reader.ReadLine();
                        item = data.Split(';');

                        if (item.Length == header.Length)
                        {
                            lines.Add(item);
                            data = "";
                        }
                        else
                        {
                            data += "\r\n";
                        }
                    }
                }


                foreach (var obj in lines)
                {
                    d = new DaysCsv();

                    d.Day_20_Monday.Name = this.GetLabel("lbl_Monday");
                    d.Day_20_Monday.Seqn = 1;

                    d.Day_30_Tuesday.Name = this.GetLabel("lbl_Tuesday");
                    d.Day_30_Tuesday.Seqn = 2;

                    d.Day_40_Wednesday.Name = this.GetLabel("lbl_Wednesday");
                    d.Day_40_Wednesday.Seqn = 3;

                    d.Day_50_Thursday.Name = this.GetLabel("lbl_Thursday");
                    d.Day_50_Thursday.Seqn = 4;

                    d.Day_60_Friday.Name = this.GetLabel("lbl_Friday");
                    d.Day_60_Friday.Seqn = 5;

                    d.Day_70_Saturday.Name = this.GetLabel("lbl_Saturday");
                    d.Day_70_Saturday.Seqn = 6;

                    d.Day_10_Sunday.Name = this.GetLabel("lbl_Sunday");
                    d.Day_10_Sunday.Seqn = 7;

                    d.Day_80_PH.Name = this.GetLabel("lbl_pHoliday");
                    d.Day_80_PH.Seqn = 8;

                    foreach (var col in obj)
                    {
                        if (col != null && col.Contains(":"))
                        {
                            var colSplit = col.Split(':').Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Trim()).ToArray();

                            if (colSplit == null || colSplit.Length != 2)
                            {
                                d.ErrorMessage = "Invalid Row: " + col;
                            }
                            else
                            {
                                switch (colSplit[0])
                                {
                                    case "10":
                                        d.Day_10_Sunday.CsvValue = colSplit[1];
                                        break;

                                    case "20":
                                        d.Day_20_Monday.CsvValue = colSplit[1];
                                        break;

                                    case "30":
                                        d.Day_30_Tuesday.CsvValue = colSplit[1];
                                        break;

                                    case "40":
                                        d.Day_40_Wednesday.CsvValue = colSplit[1];
                                        break;

                                    case "50":
                                        d.Day_50_Thursday.CsvValue = colSplit[1];
                                        break;

                                    case "60":
                                        d.Day_60_Friday.CsvValue = colSplit[1];
                                        break;

                                    case "70":
                                        d.Day_70_Saturday.CsvValue = colSplit[1];
                                        break;

                                    case "80":
                                        d.Day_80_PH.CsvValue = colSplit[1];
                                        break;

                                    default:
                                        d.ErrorMessage = "Invalid Day value found :" + colSplit[0];
                                        break;
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(col))
                        {
                            if (string.IsNullOrEmpty(d.AddressUUID))
                            {
                                d.AddressUUID = col.Trim();
                            }
                            else
                            {
                                d.ErrorMessage = "Invalid Day value found :" + col;
                            }
                        }
                    }

                    days.Add(d.Setup());
                }

                return days == null ? new DaysCsv[0] : days.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                props = null;
                header = null;
                d = null;

                if (days != null)
                {
                    days.Clear();
                    days = null;
                }

                if (lines != null)
                {
                    lines.Clear();
                    lines = null;
                }
            }
        }
    }
}
