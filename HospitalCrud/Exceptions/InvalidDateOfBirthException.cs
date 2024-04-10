namespace HospitalCrud.Exceptions
{
    /// <summary>
    /// The exception thrown when an attempt is made to include a patient with an invalid date of birth
    /// </summary>
    public class InvalidDateOfBirthException : Exception
    {
        public InvalidDateOfBirthException(DateTime? dateOfBirth) : base($"Data de nascimento inválida")
        {
        }
    }
}
