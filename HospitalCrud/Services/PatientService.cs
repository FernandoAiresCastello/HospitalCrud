using HospitalCrud.DTO;
using HospitalCrud.Exceptions;
using HospitalCrud.Model;
using HospitalCrud.Repositories;
using HospitalCrud.Util;
using HospitalCrud.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace HospitalCrud.Services
{
    public class PatientService : IPatientService
	{
		private readonly IPatientRepository patientRepository;

		public PatientService(IPatientRepository patientRepository)
		{
			this.patientRepository = patientRepository;
		}

		public async Task<Patient> AddNewPatient(Patient patient)
		{
            try
			{
				if (patient.Id.HasValue)
					throw new InvalidOperationException("Não é permitido informar o id ao cadastrar um novo paciente");

				AssertValidDateOfBirthOrElseExcept(patient.DateOfBirth);
                patient.DateOfBirth = patient.DateOfBirth.ToUtc();

                await patientRepository.Add(patient);

				return patient;
			}
			catch (DbUpdateException ex)
			{
				if (PostgresExceptionIdentifier.IsUniqueConstraintViolationError(ex, "IX_Patient_Cpf"))
					throw new DuplicateCpfException(patient.Cpf!);

				throw;
			}
		}

		private void AssertValidDateOfBirthOrElseExcept(DateTime? dateOfBirth)
		{
			if (!dateOfBirth.IsValidDateOfBirthOrNull())
				throw new InvalidDateOfBirthException(dateOfBirth);
		}

		public async Task<ICollection<Patient>> GetAllPatients()
		{
			return await patientRepository.GetAll();
		}

		public async Task<Patient> GetPatientById(int id)
		{
			return await patientRepository.GetById(id);
		}

		public async Task<Patient> GetPatientByCpf(string cpf)
		{
			return await patientRepository.GetPatientByCpf(cpf);
		}

		public async Task UpdatePatient(Patient newData)
		{
			if (!newData.Id.HasValue)
				throw new MissingIdException();

			var currentData = await GetPatientById(newData.Id.Value);

			currentData.FirstName = newData.FirstName;
			currentData.LastName = newData.LastName;
			currentData.Phone = newData.Phone;
			currentData.Email = newData.Email;
			currentData.Cpf = newData.Cpf;

			AssertValidDateOfBirthOrElseExcept(newData.DateOfBirth);
			currentData.DateOfBirth = newData.DateOfBirth.ToUtc();

			await patientRepository.Update(currentData);
		}

		public async Task UpdatePatient(PatientUpdateDTO dto)
		{
			var patient = await GetPatientById(dto.Id);

			if (dto.FirstName != null)
				patient.FirstName = dto.FirstName;
			if (dto.LastName != null)
				patient.LastName = dto.LastName;
			if (dto.Phone != null)
				patient.Phone = dto.Phone;
			if (dto.Email != null)
				patient.Email = dto.Email;
			if (dto.Cpf != null)
				patient.Cpf = dto.Cpf;

            if (dto.DateOfBirth != null)
            {
                AssertValidDateOfBirthOrElseExcept(patient.DateOfBirth);
                patient.DateOfBirth = dto.DateOfBirth.ToUtc();
            }

            await patientRepository.Update(patient);
		}

		public async Task DeletePatientById(int id)
		{
			await patientRepository.Delete(id);
		}
	}
}
