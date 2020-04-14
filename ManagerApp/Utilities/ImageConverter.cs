using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ManagerApp.Utilities
{
    public class ImageConverter
    {
        public async static Task<ImageSource> ConvertBase64ToImageSource(string source)
        {
            var bytes = Convert.FromBase64String(source.Split(',')[1]);
            var buf = bytes.AsBuffer();
            var stream = buf.AsStream();
            var image = stream.AsRandomAccessStream();
            var decoder = await BitmapDecoder.CreateAsync(image);
            image.Seek(0);
            var output = new WriteableBitmap((int)decoder.PixelHeight, (int)decoder.PixelWidth);
            await output.SetSourceAsync(image);
            return output;
        }
    }
}
