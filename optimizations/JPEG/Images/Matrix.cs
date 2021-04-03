using System;
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
                    participant[j] = new Pixel(0, 0, 0, PixelFormat.RGB);
            }
        }
        
        public Matrix(Bitmap bmp)
        {
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
                    matrix.Pixels[j][i] = new Pixel(r, g, b, PixelFormat.RGB);
                }
            }

            // for(var j = 0; j < height; j++)
            // {
            //     for(var i = 0; i < width; i++)
            //     {
            //         var pixel = bmp.GetPixel(i, j);
            //         matrix.Pixels[j, i] = new Pixel(pixel.R, pixel.G, pixel.B, PixelFormat.RGB);
            //     }
            // }
            
            bmp.UnlockBits(bitmapData);

            return matrix;
        }

        public static explicit operator Bitmap(Matrix matrix)
        {
            var bmp = new Bitmap(matrix.Width, matrix.Height);

            for(var j = 0; j < bmp.Height; j++)
            {
                for(var i = 0; i < bmp.Width; i++)
                {
                    var pixel = matrix.Pixels[j][i];
                    bmp.SetPixel(i, j, Color.FromArgb(ToByte(pixel.R), ToByte(pixel.G), ToByte(pixel.B)));
                }
            }

            return bmp;
        }

        public static int ToByte(double d)
        {
            var val = (int) d;
            if (val > byte.MaxValue)
                return byte.MaxValue;
            if (val < byte.MinValue)
                return byte.MinValue;
            return val;
        }
    }
}