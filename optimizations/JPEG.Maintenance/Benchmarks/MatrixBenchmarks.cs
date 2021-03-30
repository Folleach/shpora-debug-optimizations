using System.Drawing;
using JPEG.Images;
using BenchmarkDotNet.Attributes;
using JPEG.Maintenance.Legacy;

namespace JPEG.Maintenance.Benchmarks
{
    [DisassemblyDiagnoser]
    public class MatrixBenchmarks
    {
        private Bitmap image = new Bitmap(24, 19);

        [Benchmark]
        public void BitmapToMatrix()
        {
            Matrix matrix = (Matrix)image;
        }

        [Benchmark]
        public void BitmapToMatrix_Legacy()
        {
            Matrix_Legacy matrix = (Matrix_Legacy)image;
        }
    }
}