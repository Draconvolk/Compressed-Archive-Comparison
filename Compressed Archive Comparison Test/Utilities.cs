using CompressedArchiveComparison.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace CompressedArchiveComparisonTests
{
    public static class Utilities
    {
        /// <summary>
        /// Extension Method to flatten an IEnumerable<string> to a string for comparison
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string FlattenToString(this IEnumerable<string> list, string separator = ", ")
            => list.Aggregate(new StringBuilder(), (current, next) => current.Append(separator).Append(next)).ToString().Trim().Trim([.. separator]);

        public static void AssertAreEqual(List<string> expectedResult, IEnumerable<string> result, bool debug = false)
        {
            var expectedResultsFlattened = expectedResult.FlattenToString(Environment.NewLine);
            var actualResultsFlattened = result.FlattenToString(Environment.NewLine);

            Assert.IsNotNull(result);
            var resultList = result.ToList();
            if (debug)
            {
                Console.WriteLine("Expected:");
                Console.WriteLine(expectedResultsFlattened);
                Console.WriteLine();
                Console.WriteLine("Actual:");
                Console.WriteLine(actualResultsFlattened);
                Console.WriteLine();

                for (var x = 0; x < expectedResult.Count && x < resultList.Count; x++)
                {
                    Console.WriteLine(expectedResult[x] + "\t\t" + resultList[x]);
                }
                Console.WriteLine();
            }

            Assert.AreEqual(expectedResultsFlattened, actualResultsFlattened);
        }

        public static T? GetInjectedObject<T>()
        {
            var host = CompressionHostBuilder.CreateHostBuilder(true).Build();

            Assert.IsNotNull(host);
            return host.Services.GetService<T>();
        }
    }
}
