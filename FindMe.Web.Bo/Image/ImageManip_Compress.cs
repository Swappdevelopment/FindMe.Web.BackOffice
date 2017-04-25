using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace FindMe.Web.App
{
    public partial class ImageManip
    {
        private static MemoryStream Compress(Stream input, int quality = 0)
        {
            MemoryStream output;

            Bitmap resized;

            try
            {
                using (var image = new Bitmap(input))
                {
                    int width = image.Width;
                    int height = image.Height;

                    resized = new Bitmap(width, height);

                    using (var graphics = Graphics.FromImage(resized))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.DrawImage(image, 0, 0, width, height);


                        output = new MemoryStream();

                        var qualityParamId = Encoder.Quality;

                        var encoderParameters = new EncoderParameters(1);

                        encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);

                        var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                        resized.Save(output, codec, encoderParameters);
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                output = null;
                resized = null;
            }
        }
    }
}
