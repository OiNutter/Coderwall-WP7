using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;
using System.Diagnostics;
namespace Coderwall.Models
{
    public class ImageCache
    {
        private IsolatedStorageFile Cache { get; set; }

        public ImageCache()
        {
            Cache = IsolatedStorageFile.GetUserStoreForApplication();
        }

        public bool ImageExists(Uri ImageUri)
        {
            string filename = "Images\\" + Path.GetFileName(ImageUri.LocalPath);

            return Cache.FileExists(filename);
        }

        public WriteableBitmap LoadImage(Uri ImageUri)
        {
            string filename = "Images\\" + Path.GetFileName(ImageUri.LocalPath);
            IsolatedStorageFileStream FileStream = Cache.OpenFile(filename,FileMode.Open);
            byte[] Bytes;

            using (FileStream)
            {
                Bytes = new byte[FileStream.Length];
                FileStream.Read(Bytes, 0, Bytes.Length);
            }

            return GetImage(Bytes);
        }

        public void StoreImage(object sender, RoutedEventArgs e ){
            BitmapImage Image = (BitmapImage)sender;
            byte[] Bytes = GetByteArray(new WriteableBitmap(Image));
            string filename = "Images\\" + Path.GetFileName(Image.UriSource.LocalPath);

            if(!Cache.DirectoryExists("Images"))
                Cache.CreateDirectory("Images");

            if (Cache.FileExists(filename))
                Cache.DeleteFile(filename);

            IsolatedStorageFileStream FileStream = Cache.CreateFile(filename);

            using (FileStream)
                FileStream.Write(Bytes, 0, Bytes.Length);

        }

        private byte[] GetByteArray(WriteableBitmap Bitmap)
        {
            long MatrixSize = Bitmap.PixelWidth * Bitmap.PixelHeight;
	         
            byte[] Bytes = new byte[MatrixSize * 4 + 4];
	 
	        long BufferPos = 0;

            Bytes[BufferPos++] = (byte)((Bitmap.PixelWidth / 256) & 0xff);
            Bytes[BufferPos++] = (byte)((Bitmap.PixelWidth % 256) & 0xff);
            Bytes[BufferPos++] = (byte)((Bitmap.PixelHeight / 256) & 0xff);
            Bytes[BufferPos++] = (byte)((Bitmap.PixelHeight % 256) & 0xff);
	 
	        for (int MatrixPos = 0; MatrixPos < MatrixSize; MatrixPos++)
	        {
                Bytes[BufferPos++] = (byte)((Bitmap.Pixels[MatrixPos] >> 24) & 0xff);
                Bytes[BufferPos++] = (byte)((Bitmap.Pixels[MatrixPos] >> 16) & 0xff);
                Bytes[BufferPos++] = (byte)((Bitmap.Pixels[MatrixPos] >> 8) & 0xff);
                Bytes[BufferPos++] = (byte)((Bitmap.Pixels[MatrixPos]) & 0xff);   
	        }
	 
	        return Bytes;

        }

        private WriteableBitmap GetImage(byte[] buffer)
	    {
	        int width = buffer[0]*256 + buffer[1];
	        int height = buffer[2]*256 + buffer[3];
	 
	        long matrixSize = width*height;
	 
	        WriteableBitmap Bitmap = new WriteableBitmap(width, height);
	 
	        int bufferPos = 4;
	 
	        for (int matrixPos = 0; matrixPos < matrixSize; matrixPos++)
	        {
	            int pixel = buffer[bufferPos++];
	            pixel = pixel << 8 | buffer[bufferPos++];
	            pixel = pixel << 8 | buffer[bufferPos++];
	            pixel = pixel << 8 | buffer[bufferPos++];
                Bitmap.Pixels[matrixPos] = pixel;
	        }

            return Bitmap;
	    }

    }
}
