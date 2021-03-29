using BenchmarkDotNet.Attributes;

namespace JPEG.Maintenance.Benchmarks
{
    [DisassemblyDiagnoser]
    public class MathExBenchmarks
    {
        [Benchmark]
        public void MathEx_Sum()
        {
        }

        [Benchmark]
        public void NewMathEx_Sum()
        {
        }
    }
}
