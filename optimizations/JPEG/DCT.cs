using System;
using System.Runtime.CompilerServices;
using JPEG.Utilities;

namespace JPEG
{
	public class DCT
	{
		private static readonly double OneDivSqrtTwo = 1 / Math.Sqrt(2);
		
		public static double[,] DCT2D(double[,] input, int subX, int subY)
		{
			var height = input.GetLength(0);
			var width = input.GetLength(1);
			var coeffs = new double[width, height];
            
			for (var u = 0; u < width; u++)
			{
				for (var v = 0; v < height; v++)
				{
					var sum = 0d;
					for (var x = 0; x < width; x += subX)
					{
						for (var y = 0; y < height; y += subY)
						{
							var b = Math.Cos(((2d * x + 1d) * u * Math.PI) / (2 * width));
							var c = Math.Cos(((2d * y + 1d) * v * Math.PI) / (2 * height));

							sum += input[x, y] * b * c;
						}
					}

					coeffs[u, v] = sum * Beta(height, width) * Alpha(u) * Alpha(v);
				}
			}

			return coeffs;
		}

		public static void IDCT2D(double[,] coeffs, double[,] output, int subX, int subY)
		{
			var height = coeffs.GetLength(1);
			var width = coeffs.GetLength(0);
			for(var x = 0; x < height; x++)
			{
				for(var y = 0; y < width; y++)
				{
					var sum = 0d;
					for (var u = 0; u < height; u += subX)
					{
						for (var v = 0; v < width; v += subY)
						{
							sum += BasisFunction(coeffs[u, v], u, v, x, y, width, height)
							       * (u == 0 ? OneDivSqrtTwo : 1)
							       * (v == 0 ? OneDivSqrtTwo : 1);
						}
					}

					output[x, y] = sum * (1d / height + 1d / width);
				}
			}
		}

		private static double BasisFunction(double a, double u, double v, double x, double y, int height, int width)
		{
			var b = Math.Cos(((2d * x + 1d) * u * Math.PI) / (2 * width));
			var c = Math.Cos(((2d * y + 1d) * v * Math.PI) / (2 * height));

			return a * b * c;
		}

		private static double Alpha(int u)
		{
			if(u == 0)
				return 1 / Math.Sqrt(2);
			return 1;
		}

		private static double Beta(int height, int width)
		{
			return 1d / width + 1d / height;
		}
	}
}