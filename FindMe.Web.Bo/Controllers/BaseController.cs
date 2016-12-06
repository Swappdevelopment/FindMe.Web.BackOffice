using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class BaseController : Controller
    {
        protected IConfigurationRoot _config;
        protected WebDbReader _reader;

        public BaseController(IConfigurationRoot config, WebDbReader reader)
            : base()
        {
            _config = config;
            _reader = reader;

            if (_reader != null)
            {
                SetDbiClientIpAddressASync += BaseController_SetDbiClientIpAddressASync;
                SetDbiClientIpAddressASync(null, null);

                reader.ChangeAccessToken += Reader_ChangeAccessToken;
            }
        }

        private event EventHandler SetDbiClientIpAddressASync;
        private async void BaseController_SetDbiClientIpAddressASync(object sender, EventArgs e)
        {
            SetDbiClientIpAddressASync -= BaseController_SetDbiClientIpAddressASync;

            if (_reader != null)
            {
                _reader.SetClientIpAddress(await GetClientIpAddressV4ASync());
            }
        }


        private void Reader_ChangeAccessToken(object sender, Swapp.Data.ValuePairEventArgs<string, bool> e)
        {

        }



        public string ClientIpAddress
        {
            get
            {
                if (this.HttpContext != null
                    && this.HttpContext.Connection != null)
                {
                    return this.HttpContext.Connection.RemoteIpAddress.ToString();
                }

                return null;
            }
        }


        public async Task<string> GetClientIpAddressV4ASync()
        {
            string result = this.ClientIpAddress;

            if (!string.IsNullOrEmpty(this.ClientIpAddress))
            {
                IPAddress ipAddress;

                if (IPAddress.TryParse(result, out ipAddress))
                {
                    if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        ipAddress = (await Dns.GetHostEntryAsync(ipAddress)).AddressList.FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    }

                    result = ipAddress == null ? result : ipAddress.ToString();
                }
            }

            return result;
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_reader != null)
            {
                _reader.Dispose();
                _reader.ChangeAccessToken -= Reader_ChangeAccessToken;

                _reader = null;
            }
        }
    }
}
