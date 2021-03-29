using System;
using System.Collections.Generic;
using JPEG.Maintenance.Legacy;
using JPEG.Utilities;
using NUnit.Framework;

namespace JPEG.Maintenance.Tests
{
    [TestFixture]
    public class MathExTests
    {
        [Test, Combinatorial]
        public void NewMathEx_Sum_ShouldBeReturnSameValuesAsMathEx(
            [Values(-10, -5, -1, 0, 1, 3, 7)] int from,
            [Values(-10, -5, -1, 0, 1, 3, 7)] int to)
        {
            if (to - from < 0)
                return;
            var cache = new Dictionary<int, double>();
            var random = new Random();

            double Function(int value)
            {
                return cache.ContainsKey(value)
                    ? cache[value]
                    : cache[value] = random.NextDouble() * 10;
            }
            
            var current = MathEx.Sum(from, to, Function);
            var legacy = MathEx_Legacy.Sum(from, to, Function);
            
            Assert.AreEqual(legacy, current);
        }
    }
}
