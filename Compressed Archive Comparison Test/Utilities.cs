namespace CompressedArchiveComparisonTests
{
	public static class Utilities
	{
		/// <summary>
		/// Extension Method to flatten an IEnumerable<string> to a string for comparison
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static string FlattenToString(this IEnumerable<string> list) => list.Aggregate((a, b) => $"{a}, {b}").Trim();
	}
}
