using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using CAA_Event_Management.Models;
using Windows.Storage;

namespace CAA_Event_Management.Utilities
{
    public class ImageConverter
    {
        // conversion image/byte with modifications
        // https://stackoverflow.com/questions/35111635/how-can-i-convert-a-image-into-a-byte-array-in-uwp-platform

        /// <summary>
        /// Takes in Byte[] and converts to image. Use to get image from Database
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public BitmapImage ByteToImage(Byte[] picture)
        {
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(picture);
                    _ = writer.StoreAsync();
                }
                var image = new BitmapImage();
                _ = image.SetSourceAsync(stream);
                return image;
            }
        }

        /// <summary>
        /// Takes image from a file source and converts to Byte[].
        /// Use to save image to database
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<Byte[]> ImageToByte(StorageFile file)
        {
            byte[] byteArray;
            using (var inputStream = await file.OpenReadAsync())
            {
                var readStream = inputStream.AsStreamForRead();
                byteArray = new byte[readStream.Length];
                await readStream.ReadAsync(byteArray, 0, byteArray.Length);            
            }
            return byteArray;
        }
    }
}
