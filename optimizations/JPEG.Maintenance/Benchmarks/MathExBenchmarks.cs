using BenchmarkDotNet.Attributes;
using JPEG.Maintenance.Legacy;
using JPEG.Utilities;

namespace JPEG.Maintenance.Benchmarks
{
    [DisassemblyDiagnoser]
    public class MathExBenchmarks
    {
        private const int N = 1000000;
        
        [Benchmark]
        public void MathExLegacy_Sum()
        {
            MathEx_Legacy.Sum(0, N, x => 0.5d);
        }

        [Benchmark]
        public void MathEx_Sum()
        {
            MathEx.Sum(0, N, x => 0.5d);
        }
    }
}
