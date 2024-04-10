using HospitalCrud.Model;
using Swashbuckle.AspNetCore.Filters;

namespace HospitalCrud.Util
{
	/// <summary>
	/// Class used by Swagger as an example provider of Patient models
	/// </summary>
	public class PatientExample : IExamplesProvider<Patient>
	{
		public Patient GetExamples()
		{
			return new Patient
			{
				FirstName = "Fulano",
				LastName = "de Tal",
				Cpf = "69819324076",
				Email = "email@test.com",
				Phone = "(77)7777-7777",
				DateOfBirth = DateTime.ParseExact("10/09/1988", "dd/MM/yyyy", null)
			};
		}
	}
}
