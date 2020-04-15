using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ManagerApp.Utilities
{
    public class ImageConverter
    {
        public async static Task<ImageSource> ConvertBase64ToImageSource(string source)
        {
            byte[] bytes;
            if(source.Contains(","))
            {
                bytes = Convert.FromBase64String(source.Split(',')[1]);
            }
            else
            {
                bytes = Convert.FromBase64String(source);
            }
            var buf = bytes.AsBuffer();
            var stream = buf.AsStream();
            var image = stream.AsRandomAccessStream();
            var decoder = await BitmapDecoder.CreateAsync(image);
            image.Seek(0);
            var output = new WriteableBitmap((int)decoder.PixelHeight, (int)decoder.PixelWidth);
            await output.SetSourceAsync(image);
            return output;
        }

        public async static Task<string> ConvertStorageFileToBase64(Windows.Storage.StorageFile file)
        {
            byte[] fileBytes = null;
            if(file == null)
            {
                return null;
            }
            using (var stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (var reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }
            string base64 = Convert.ToBase64String(fileBytes);
            return base64;
        }
    }
}
