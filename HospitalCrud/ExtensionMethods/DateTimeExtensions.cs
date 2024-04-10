using System;

namespace HospitalCrud.ExtensionMethods
{
    /// <summary>
    /// Useful extension methods to the System.DateTime class
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert a DateTime object to UTC format
        /// </summary>
        /// <param name="dateTime">The DateTime object to convert</param>
        /// <returns>An equivalent DateTime object specified with the UTC kind</returns>
        public static DateTime? ToUtc(this DateTime? dateTime)
        {
            //return dateTime.HasValue ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) : null;
            return dateTime.HasValue ? dateTime.Value.ToUniversalTime() : null;
        }

        /// <summary>
        /// Check if the provided DateTime represents a valid date of birth, or null
        /// </summary>
        /// <param name="dateOfBirth">The DateTime object to check</param>
        /// <returns>True if the provided date is a valid date of birth or null; false otherwise</returns>
        public static bool IsValidDateOfBirthOrNull(this DateTime? dateOfBirth)
        {
            if (!dateOfBirth.HasValue)
                return true;

            return dateOfBirth != DateTime.MinValue && dateOfBirth < DateTime.Now;
        }
    }
}
