using System;
using System.IO;

namespace FindMe.Web.App
{
    public partial class ImageManip
    {
        public static MemoryStream OptimizeImage(Stream input, int quality, int width, int height, bool resize = true, bool compress = true)
        {
            MemoryStream result;

            try
            {
                if (resize && compress)
                {
                    using (var resizedStream = Resize(input, width, height))
                    {
                        result = Compress(resizedStream, quality: quality);
                    }
                }
                else if (resize)
                {
                    result = Resize(input, width, height);
                }
                else if (compress)
                {
                    result = Compress(input, quality: quality);
                }
                else
                {
                    result = new MemoryStream();
                    input.CopyTo(result);
                }


                return result;
                //using (var resizedStream = Resize(input))
                //{

                //    return Compress(resizedStream);
                //    using (var compressStream = Compress(resizedStream))
                //    {
                //        using (var output = File.Exists(newImagePath) ? File.OpenWrite(newImagePath) : File.Create(newImagePath))
                //        {
                //            bArr = compressStream.ToArray();

                //            output.Write(bArr, 0, bArr.Length);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                result = null;
                //bArr = null;
            }
        }
    }
}
