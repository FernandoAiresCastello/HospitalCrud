using HospitalCrud.Model;

namespace HospitalCrud.Repositories
{
	/// <summary>
	/// Interface for a data repository containing patients
	/// </summary>
	public interface IPatientRepository : IRepository<Patient>
	{
		/// <summary>
		/// Retrieve a patient by their CPF number
		/// </summary>
		/// <param name="cpf">The CPF number to look for</param>
		/// <returns>The patient with the specified CPF, if found</returns>
		Task<Patient> GetPatientByCpf(string cpf);
	}
}
