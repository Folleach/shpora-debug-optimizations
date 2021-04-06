using System;
using System.Collections.Generic;
using System.Linq;

namespace JPEG.Images
{
    public struct Pixel
    {
        private static readonly HashSet<PixelFormat> SupportFormats = new HashSet<PixelFormat>(new[] { PixelFormat.RGB, PixelFormat.YCbCr });
        private PixelFormat format;

        public Pixel(double firstComponent, double secondComponent, double thirdComponent, PixelFormat pixelFormat)
            : this()
        {
            SetPixel(firstComponent, secondComponent, thirdComponent, pixelFormat);
        }

        private byte first;
        private byte second;
        private byte third;

        public double R => format == PixelFormat.RGB ? first : (298.082 * first + 408.583 * third) / 256.0 - 222.921;
        public double G => format == PixelFormat.RGB ? second : (298.082 * first - 100.291 * second - 208.120 * third) / 256.0 + 135.576;
        public double B => format == PixelFormat.RGB ? third : (298.082 * first + 516.412 * second) / 256.0 - 276.836;

        public double Y => format == PixelFormat.YCbCr ? first : 16.0 + (65.738 * first + 129.057 * second + 24.064 * third) / 256.0;
        public double Cb => format == PixelFormat.YCbCr ? second : 128.0 + (-37.945 * first - 74.494 * second + 112.439 * third) / 256.0;
        public double Cr => format == PixelFormat.YCbCr ? third : 128.0 + (112.439 * first - 94.154 * second - 18.285 * third) / 256.0;

        public void SetPixel(double firstComponent, double secondComponent, double thirdComponent,
            PixelFormat pixelFormat)
        {
            if (!SupportFormats.Contains(pixelFormat))
                throw new FormatException("Unknown pixel format: " + pixelFormat);
            format = pixelFormat;
            first = (byte)firstComponent;
            second = (byte)secondComponent;
            third = (byte)thirdComponent;
        }
    }
}