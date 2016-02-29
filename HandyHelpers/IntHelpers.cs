using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandyHelpers
{
    /// <summary>
    /// Helper methods for integers
    /// </summary>
    public static class IntHelpers
    {
        /// <summary>
        /// Spin for a given number of times and invoke the callback function on each iteration
        /// </summary>
        /// <param name="iterations">Expected number of iterations</param>
        /// <param name="callback">
        ///   The callback function, which takes the current iteration starting from zero and returns whether subsequent iteration shall be executed
        /// </param>
        /// <returns>How many iterations actually executed</returns>
        public static int Times(this int iterations, Func<int, bool> callback)
        {
            if (iterations <= 0)
            {
                return 0;
            }

            int i = 0;
            while (i < iterations)
            {
                if (!callback(i++))
                {
                    break;
                }
            }

            return i;
        }
    }
}
