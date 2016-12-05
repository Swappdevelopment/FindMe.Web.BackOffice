using FindMe.Data;
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
        protected AppDbInteractor _dbi;

        public BaseController(IConfigurationRoot config, AppDbInteractor dbi)
            : base()
        {
            _config = config;
            _dbi = dbi;

            if (_dbi != null)
            {
                SetDbiClientIpAddressASync += BaseController_SetDbiClientIpAddressASync;
                SetDbiClientIpAddressASync(null, null);
            }
        }

        private event EventHandler SetDbiClientIpAddressASync;
        private async void BaseController_SetDbiClientIpAddressASync(object sender, EventArgs e)
        {
            SetDbiClientIpAddressASync -= BaseController_SetDbiClientIpAddressASync;

            if (_dbi != null)
            {
                _dbi.SetClientIpAddress(await GetClientIpAddressV4ASync());
            }
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
    }
}
