using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargestProductInSeries
{
    /// <summary>
    /// This class finds the largest product in a consecutive series of adjacent digits
    /// (0-9)
    /// </summary>
    class LargestProductInSeries
    {
        static void Main(string[] args)
        {
            const string SeriesString = @"73167176531330624919225119674426574742355349194934
                                    96983520312774506326239578318016984801869478851843
                                    85861560789112949495459501737958331952853208805511
                                    12540698747158523863050715693290963295227443043557
                                    66896648950445244523161731856403098711121722383113
                                    62229893423380308135336276614282806444486645238749
                                    30358907296290491560440772390713810515859307960866
                                    70172427121883998797908792274921901699720888093776
                                    65727333001053367881220235421809751254540594752243
                                    52584907711670556013604839586446706324415722155397
                                    53697817977846174064955149290862569321978468622482
                                    83972241375657056057490261407972968652414535100474
                                    82166370484403199890008895243450658541227588666881
                                    16427171479924442928230863465674813919123162824586
                                    17866458359124566529476545682848912883142607690042
                                    24219022671055626321111109370544217506941658960408
                                    07198403850962455444362981230987879927244284909188
                                    84580156166097919133875499200524063689912560717606
                                    05886116467109405077541002256983155200055935729725
                                    71636269561882670428252483600823257530420752963450";

            //Test that it works..
            int[] MaxSequenceTest = p_FindMaxAdjacentMultiplicandDigits(SeriesString, 4);
            p_PrintResults(MaxSequenceTest);

            //Now do the fun one...
            int[] MaxSequence = p_FindMaxAdjacentMultiplicandDigits(SeriesString, 13);
            p_PrintResults(MaxSequence);

            Console.ReadKey();
        }

        /// <summary>
        /// Prints the results of the calculations.
        /// </summary>
        public static void p_PrintResults(int[] DigitSequence)
        {
            int Product = p_MultiplyDigits(DigitSequence);
            string DigitString = string.Join("", DigitSequence);

            //print results!
            Console.WriteLine($"The maximum product for a {DigitSequence.Count()} consecutive digits is: {Product} and the consecutive digits are: {DigitString}.");
            Console.WriteLine();
        }

        /// <summary>
        /// Finds the largest product for a specified number of consecutive series of digits (0-9)
        /// in a larger series
        /// </summary>
        public static int[] p_FindMaxAdjacentMultiplicandDigits(string Series, int AdjacentDigitCount)
        {
            List<int> DigitList = new List<int>();

            //sanitize the digits and add them to the list
            foreach (char c in Series.ToCharArray())
            {
                //convert the unicode character to a digit or else skip if result is -1
                int Digit = (int)char.GetNumericValue(c);
                //if it is actually a digit...
                if (Digit != -1)
                {
                    DigitList.Add(Digit);
                }
            }

            ConcurrentDictionary<int[], int> DigitSequenceProductList = new ConcurrentDictionary<int[], int>();

            //multi-loop
            Parallel.For(0, DigitList.Count() - AdjacentDigitCount, i =>
            {
                //multiply the subset of digits
                int[] ConsecutiveDigitSubset = DigitList
                .Skip(i)
                .Take(AdjacentDigitCount)
                .ToArray();

                int Product = p_MultiplyDigits(ConsecutiveDigitSubset);

                //add to the list of all the products
                DigitSequenceProductList.TryAdd(ConsecutiveDigitSubset, Product);
            });

            //Find the largest product
            int MaxProduct = DigitSequenceProductList
                .Values
                .Max();

            //return the sequence of digits that produced it
            return DigitSequenceProductList
                .Where(r => r.Value == MaxProduct)
                .Select(r => r.Key)
                .FirstOrDefault();
        }

        /// <summary>
        /// Multiplies a list of digits
        /// </summary>
        public static int p_MultiplyDigits(int[] DigitList)
        {
            return DigitList
                .Aggregate(1, (a, b) => a * b);
        }
    }
}
