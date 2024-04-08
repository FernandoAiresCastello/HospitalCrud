namespace HospitalCrud.Exceptions
{
	/// <summary>
	/// The exception thrown when an attempt is made to include a patient with a CPF that already exists
	/// </summary>
	public class DuplicateCpfException : Exception
	{
		public DuplicateCpfException(string cpf) : base($"Já existe um paciente com o CPF informado: {cpf}")
		{
		}
	}
}
