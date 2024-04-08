namespace HospitalCrud.Exceptions
{
	/// <summary>
	/// The exception thrown when a required id is missing from a DTO
	/// </summary>
	public class MissingIdException : Exception
	{
		public MissingIdException() : base("O id obrigatório para a operação solicitada não foi informado")
		{
		}
	}
}
