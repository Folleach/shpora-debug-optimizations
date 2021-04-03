using FluentAssertions;
using JPEG.Images;
using JPEG.Maintenance.Legacy;
using NUnit.Framework;

namespace JPEG.Maintenance.Tests
{
    [TestFixture]
    public class PixelTests
    {
        [Test, Combinatorial]
        public void Pixel_Constructor_PixelShouldBeEquivalentWithLegacyPixel(
            [Values(0, 100, 255)] int firstComponent,
            [Values(13, 17, 0)] int secondComponent,
            [Values(255, 0, 33)] int thirdComponent,
            [Values(PixelFormat.RGB)] PixelFormat format)
        {
            var pixel = new Pixel(firstComponent, secondComponent, thirdComponent, format);
            var legacyPixel = new Pixel_Legacy(firstComponent, secondComponent, thirdComponent, format);
            
            pixel.Should().BeEquivalentTo(legacyPixel);
        }
    }
}