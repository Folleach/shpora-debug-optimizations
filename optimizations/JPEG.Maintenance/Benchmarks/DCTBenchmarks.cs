using BenchmarkDotNet.Attributes;
using JPEG.Maintenance.Legacy;

namespace JPEG.Maintenance.Benchmarks
{
    [DisassemblyDiagnoser()]
    public class DCTBenchmarks
    {
        private double[,] input;
        
        [GlobalSetup]
        public void Setup()
        {
            input = new double[,]
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
        }
        
        [Benchmark]
        public void DCT2D()
        {
            DCT.DCT2D(input, 1, 1);
        }
        
        [Benchmark]
        public void DCT2D_Legacy()
        {
            DCT_Legacy.DCT2D(input);
        }
    }
}