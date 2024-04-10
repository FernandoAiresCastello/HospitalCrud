using Npgsql;

namespace HospitalCrud.Util
{
	/// <summary>
	/// Use this class to identify exceptions which are specific to the Postgres backend.
	/// </summary>
	public static class PostgresExceptionIdentifier
	{
		private static class ErrorCodes
		{
			// Complete reference of error codes:
			// https://www.postgresql.org/docs/current/errcodes-appendix.html

			public const string UndefinedTable = "42P01";
			public const string ValueTooLong = "22001";
			public const string UniqueConstraintViolation = "23505";
		}

		/// <summary>
		/// Check if the exception indicates a "value too long" error.
		/// </summary>
		/// 
		/// <param name="ex">The exception to be identified</param>
		/// 
		/// <returns>
		/// True if the exception is caused by a "value too long" error; false otherwise.
		/// </returns>
		public static bool IsValueTooLongException(Exception ex)
		{
			return	ex.InnerException is PostgresException pgex &&
					pgex.SqlState == ErrorCodes.ValueTooLong;
		}

		/// <summary>
		/// Check if the exception indicates a "unique constraint violation error" related to the constraint name specified.
		/// </summary>
		/// 
		/// <param name="ex">The exception to be identified</param>
		/// <param name="constraint">The constraint name to check for</param>
		/// 
		/// <returns>
		/// True if the exception is caused by a "unique contraint violation error"; false otherwise.
		/// </returns>
		public static bool IsUniqueConstraintViolationError(Exception ex, string constraint)
		{
			return  ex.InnerException is PostgresException pgex && 
					pgex.SqlState == ErrorCodes.UniqueConstraintViolation &&
					pgex.ConstraintName == constraint;
		}

		/// <summary>
		/// Check if the exception indicates that a database table does not exist
		/// </summary>
		/// 
		/// <param name="ex">The exception to be identified</param>
		/// 
		/// <returns>
		/// True if the exception is caused by a missing database table; false otherwise.
		/// </returns>
		public static bool IsUndefinedTable(Exception ex)
		{
			return	ex is PostgresException pgex &&
					pgex.SqlState == ErrorCodes.UndefinedTable;
		}
	}
}
