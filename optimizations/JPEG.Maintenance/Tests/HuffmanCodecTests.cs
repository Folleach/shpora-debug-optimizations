using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JPEG.Maintenance.Legacy;
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
        
        [Test]
        public void HuffmanCodec_Encode_ShouldBeReturnSameValuesAsLegacyHuffmanCodec()
        {
            var huffmanResult = HuffmanCodec.Encode(encodeData, out var decodeTable, out var bitsCount);
            var huffmanResultLegacy = HuffmanCodec_Legacy.Encode(encodeData, out var decodeTableLegacy, out var bitsCountLegacy);

            huffmanResult.Should().BeEquivalentTo(huffmanResultLegacy);
            decodeTable.Keys.Select(x => x.Bits).Should().BeEquivalentTo(decodeTableLegacy.Keys.Select(x => x.Bits));
            decodeTable.Keys.Select(x => x.BitsCount).Should().BeEquivalentTo(decodeTableLegacy.Keys.Select(x => x.BitsCount));
            decodeTable.Values.Should().BeEquivalentTo(decodeTableLegacy.Values);
            bitsCount.Should().Be(bitsCountLegacy);
        }
        
        [Test]
        public void HuffmanCodec_Decode_ShouldBeReturnSameValuesAsLegacyHuffmanCodec()
        {
            var encodedData = HuffmanCodec.Encode(encodeData, out var decodeTable, out var bitsCount);
            var encodedDataLegacy = HuffmanCodec_Legacy.Encode(encodeData, out var decodeTableLegacy, out var bitsCountLegacy);

            var huffmanResultLegacy = HuffmanCodec_Legacy.Decode(encodedData, decodeTableLegacy, bitsCount);
            var huffmanResult = HuffmanCodec.Decode(encodedData, decodeTable, bitsCount);

            huffmanResult.Should().BeEquivalentTo(huffmanResultLegacy);
        }
    }
}