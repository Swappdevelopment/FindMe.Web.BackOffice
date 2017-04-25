using ImageSharp;
using System;
using System.IO;

namespace FindMe.Web.App
{
    public partial class ImageManip
    {
        public static void OptimizeImage(Stream input, string newImagePath, int quality)
        {
            byte[] bArr;

            try
            {
                using (var resizedStream = Resize(input))
                {
                    using (var compressStream = Compress(resizedStream))
                    {
                        using (var output = File.Exists(newImagePath) ? File.OpenWrite(newImagePath) : File.Create(newImagePath))
                        {
                            bArr = compressStream.ToArray();

                            output.Write(bArr, 0, bArr.Length);
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
                bArr = null;
            }
        }
    }
}
