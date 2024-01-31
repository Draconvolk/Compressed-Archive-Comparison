namespace CompressedArchiveComparisonTests
{
	public static class Utilities
	{
		/// <summary>
		/// Extension Method to flatten an IEnumerable<string> to a string for comparison
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static string FlattenToString(this IEnumerable<string> list, string separator=", ") => list.Aggregate((a, b) => $"{a}{separator}{b}").Trim();

		public static void AssertAreEqual(List<string> expectedResult, IEnumerable<string> result, bool debug = false)
		{
			var expectedResultsFlattened = expectedResult.FlattenToString(Environment.NewLine);
			var actualResultsFlattened = result.FlattenToString(Environment.NewLine);

			if (debug)
			{
				Console.WriteLine("Expected:");
				Console.WriteLine(expectedResultsFlattened);
				Console.WriteLine();
				Console.WriteLine("Actual:");
				Console.WriteLine(actualResultsFlattened);
			}

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResultsFlattened, actualResultsFlattened);
		}
	}
}
