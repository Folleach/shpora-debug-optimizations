using BenchmarkDotNet.Attributes;
using JPEG.Images;
using JPEG.Maintenance.Legacy;

namespace JPEG.Maintenance.Benchmarks
{
    [DisassemblyDiagnoser()]
    public class PixelBenchmarks
    {
        [Benchmark]
        public void Constructor_Legacy()
        {
            new Pixel_Legacy(0, 0, 0, PixelFormat.RGB);
        }
        
        [Benchmark]
        public void Constructor()
        {
            new Pixel(0, 0, 0, PixelFormat.RGB);
        }
    }
}