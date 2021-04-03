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
            var output = DCT.DCT2D(input);

            output.Should().BeEquivalentTo(legacyOutput);
        }
    }
}