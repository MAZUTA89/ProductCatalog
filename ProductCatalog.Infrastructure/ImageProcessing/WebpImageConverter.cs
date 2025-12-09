using System;
using ImageMagick;

namespace ProductCatalog.Infrastructure.ImageProcessing
{
    public class WebpImageConverter : IImageConverter
    {
        static WebpImageConverter()
        {
            MagickNET.Initialize();
        }

        public async Task<Stream> ConvertFromAsync(Stream stream)
        {
            using var image = new MagickImage(stream);

            var settings = new MagickReadSettings();
            settings.Width = 800;
            settings.Height = 600;

            var targetStream = new MemoryStream();

            byte[] imageBytes = image.ToByteArray();

            using var webpImg = new MagickImage(imageBytes, settings);

            webpImg.Format = MagickFormat.WebP;

            await webpImg.WriteAsync(targetStream);

            targetStream.Position = 0;

            return targetStream;
        }
    }
}
