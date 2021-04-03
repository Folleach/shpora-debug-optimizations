using System;

namespace JPEG.Utilities
{
    public static class MathEx
    {
        public static double Sum(int from, int to, Func<int, double> function)
        {
            var result = 0d;
            for (; from < to; from++)
                result += function(from);
            
            return result;
        }

        public static double SumByTwoVariables(int from1, int to1, int from2, int to2, Func<int, int, double> function)
        {
            var result = 0d;
            for (; from1 < to1; from1++)
            {
                for (var i = from2; i < to2; i++)
                    result += function(from1, i);
            }

            return result;
        }


        public static void LoopByTwoVariables(int from1, int to1, int from2, int to2, Action<int, int> function)
        {
            for (; from1 < to1; from1++)
            {
                for (var i = from2; i < to2; i++)
                    function(from1, i);
            }
        }
    }
}