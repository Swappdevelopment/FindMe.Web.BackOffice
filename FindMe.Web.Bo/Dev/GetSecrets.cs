using Newtonsoft.Json.Linq;
using Swapp.Data;
using System;
using System.IO;

namespace FindMe.Web.App
{
    public static class DevSecrets
    {
        private static JObject _jSecrets = null;

        public static string GetSecretValue(string key, string rootPath)
        {
            string[] keyPath = null;


            object jo = null;

            try
            {
                InitSecretsObj(rootPath);

                keyPath = key.Split(':');

                jo = _jSecrets;


                foreach (var k in keyPath)
                {
                    if (jo != null
                        && jo is JObject)
                    {
                        jo = ((JObject)jo).JGetPropVal(k);
                    }
                }

                if (jo != null)
                {
                    return jo.ToString();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                jo = null;
            }
        }

        private static void InitSecretsObj(string rootPath)
        {
            if (_jSecrets == null)
            {
                DirectoryInfo di = null;
                FileInfo fi = null;

                try
                {
                    di = new DirectoryInfo(rootPath);

                    if (di.Exists)
                    {
                        di = di.Parent.Parent.Parent.Parent;

                        fi = new FileInfo(Path.Combine(di.FullName, @"Secrets\secrets.json"));

                        if (fi.Exists)
                        {
                            using (var streamReader = fi.OpenText())
                            {
                                _jSecrets = Helper.JSonDeserializeObject<JObject>(streamReader.ReadToEnd());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    di = null;
                    fi = null;
                }
            }
        }
    }
}
