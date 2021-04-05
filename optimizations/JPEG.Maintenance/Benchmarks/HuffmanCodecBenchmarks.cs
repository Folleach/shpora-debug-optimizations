using System;
using BenchmarkDotNet.Attributes;
using JPEG.Maintenance.Legacy;

namespace JPEG.Maintenance.Benchmarks
{
    [DisassemblyDiagnoser()]
    public class HuffmanCodecBenchmarks
    {
        private const int N = 30;
        private byte[] data;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();
            data = new byte[N];
            for (var i = 0; i < N; i++)
                data[i] = (byte)random.Next(20);
        }
        
        [Benchmark]
        public void HuffmanCodec_Encode()
        {
            HuffmanCodec.Encode(data, out _, out _);
        }

        [Benchmark]
        public void HuffmanCodecLegacy_Encode()
        {
            HuffmanCodec_Legacy.Encode(data, out _, out _);
        }
    }
}