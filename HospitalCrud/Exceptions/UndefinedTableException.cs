using Npgsql;

namespace HospitalCrud.Exceptions
{
	/// <summary>
	/// The exception thrown when a database table is not found
	/// </summary>
	public class UndefinedTableException : Exception
	{
		public UndefinedTableException(Exception ex) : 
			base($"A tabela especificada não existe no banco de dados: {ex.Message}")
		{
		}
	}
}
