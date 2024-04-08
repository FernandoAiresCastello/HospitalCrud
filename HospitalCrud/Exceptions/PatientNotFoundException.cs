namespace HospitalCrud.Exceptions
{
	/// <summary>
	/// The exception thrown when a specific patient is not found in the data repository
	/// </summary>
	public class PatientNotFoundException : Exception
	{
		public PatientNotFoundException(object key)
			: base($"Paciente não foi encontrado com a chave: {key}")
		{
		}
	}
}
