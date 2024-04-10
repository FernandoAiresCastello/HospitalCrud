using HospitalCrud.DTO;
using HospitalCrud.Model;

namespace HospitalCrud.Services
{
	/// <summary>
	/// Interface for a service that handles patients
	/// </summary>
	public interface IPatientService
	{
		Task<Patient> AddNewPatient(Patient patient);
		Task<ICollection<Patient>> GetAllPatients();
		Task<Patient> GetPatientById(int id);
		Task<Patient> GetPatientByCpf(string cpf);
		Task UpdatePatientFromApi(PatientUpdateDTO dto);
		Task UpdatePatientFromView(Patient patient);
		Task DeletePatientById(int id);
	}
}
