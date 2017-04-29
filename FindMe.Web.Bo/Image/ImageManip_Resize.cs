using ImageSharp;
using ImageSharp.Formats;
using ImageSharp.Processing;
using System;
using System.IO;

namespace FindMe.Web.App
{
    public partial class ImageManip
    {
        private static MemoryStream Resize(Stream input, int width, int height)
        {
            MemoryStream output;

            try
            {
                output = new MemoryStream();

                var image = Image.Load(input)
                    .Resize(new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Crop
                    });

                image.Save(output, new JpegEncoder());


                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                output = null;
            }
        }
    }
}
