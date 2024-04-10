namespace HospitalCrud.Exceptions
{
	/// <summary>
	/// The exception thrown when a value is too long for a database column
	/// </summary>
	public class ValueTooLongException : Exception
	{
		public ValueTooLongException(string message) : base(message)
		{
		}
	}
}
