using System.Drawing;
using System.Drawing.Imaging;

namespace JPEG.Images
{
    class Matrix
    {
        public readonly Pixel[][] Pixels;
        public readonly int Height;
        public readonly int Width;
				
        public Matrix(int height, int width)
        {
            Height = height;
            Width = width;
			
            Pixels = new Pixel[height][];
            for (var i = 0; i < height; ++i)
            {
                Pixel[] participant = Pixels[i] = new Pixel[width];
                for(var j = 0; j< width; ++j)
                    participant[j] = new Pixel();
            }
        }

        public static unsafe explicit operator Matrix(Bitmap bmp)
        {
            var height = bmp.Height - bmp.Height % 8;
            var width = bmp.Width - bmp.Width % 8;
            var matrix = new Matrix(height, width);

            var bitmapData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            
            for (var j = 0; j < height; j++)
            {
                var row = (byte*)bitmapData.Scan0 + (j * bitmapData.Stride);
                for (var i = 0; i < width; i++)
                {
                    var b = *(row++);
                    var g = *(row++);
                    var r = *(row++);
                    matrix.Pixels[j][i].SetPixel(r, g, b, PixelFormat.RGB);
                }
            }

            bmp.UnlockBits(bitmapData);

            return matrix;
        }

        public static unsafe explicit operator Bitmap(Matrix matrix)
        {
            var bmp = new Bitmap(matrix.Width, matrix.Height);

            var bitmapData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            
            for (var j = 0; j < matrix.Height; j++)
            {
                var row = (byte*)bitmapData.Scan0 + (j * bitmapData.Stride);
                for (var i = 0; i < matrix.Width; i++)
                {
                    *(row++) = ToByte(matrix.Pixels[j][i].B);
                    *(row++) = ToByte(matrix.Pixels[j][i].G);
                    *(row++) = ToByte(matrix.Pixels[j][i].R);
                }
            }
            
            bmp.UnlockBits(bitmapData);

            return bmp;
        }

        private static byte ToByte(double d)
        {
            var val = (int) d;
            if (val > byte.MaxValue)
                return byte.MaxValue;
            if (val < byte.MinValue)
                return byte.MinValue;
            return (byte)val;
        }
    }
}