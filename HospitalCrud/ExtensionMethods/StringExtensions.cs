using System.Text.RegularExpressions;

namespace HospitalCrud.ExtensionMethods
{
	/// <summary>
	/// Useful extension methods to operate on strings
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Returns a string that contains only the decimal digits (0-9) of the original string
		/// </summary>
		public static string? KeepOnlyDigits(this string? str)
		{
			return str == null ? str : Regex.Replace(str, "[^0-9]", "");
		}
	}
}
