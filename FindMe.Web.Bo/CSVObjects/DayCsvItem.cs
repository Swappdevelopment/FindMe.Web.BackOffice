using System;
using System.Linq;

namespace FindMe.Web.App
{
    public class DayCsvItem
    {
        public DayCsvItem()
        {
            this.ValuesSet = false;
        }

        public string CsvValue { get; set; }
        public string Name { get; set; }
        public int Seqn { get; set; }

        public int ID { get; set; }
        public int HourFrom { get; set; }
        public int HourTo { get; set; }
        public int MinFrom { get; set; }
        public int MinTo { get; set; }

        public string ErrorMessage { get; set; }

        public bool ValuesSet { get; set; }

        public void Setup()
        {
            if (string.IsNullOrEmpty(CsvValue)) return;

            string raw = CsvValue.Replace(" ", "").ToLower();

            if (raw.Contains("close") || raw.Contains("cls") || raw.Contains("ferm"))
            {
                HourFrom = 0;
                MinFrom = 0;

                HourTo = 0;
                MinTo = 0;

                this.ValuesSet = true;
            }
            else if (raw.Contains("24") && raw.Contains("7"))
            {
                HourFrom = 0;
                MinFrom = 0;

                HourTo = 23;
                MinTo = 59;

                this.ValuesSet = true;
            }
            else if (raw.Contains("to"))
            {
                var split = raw.Replace("to", "|").Split('|').Select(s => FormatTime(s.Trim())).ToArray();

                if (split.Length < 1 || split.Length > 2)
                    throw new Exception("Invalid Time Format: " + CsvValue);

                int hour, min;

                ReadTime(split[0], out hour, out min);

                this.HourFrom = hour;
                this.MinFrom = min;

                ReadTime(split[1], out hour, out min);

                this.HourTo = hour;
                this.MinTo = min;

                this.ValuesSet = true;
            }
            else
            {
                throw new Exception("Time Format Unrecognised: " + CsvValue);
            }
        }

        private string FormatTime(string value)
        {
            try
            {
                string result;

                if (string.IsNullOrEmpty(value))
                {
                    result = "23:59";
                }
                else
                {
                    var split = value.Split(':');

                    if (split.Length == 1)
                    {
                        switch (value.Length)
                        {
                            case 1:
                                result = "0" + value + ":00";
                                break;

                            case 2:
                                result = "" + value + ":00";
                                break;

                            case 3:
                                result = "0" + value.Substring(0, 1) + ":" + value.Substring(1, 2);
                                break;

                            case 4:
                                result = "" + value.Substring(0, 2) + ":" + value.Substring(2, 2);
                                break;

                            default:
                                throw new Exception("Invalid Time Format: " + value);
                        }
                    }
                    else if (split.Length == 2)
                    {
                        if (split[0] == null || split[0].Length > 2 || split[0].Length < 1)
                            throw new Exception("Invalid Time Format: " + value);

                        if (split[1] == null || split[1].Length > 2 || split[1].Length < 1)
                            throw new Exception("Invalid Time Format: " + value);


                        result = (split[0].Length == 1 ? ("0" + split[0]) : split[0]) + ":" + (split[1].Length == 1 ? ("0" + split[1]) : split[1]);
                    }
                    else
                    {
                        throw new Exception("Invalid Time Format: " + value);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                throw ex;
            }
        }

        private void ReadTime(string value, out int hour, out int min)
        {
            var split = value.Split(':');

            try
            {
                hour = int.Parse(split[0]);
                min = int.Parse(split[1]);
            }
            catch
            {
                this.ErrorMessage = "Invalid Time Format: " + value;
                throw new Exception(this.ErrorMessage);
            }
        }


        public object Simplify()
        {
            return new
            {
                this.HourFrom,
                this.HourTo,
                this.MinFrom,
                this.MinTo,
                this.ValuesSet,
                this.ErrorMessage
            };
        }
        public object Pivot()
        {
            return new
            {
                this.ID,
                this.Name,
                this.HourFrom,
                this.HourTo,
                this.MinFrom,
                this.MinTo,
                this.ValuesSet
            };
        }
    }
}
