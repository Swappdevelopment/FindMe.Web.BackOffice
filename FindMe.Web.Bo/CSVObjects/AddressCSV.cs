using System.ComponentModel.DataAnnotations;

namespace FindMe.Web.App.CSVObjects
{
    public class AddressCSV
    {
        public const int ADDITIONAL_PROPERTIES_COUNT_COMPARE = -1;

        public string ErrorMessage { get; set; }


        public string AddressUUID { get; set; }

        [Required]
        public string AddressName { get; set; }

        public string ClientName { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string AddressSlug { get; set; }

        [Required]
        public string CityName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool Passport { get; set; }
        public bool RecByFb { get; set; }
        public string DescriptionEN { get; set; }
        public string DescriptionFR { get; set; }

        public string PhysicalAddr1 { get; set; }
        public string PhysicalAddr2 { get; set; }
        public string PhysicalAddr3 { get; set; }
        public string PhysicalAddr4 { get; set; }
        public string PhysicalAddr5 { get; set; }
        public string PhysicalAddr6 { get; set; }

        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string PhoneNumber3 { get; set; }

        public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }
        public string MobileNumber3 { get; set; }

        public string FaxNumber1 { get; set; }
        public string FaxNumber2 { get; set; }
        public string FaxNumber3 { get; set; }

        public string FbPageUrl { get; set; }

        public string WebsiteUrl1 { get; set; }
        public string WebsiteUrl2 { get; set; }
        public string WebsiteUrl3 { get; set; }

        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }

        public string AddressLogoUrl { get; set; }

        public string AddressImgUrl1 { get; set; }
        public string AddressImgUrl2 { get; set; }
        public string AddressImgUrl3 { get; set; }
        public string AddressImgUrl4 { get; set; }
        public string AddressImgUrl5 { get; set; }
        public string AddressImgUrl6 { get; set; }
        public string AddressImgUrl7 { get; set; }
        public string AddressImgUrl8 { get; set; }
        public string AddressImgUrl9 { get; set; }
        public string AddressImgUrl10 { get; set; }
        public string AddressImgUrl11 { get; set; }
        public string AddressImgUrl12 { get; set; }
        public string AddressImgUrl13 { get; set; }
        public string AddressImgUrl14 { get; set; }
        public string AddressImgUrl15 { get; set; }
        public string AddressImgUrl16 { get; set; }
        public string AddressImgUrl17 { get; set; }
        public string AddressImgUrl18 { get; set; }
        public string AddressImgUrl19 { get; set; }
        public string AddressImgUrl20 { get; set; }

        public string AddressDocUrl1 { get; set; }
        public string AddressDocUrl2 { get; set; }
        public string AddressDocUrl3 { get; set; }
        public string AddressDocUrl4 { get; set; }
        public string AddressDocUrl5 { get; set; }
        public string AddressDocUrl6 { get; set; }
        public string AddressDocUrl7 { get; set; }
        public string AddressDocUrl8 { get; set; }
        public string AddressDocUrl9 { get; set; }
        public string AddressDocUrl10 { get; set; }

        public string AddressVideoUrl1 { get; set; }
        public string AddressVideoUrl2 { get; set; }
        public string AddressVideoUrl3 { get; set; }
        public string AddressVideoUrl4 { get; set; }
        public string AddressVideoUrl5 { get; set; }
        public string AddressVideoUrl6 { get; set; }
        public string AddressVideoUrl7 { get; set; }
        public string AddressVideoUrl8 { get; set; }
        public string AddressVideoUrl9 { get; set; }
        public string AddressVideoUrl10 { get; set; }

        public string AddressTags { get; set; }
    }
}
