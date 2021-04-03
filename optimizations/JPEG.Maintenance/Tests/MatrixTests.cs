using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using JPEG.Images;
using JPEG.Maintenance.Legacy;

namespace JPEG.Maintenance.Tests
{
    [TestFixture]
    public class MatrixTests
    {
        private Bitmap image;
        
        [SetUp]
        public void Setup()
        {
            image = new Bitmap(100, 76);
            var random = new Random();
            
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    image.SetPixel(x, y, Color.FromArgb(
                        random.Next(256),
                        random.Next(256),
                        random.Next(256),
                        random.Next(256)
                        ));
                }
            }
        }
        
        [Test]
        public void Matrix_ConvertBitmapToMatrix_ShouldBeReturnSamePixelsAsMatrixLegacy()
        {
            var current = (Matrix) image;
            var legacy = (Matrix_Legacy) image;

            Assert.AreEqual(legacy.Pixels.GetLength(0), current.Pixels.Length);
            Assert.AreEqual(legacy.Pixels.GetLength(1), current.Pixels[0].Length);
            
            for (var x = 0; x < current.Height; x++)
            {
                for (var y = 0; y < current.Width; y++)
                {
                    Assert.AreEqual(legacy.Pixels[x, y], current.Pixels[x][y]);
                }
            }
        }
    }
}