using System;
using System.Collections.Generic;
using FluentAssertions;
using JPEG.Maintenance.Legacy;
using JPEG.Utilities;
using NUnit.Framework;

namespace JPEG.Maintenance.Tests
{
    [TestFixture]
    public class MathExTests
    {
        [Test, Combinatorial]
        public void MathEx_Sum_ShouldBeReturnSameValuesAsMathExLegacy(
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
        
        [Test, Combinatorial]
        public void MathEx_SumByTwoVariables_ShouldBeReturnSameValuesAsMathExLegacy(
            [Values(-10, -1, 0, 1, 7)] int from1,
            [Values(-10, -1, 0, 1, 7)] int to1,
            [Values(-10, -1, 0, 1, 7)] int from2,
            [Values(-10, -1, 0, 1, 7)] int to2)
        {
            if (to1 - from1 < 0 || to2 - from2 < 0)
                return;
            var cache = new Dictionary<int, Dictionary<int, double>>();
            var random = new Random();

            double Function(int value1, int value2)
            {
                return value1 + value2;
            }

            var current = MathEx.SumByTwoVariables(from1, to1, from2, to2, Function);
            var legacy = MathEx_Legacy.SumByTwoVariables(from1, to1, from2, to2, Function);
            
            Assert.AreEqual(legacy, current);
        }

        [Test, Sequential]
        public void MathEx_LoopByTwoVariables_ShouldBeReturnSameValuesAsMathExLegacy(
            [Values(0, -9, 0, -7)] int from1,
            [Values(3, -4, 0,  3)] int to1,
            [Values(0, -9, 0, -7)] int from2,
            [Values(3, -4, 0,  3)] int to2)
        {
            if (to1 - from1 < 0 || to2 - from2 < 0)
                Assert.Fail("Invalid test. To should be more than from");
            
            var legacyCount = 0;
            var count = 0;
            
            var legacyTable = new Dictionary<Tuple<int, int, int, int>, HashSet<Tuple<int, int>>>();
            var table = new Dictionary<Tuple<int, int, int, int>, HashSet<Tuple<int, int>>>();

            MathEx_Legacy.LoopByTwoVariables(from1, to1, from2, to2, (x, y) =>
            {
                legacyCount++;
                var key = Tuple.Create(from1, to1, from2, to2);
                if (!legacyTable.TryGetValue(key, out var set))
                    legacyTable.Add(key, set = new HashSet<Tuple<int, int>>());
                set.Add(Tuple.Create(x, y));
            });

            MathEx.LoopByTwoVariables(from1, to1, from2, to2, (x, y) =>
            {
                count++;
                var key = Tuple.Create(from1, to1, from2, to2);
                if (!table.TryGetValue(key, out var set))
                    table.Add(key, set = new HashSet<Tuple<int, int>>());
                set.Add(Tuple.Create(x, y));
            });
            
            Assert.AreEqual(legacyCount, count);
            table.Should().BeEquivalentTo(legacyTable);
        }
    }
}
