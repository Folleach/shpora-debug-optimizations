using System;
using System.Linq;
using FluentAssertions;
using JPEG.Maintenance.Legacy;
using JPEG.Utilities;
using NUnit.Framework;

namespace JPEG.Maintenance.Tests
{
    [TestFixture]
    public class HuffmanCodecTests
    {
        private byte[] encodeData;
        private int NOfEncodeData = 1000;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            encodeData = new byte[NOfEncodeData];
            for (var i = 0; i < NOfEncodeData; i++)
                encodeData[i] = (byte)random.Next(256);
        }
    }
}