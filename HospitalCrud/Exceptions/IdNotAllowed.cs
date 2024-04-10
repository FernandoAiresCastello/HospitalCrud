using HospitalCrud.Util;

namespace HospitalCrud.Exceptions
{
	/// <summary>
	/// The exception thrown when an unsolicited id is present in a DTO
	/// </summary>
	public class IdNotAllowed : Exception
	{
		public IdNotAllowed() : base(ValidationMessages.IdNotAllowed)
		{
		}
	}
}
