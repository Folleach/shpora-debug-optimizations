using System;
using FluentAssertions;
using JPEG.Maintenance.Legacy;
using NUnit.Framework;

namespace JPEG.Maintenance.Tests
{
    [TestFixture]
    public class DCTTests
    {
        [Test]
        public void DCT_DCT2D_ShouldBeEquivalentToLegacyDCT2D()
        {
            var input = new double[,]
            {
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
            };
            
            var legacyOutput = DCT_Legacy.DCT2D(input);
            var output = DCT.DCT2D(input, 1, 1);

            output.Should().BeEquivalentTo(legacyOutput);
        }

        [Test]
        public void DCT_IDCT2D_ShouldBeEquivalentToLegacyIDCT2D()
        {
            var input = new double[,]
            {
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
                {1, 2, 3, 4, 5, 6, 7, 8},
            };

            var legacyOutput = new double[input.GetLength(0), input.GetLength(1)];
            var output = new double[input.GetLength(0), input.GetLength(1)];
            
            DCT_Legacy.IDCT2D(input, legacyOutput);
            DCT.IDCT2D(input, output, 1, 1);

            output.Should().BeEquivalentTo(legacyOutput);
        }
    }
}