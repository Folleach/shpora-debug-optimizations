using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace JPEG.Maintenance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
                .Run(args,
                    ManualConfig.Create(DefaultConfig.Instance)
                        .WithOptions(ConfigOptions.DisableOptimizationsValidator));
        }
    }
}
