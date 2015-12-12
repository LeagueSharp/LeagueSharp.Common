namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The weighted random class.
    /// </summary>
    public static class WeightedRandom
    {
        #region Static Fields

        /// <summary>
        ///     The random instance.
        /// </summary>
        public static Random Random = new Random(Utils.TickCount);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the next random value.
        /// </summary>
        /// <param name="min">
        ///     The minimum value.
        /// </param>
        /// <param name="max">
        ///     The maximum value.
        /// </param>
        /// <returns>
        ///     The random value between min to max.
        /// </returns>
        public static int Next(int min, int max)
        {
            var list = new List<int>();
            list.AddRange(Enumerable.Range(min, max));

            var mean = list.Average();
            var stdDev = list.StandardDeviation();

            var v1 = Random.NextDouble();
            var v2 = Random.NextDouble();

            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(v1)) * Math.Sin(2.0 * Math.PI * v2);
            return (int)(mean + stdDev * randStdNormal);
        }

        /// <summary>
        ///     The standard deviation.
        /// </summary>
        /// <param name="values">
        ///     The values.
        /// </param>
        /// <returns>
        ///     The returned deviation.
        /// </returns>
        public static double StandardDeviation(this IEnumerable<int> values)
        {
            var enumerable = values as int[] ?? values.ToArray();
            var avg = enumerable.Average();
            return Math.Sqrt(enumerable.Average(v => Math.Pow(v - avg, 2)));
        }

        #endregion
    }
}