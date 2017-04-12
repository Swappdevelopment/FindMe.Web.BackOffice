using System;
using System.Collections.Generic;

namespace FindMe.Web.App
{
    public class DaysCsv
    {
        public const int ADDITIONAL_PROPERTIES_COUNT_COMPARE = -1;

        public DaysCsv()
        {
            this.Day_10_Sunday = new DayCsvItem() { ID = 10 };
            this.Day_20_Monday = new DayCsvItem() { ID = 20 };
            this.Day_30_Tuesday = new DayCsvItem() { ID = 30 };
            this.Day_40_Wednesday = new DayCsvItem() { ID = 40 };
            this.Day_50_Thursday = new DayCsvItem() { ID = 50 };
            this.Day_60_Friday = new DayCsvItem() { ID = 60 };
            this.Day_70_Saturday = new DayCsvItem() { ID = 70 };
            this.Day_80_PH = new DayCsvItem() { ID = 80 };
        }

        public string ErrorMessage { get; set; }


        public string AddressUUID { get; set; }

        public DayCsvItem Day_10_Sunday { get; private set; }
        public DayCsvItem Day_20_Monday { get; private set; }
        public DayCsvItem Day_30_Tuesday { get; private set; }
        public DayCsvItem Day_40_Wednesday { get; private set; }
        public DayCsvItem Day_50_Thursday { get; private set; }
        public DayCsvItem Day_60_Friday { get; private set; }
        public DayCsvItem Day_70_Saturday { get; private set; }
        public DayCsvItem Day_80_PH { get; private set; }


        public DaysCsv Setup()
        {
            try
            {
                if (this.Day_10_Sunday != null)
                {
                    this.Day_10_Sunday.Setup();
                }

                if (this.Day_20_Monday != null)
                {
                    this.Day_20_Monday.Setup();
                }

                if (this.Day_30_Tuesday != null)
                {
                    this.Day_30_Tuesday.Setup();
                }

                if (this.Day_40_Wednesday != null)
                {
                    this.Day_40_Wednesday.Setup();
                }

                if (this.Day_50_Thursday != null)
                {
                    this.Day_50_Thursday.Setup();
                }

                if (this.Day_60_Friday != null)
                {
                    this.Day_60_Friday.Setup();
                }

                if (this.Day_70_Saturday != null)
                {
                    this.Day_70_Saturday.Setup();
                }

                if (this.Day_80_PH != null)
                {
                    this.Day_80_PH.Setup();
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }

            return this;
        }

        public Dictionary<string, object> ToDico()
        {
            var result = new Dictionary<string, object>();

            if (Day_20_Monday != null)
            {
                result.Add(Day_20_Monday.Name, Day_20_Monday.Simplify());
            }
            if (Day_30_Tuesday != null)
            {
                result.Add(Day_30_Tuesday.Name, Day_30_Tuesday.Simplify());
            }
            if (Day_40_Wednesday != null)
            {
                result.Add(Day_40_Wednesday.Name, Day_40_Wednesday.Simplify());
            }
            if (Day_50_Thursday != null)
            {
                result.Add(Day_50_Thursday.Name, Day_50_Thursday.Simplify());
            }
            if (Day_60_Friday != null)
            {
                result.Add(Day_60_Friday.Name, Day_60_Friday.Simplify());
            }
            if (Day_70_Saturday != null)
            {
                result.Add(Day_70_Saturday.Name, Day_70_Saturday.Simplify());
            }
            if (Day_10_Sunday != null)
            {
                result.Add(Day_10_Sunday.Name, Day_10_Sunday.Simplify());
            }
            if (Day_80_PH != null)
            {
                result.Add(Day_80_PH.Name, Day_80_PH.Simplify());
            }

            return result;
        }

        public DayCsvItem[] Pivot()
        {
            return new DayCsvItem[]
            {
                this.Day_20_Monday,
                this.Day_30_Tuesday,
                this.Day_40_Wednesday,
                this.Day_50_Thursday,
                this.Day_60_Friday,
                this.Day_70_Saturday,
                this.Day_10_Sunday,
                this.Day_80_PH
            };
        }
    }
}
