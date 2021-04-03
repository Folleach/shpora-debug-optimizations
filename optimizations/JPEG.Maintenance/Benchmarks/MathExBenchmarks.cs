using BenchmarkDotNet.Attributes;
using JPEG.Maintenance.Legacy;
using JPEG.Utilities;

namespace JPEG.Maintenance.Benchmarks
{
    [DisassemblyDiagnoser]
    public class MathExBenchmarks
    {
        private const int N = 1000000;
        private const int N_ForSumByTwoVariables = 100;
        private const int N_ForLoopByTwoVariables = 1000;
        
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

        [Benchmark]
        public void MathExLegacy_SumByTwoVariables()
        {
            MathEx_Legacy.SumByTwoVariables(0, N_ForSumByTwoVariables, 0, N_ForSumByTwoVariables, (x, y) => 0.5d);
        }
        
        [Benchmark]
        public void MathEx_SumByTwoVariables()
        {
            MathEx.SumByTwoVariables(0, N_ForSumByTwoVariables, 0, N_ForSumByTwoVariables, (x, y) => 0.5d);
        }
        
        [Benchmark]
        public void MathExLegacy_LoopByTwoVariables()
        {
            MathEx_Legacy.LoopByTwoVariables(0, N_ForLoopByTwoVariables, 0, N_ForLoopByTwoVariables, (x, y) =>
            {
            });
        }
        
        [Benchmark]
        public void MathEx_LoopByTwoVariables()
        {
            MathEx.LoopByTwoVariables(0, N_ForLoopByTwoVariables, 0, N_ForLoopByTwoVariables, (x, y) =>
            {
            });
        }
    }
}
